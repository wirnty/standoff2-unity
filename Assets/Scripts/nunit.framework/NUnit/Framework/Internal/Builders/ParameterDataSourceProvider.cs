using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;

namespace NUnit.Framework.Internal.Builders
{
	public class ParameterDataSourceProvider : IParameterDataProvider
	{
		public bool HasDataFor(IParameterInfo parameter)
		{
			return parameter.IsDefined<IParameterDataSource>(false);
		}

		public IEnumerable GetDataFor(IParameterInfo parameter)
		{
			List<object> list = new List<object>();
			IParameterDataSource[] customAttributes = parameter.GetCustomAttributes<IParameterDataSource>(false);
			for (int i = 0; i < customAttributes.Length; i++)
			{
				foreach (object datum in customAttributes[i].GetData(parameter))
				{
					list.Add(datum);
				}
			}
			return list;
		}
	}
}
