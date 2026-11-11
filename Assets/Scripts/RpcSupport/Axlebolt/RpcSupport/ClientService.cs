using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Axlebolt.RpcSupport.Protobuf;

namespace Axlebolt.RpcSupport
{
	public class ClientService
	{
		public delegate void EventHandler();

		private class ReceiveException : SocketException
		{
		}

		private static readonly object StateLock = new object();

		private Thread _sendThread;

		private Thread _receiveThread;

		private Thread _monitoringThread;

		private readonly ConcurrentQueue<RpcMessage.RpcRequest> _requests;

		private readonly ConcurrentDictionary<string, RpcMessage.RpcResponse> _responses;

		private TcpClient _tcpClient;

		private readonly RpcExceptionProvider _exceptionProvider;

		private readonly List<EventListenerProxy> _eventListeners = new List<EventListenerProxy>();

		public ConnectionState ConnectionState { get; private set; } = ConnectionState.Disconnected;


		public event EventHandler Disconnected;

		public event EventHandler ConnectionFailed;

		public ClientService(Assembly assembly)
		{
			_exceptionProvider = new RpcExceptionProvider(assembly);
			_requests = new ConcurrentQueue<RpcMessage.RpcRequest>();
			_responses = new ConcurrentDictionary<string, RpcMessage.RpcResponse>();
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void Connect(string host, int port)
		{
			lock (StateLock)
			{
				if (ConnectionState == ConnectionState.Connected)
				{
					return;
				}
				try
				{
					_tcpClient = new TcpClient();
					IAsyncResult asyncResult = _tcpClient.BeginConnect(host, port, null, null);
					if (!asyncResult.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5.0)))
					{
						ConnectionState = ConnectionState.ConnectionFailed;
						throw new ConnectionFailedException(string.Format("Can't connect to {0}:{1}", host, port));
					}
					_tcpClient.EndConnect(asyncResult);
					ConnectionState = ConnectionState.Connected;
				}
				catch (Exception innerException)
				{
					ConnectionState = ConnectionState.ConnectionFailed;
					throw new ConnectionFailedException(string.Format("Can't connect to {0}:{1}", host, port), innerException);
				}
				StartSendThread();
				StartReceiveThread();
				StartMonitoringThread();
			}
		}

