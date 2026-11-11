using System;
using System.Collections.Concurrent;
using System.Reflection;
using Axlebolt.RpcSupport.Protobuf;

namespace Axlebolt.RpcSupport
{
	public class EventListenerProxy
	{
		private class EventEndPoint
		{
			private readonly string _listenerName;

			private readonly string _eventName;

			public EventEndPoint(string listenerName, string eventName)
			{
				_listenerName = listenerName;
				_eventName = eventName;
			}

			public override int GetHashCode()
			{
				int hashCode = _listenerName.GetHashCode();
				return 31 * hashCode + _eventName.GetHashCode();
			}

			public override bool Equals(object o)
			{
				if (this == o)
				{
					return true;
				}
				if (o == null || GetType() != o.GetType())
				{
					return false;
				}
				EventEndPoint eventEndPoint = (EventEndPoint)o;
				return _listenerName.Equals(eventEndPoint._listenerName) && _eventName.Equals(eventEndPoint._eventName);
			}
		}

		private readonly ConcurrentDictionary<EventEndPoint, MethodInfo> _eventMethods = new ConcurrentDictionary<EventEndPoint, MethodInfo>();

		private readonly object _eventListener;

		public EventListenerProxy(object eventListener)
		{
			_eventListener = eventListener;
			EventListener attribute = AttributeUtils.GetAttribute<EventListener>(eventListener.GetType());
			if (attribute == null)
			{
				Logger.Error(string.Concat(eventListener, " is not [EventListener]"));
				throw new Exception("Incorrect eventListener");
			}
			MethodInfo[] methods = _eventListener.GetType().GetMethods();
			foreach (MethodInfo methodInfo in methods)
			{
				Event attribute2 = AttributeUtils.GetAttribute<Event>(methodInfo);
				if (attribute2 != null)
				{
					EventEndPoint eventEndPoint = new EventEndPoint(attribute.name, attribute2.name);
					if (!_eventMethods.TryAdd(eventEndPoint, methodInfo))
					{
						throw new Exception("Duplicate event " + eventEndPoint);
					}
					Logger.Debug("Event " + attribute2.name + " registered");
				}
				else
				{
					Logger.Debug(string.Concat("Method ", methodInfo, " skip"));
				}
			}
		}

		public bool HasEvent(RpcMessage.EventResponse eventResponse)
		{
			return _eventMethods.ContainsKey(new EventEndPoint(eventResponse.ListenerName, eventResponse.EventName));
		}

		public void ExecuteEvent(RpcMessage.EventResponse eventResponse)
		{
			EventEndPoint eventEndPoint = new EventEndPoint(eventResponse.ListenerName, eventResponse.EventName);
			MethodInfo value;
			if (_eventMethods.TryGetValue(eventEndPoint, out value))
			{
				ParameterInfo[] parameters = value.GetParameters();
				if (eventResponse.Params.Count == parameters.Length)
				{
					object[] array = new object[parameters.Length];
					try
					{
						for (int i = 0; i < parameters.Length; i++)
						{
							ParameterInfo parameterInfo = value.GetParameters()[i];
							RpcMessage.BinaryValue binaryValue = eventResponse.Params[i];
							array[i] = ProtoReflectionUtils.FromBinaryValue(binaryValue, parameterInfo.ParameterType);
						}
					}
					catch (Exception ex)
					{
						Logger.Error("Can't parse event parameter types, " + ex);
						return;
					}
					try
					{
						value.Invoke(_eventListener, array);
						return;
					}
					catch (Exception ex2)
					{
						Logger.Error("Event invocation failed, " + ex2);
						return;
					}
				}
				Logger.Debug(string.Concat("Can't process event ", eventEndPoint, ", incorrect arguments size. Expect ", parameters.Length, ", received ", eventResponse.Params.Count));
			}
			else
			{
				Logger.Debug(string.Concat("Event ", eventEndPoint, " not found"));
			}
		}
	}
}
