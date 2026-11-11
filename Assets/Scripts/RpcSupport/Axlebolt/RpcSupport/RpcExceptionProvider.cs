using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Axlebolt.RpcSupport
{
	public class RpcExceptionProvider
	{
		private readonly Dictionary<int, Type> exceptions = new Dictionary<int, Type>();

		public RpcExceptionProvider(Assembly assembly)
		{
			IEnumerable<Type> rpcExceptionTypes = GetRpcExceptionTypes(Assembly.GetExecutingAssembly());
			IEnumerable<Type> rpcExceptionTypes2 = GetRpcExceptionTypes(assembly);
			List<Type> list = new List<Type>();
			list.AddRange(rpcExceptionTypes);
			list.AddRange(rpcExceptionTypes2);
			foreach (Type item in list)
			{
				RpcException ex = (RpcException)Activator.CreateInstance(item);
				exceptions[ex.Code] =  item;
			}
		}

		public RpcException Instantiate(long id, int code)
		{
			RpcException ex = CreateException(code);
			ex.Id = id;
			return ex;
		}

		private RpcException CreateException(int code)
		{
			Type value;
			if (!exceptions.TryGetValue(code, out value))
			{
				return new RpcException(code);
			}
			return (RpcException)Activator.CreateInstance(value);
		}

		private IEnumerable<Type> GetRpcExceptionTypes(Assembly assembly)
		{
			Type exceptionType = typeof(RpcException);
			return (from t in assembly.GetTypes()
				where exceptionType.IsAssignableFrom(t) && t != exceptionType
				select t).ToArray();
		}
	}
}