		private void ConnectionFail()
		{
			lock (StateLock)
			{
				Logger.Debug("Connection Fail");
				ConnectionState = ConnectionState.ConnectionFailed;
				_tcpClient.Close();
				StopReceiveThread();
				StopSendThread();
				OnConnectionFailed();
			}
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void Disconnect()
		{
			Logger.Debug("Disconnect");
			lock (StateLock)
			{
				if (ConnectionState != 0)
				{
					return;
				}
				ConnectionState = ConnectionState.Disconnected;
				_tcpClient.Close();
				StopReceiveThread();
				StopSendThread();
			}
			StopMonitoringThread();
			OnDisconnected();
		}

		public T Create<T>()
		{
			return (T)new RpcServiceProxy(typeof(T), this, _exceptionProvider).GetTransparentProxy();
		}

		public void RegisterEventListener(object eventListener)
		{
			_eventListeners.Add(new EventListenerProxy(eventListener));
		}

		private void OnEventReceived(RpcMessage.EventResponse eventResponse)
		{
			foreach (EventListenerProxy eventListener in _eventListeners)
			{
				if (eventListener.HasEvent(eventResponse))
				{
					try
					{
						eventListener.ExecuteEvent(eventResponse);
					}
					catch (Exception ex)
					{
						Logger.Error(string.Concat("EventListener fail, (", eventListener, "), ", ex));
					}
				}
			}
		}

		private void StartSendThread()
		{
			_sendThread = new Thread(Send);
			_sendThread.Start();
		}

		private void StopSendThread()
		{
			if (_sendThread.IsAlive)
			{
				_sendThread.Join();
			}
			_sendThread = null;
		}

		private void StartReceiveThread()
		{
			_receiveThread = new Thread(Receive);
			_receiveThread.Start();
		}

		private void StopReceiveThread()
		{
			if (_receiveThread.IsAlive)
			{
				_receiveThread.Join();
			}
			_receiveThread = null;
		}

		private void StartMonitoringThread()
		{
			_monitoringThread = new Thread(Monitoring);
			_monitoringThread.Start();
		}

		private void StopMonitoringThread()
		{
			if (_monitoringThread.IsAlive)
			{
				_monitoringThread.Join();
			}
			_monitoringThread = null;
		}

		private void Monitoring()
		{
			Logger.Debug("MonotoringThread started");
			while (IsActive())
			{
				Thread.Sleep(50);
			}
			Logger.Debug("MonotoringThread stopped");
		}

		private bool IsActive()
		{
			lock (StateLock)
			{
				if (ConnectionState != 0)
				{
					return false;
				}
				if (_tcpClient.Client.Connected && _sendThread.IsAlive && _receiveThread.IsAlive)
				{
					return true;
				}
				ConnectionFail();
				return false;
			}
		}

		private void Send()
		{
			Logger.Debug("SendThread started");
			while (true)
			{
				try
				{
					if (ConnectionState != 0 || IsDebugBreak())
					{
						break;
					}
					RpcMessage.RpcRequest result;
					if (!_requests.TryDequeue(out result))
					{
						Thread.Sleep(50);
						continue;
					}
					try
					{
						byte[] body = ProtoReflectionUtils.ToBytes(result);
						body = WrapHeader(body);
						Logger.Debug("Write request " + body.Length + " bytes");
						_tcpClient.GetStream().Write(body, 0, body.Length);
						Logger.Debug("Request " + result.ServiceName + "." + result.MethodName + " success");
					}
					catch (SocketException msg)
					{
						_responses[result.Id] = null;
						Logger.Error(msg);
						goto end_IL_000d;
					}
					catch (Exception)
					{
						_responses[result.Id] = null;
						throw;
					}
					continue;
					end_IL_000d:;
				}
				catch (ThreadAbortException ex2)
				{
					Logger.Error("SendThread aborted, " + ex2);
				}
				catch (Exception ex3)
				{
					Logger.Error("SendThread fatal exception: " + ex3);
				}
				break;
			}
			ClearRequests();
			Logger.Debug("SendThread stopped");
		}

		private static bool IsDebugBreak()
		{
			if (!RpcDebugHelper.DebugDisconnect)
			{
				return false;
			}
			RpcDebugHelper.Reset();
			Logger.Debug("RpcDebugHelper refuse connection");
			return true;
		}

		private void ClearRequests()
		{
			RpcMessage.RpcRequest result;
			while (_requests.TryDequeue(out result))
			{
				_responses[result.Id] = null;
			}
		}

		private static byte[] WrapHeader(byte[] body)
		{
			byte[] bytes = BitConverter.GetBytes(body.Length);
			Array.Reverse(bytes);
			using (MemoryStream memoryStream = new MemoryStream(4 + body.Length))
			{
				memoryStream.Write(bytes, 0, bytes.Length);
				memoryStream.Write(body, 0, body.Length);
				return memoryStream.ToArray();
			}
		}

		private void Receive()
		{
			Logger.Debug("ReceiveThread started");
			while (true)
			{
				try
				{
					if (ConnectionState != 0)
					{
						break;
					}
					int num = ReceiveResponseSize();
					using (MemoryStream memoryStream = new MemoryStream(num))
					{
						for (int i = 0; i < num; i++)
						{
							memoryStream.WriteByte(ReceiveByte());
						}
						byte[] data = memoryStream.ToArray();
						RpcMessage.ResponseMessage responseMessage = ProtoReflectionUtils.FromBytes<RpcMessage.ResponseMessage>(data);
						if (responseMessage.RpcResponse != null)
						{
							_responses[responseMessage.RpcResponse.Id] = responseMessage.RpcResponse;
							WarnResponseSize();
						}
						else if (responseMessage.EventResponse != null)
						{
							OnEventReceived(responseMessage.EventResponse);
						}
						else
						{
							Logger.Error("Invalid response message, response and command is null");
						}
					}
					continue;
				}
				catch (SocketException msg)
				{
					Logger.Debug(msg);
				}
				catch (IOException ex)
				{
					if (ex.InnerException is ThreadAbortException || ex.InnerException is SocketException)
					{
						Logger.Debug("SendThread aborted " + ex);
						break;
					}
					Logger.Error("ReceiveThread fatal exception: " + ex);
					continue;
				}
				catch (Exception ex2)
				{
					Logger.Error("ReceiveThread fatal exception: " + ex2);
				}
				break;
			}
			Logger.Debug("ReceiveThread stopped");
		}

		private void WarnResponseSize()
		{
			if (_responses.Count > 50)
			{
				Logger.Error(string.Format("Response count too long({0}), is it ok?", _responses.Count));
			}
		}

		private int ReceiveResponseSize()
		{
			byte[] array = new byte[4];
			for (int i = 0; i < array.Length; i++)
			{
				array[array.Length - 1 - i] = ReceiveByte();
			}
			return BitConverter.ToInt32(array, 0);
		}

		private byte ReceiveByte()
		{
			byte[] array = new byte[1];
			if (_tcpClient.GetStream().Read(array, 0, 1) > 0)
			{
				return array[0];
			}
			throw new ReceiveException();
		}

		public RpcMessage.RpcResponse SendRequest(RpcMessage.RpcRequest request)
		{
			_requests.Enqueue(request);
			while (ConnectionState == ConnectionState.Connected)
			{
				RpcMessage.RpcResponse value;
				if (_responses.TryRemove(request.Id, out value))
				{
					if (value == null)
					{
						throw new ConnectionFailedException();
					}
					return value;
				}
				Thread.Sleep(50);
			}
			throw new ConnectionFailedException();
		}

		protected virtual void OnConnectionFailed()
		{
			if (this.ConnectionFailed != null)
			{
				this.ConnectionFailed();
			}
		}

		protected virtual void OnDisconnected()
		{
			if (this.Disconnected != null)
			{
				this.Disconnected();
			}
		}
	}
}
