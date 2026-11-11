using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using Axlebolt.RpcSupport.Protobuf;

namespace Axlebolt.RpcSupport
{
	public class RpcServiceProxy : RealProxy
	{
		private class RpcParameters
		{
			public readonly string ServiceName;

			public readonly string MethodName;

			public RpcParameters(string serviceName, string methodName)
			{
				ServiceName = serviceName;
				MethodName = methodName;
			}
		}

		private readonly Type _serviceType;

		private readonly Dictionary<MethodBase, RpcParameters> _rpcMethods = new Dictionary<MethodBase, RpcParameters>();

		private readonly ClientService _client;

		private readonly RpcExceptionProvider _exceptionProvider;

		public RpcServiceProxy(Type serviceType, ClientService client, RpcExceptionProvider exceptionProvider)
			: base(serviceType)
		{
			_serviceType = serviceType;
			_client = client;
			_exceptionProvider = exceptionProvider;
			ScanRpcMethods();
		}

		private void ScanRpcMethods()
		{
			RpcService attribute = AttributeUtils.GetAttribute<RpcService>(_serviceType);
			if (attribute == null)
			{
				Logger.Error(string.Concat(_serviceType, " is not rpc service"));
				throw new Exception("Incorrect rpc service type");
			}
			MethodInfo[] methods = _serviceType.GetMethods();
			foreach (MethodInfo methodInfo in methods)
			{
				Rpc attribute2 = AttributeUtils.GetAttribute<Rpc>(methodInfo);
				if (attribute2 != null)
				{
					Validate(methodInfo);
					Console.WriteLine(methodInfo.Name + " registred");
					_rpcMethods[methodInfo] = new RpcParameters(attribute.name, attribute2.Name);
				}
				else
				{
					Console.WriteLine(methodInfo.Name + " skipped");
				}
			}
		}

		private void Validate(MethodInfo method)
		{
			ParameterInfo[] parameters = method.GetParameters();
			foreach (ParameterInfo parameterInfo in parameters)
			{
				if (!ProtoReflectionUtils.IsSupportedType(parameterInfo.ParameterType))
				{
					throw new Exception("Invalid Rpc method argument type, " + method.Name + ". Unsupported type " + parameterInfo.ParameterType);
				}
			}
			if (!ProtoReflectionUtils.IsSupportedType(method.ReturnType))
			{
				throw new Exception("Invalid Rpc method return type, " + method.Name + ". Unsupported type " + method.ReturnType);
			}
		}

		public override IMessage Invoke(IMessage msg)
		{
			IMethodCallMessage methodCallMessage = msg as IMethodCallMessage;
			RpcParameters value;
			if (methodCallMessage != null && _rpcMethods.TryGetValue(methodCallMessage.MethodBase, out value))
			{
				object ret = RpcCall(methodCallMessage.MethodBase, value, methodCallMessage.Args);
				return new ReturnMessage(ret, null, 0, methodCallMessage.LogicalCallContext, methodCallMessage);
			}
			throw new Exception(methodCallMessage.MethodName + " is not Rpc");
		}

		private object RpcCall(MethodBase methodBase, RpcParameters endPoint, object[] args)
		{
			if (methodBase is MethodInfo)
			{
				MethodInfo methodInfo = methodBase as MethodInfo;
				RpcMessage.RpcRequest rpcRequest = new RpcMessage.RpcRequest();
				rpcRequest.Id = Guid.NewGuid().ToString();
				rpcRequest.ServiceName = endPoint.ServiceName;
				rpcRequest.MethodName = endPoint.MethodName;
				rpcRequest.Params = new List<RpcMessage.BinaryValue>();
				foreach (object o in args)
				{
					rpcRequest.Params.Add(ProtoReflectionUtils.ToBinaryValue(o));
				}
				RpcMessage.RpcResponse rpcResponse = _client.SendRequest(rpcRequest);
				if (rpcResponse.Exception == null)
				{
					return ProtoReflectionUtils.FromBinaryValue(rpcResponse.Return, methodInfo.ReturnType);
				}
				RpcMessage.Exception exception = rpcResponse.Exception;
				throw _exceptionProvider.Instantiate(exception.Id, exception.Code);
			}
			throw new Exception("Incorrect method type");
		}
	}
}
