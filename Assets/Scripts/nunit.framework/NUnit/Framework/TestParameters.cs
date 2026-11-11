using System;
using System.Collections.Generic;
using System.Globalization;

namespace NUnit.Framework
{
	public class TestParameters
	{
		private static readonly IFormatProvider MODIFIED_INVARIANT_CULTURE = CreateModifiedInvariantCulture();

		private readonly Dictionary<string, string> _parameters = new Dictionary<string, string>();

		public int Count
		{
			get
			{
				return _parameters.Count;
			}
		}

		public ICollection<string> Names
		{
			get
			{
				return _parameters.Keys;
			}
		}

		public string this[string name]
		{
			get
			{
				return Get(name);
			}
		}

		public bool Exists(string name)
		{
			return _parameters.ContainsKey(name);
		}

		public string Get(string name)
		{
			if (!Exists(name))
			{
				return null;
			}
			return _parameters[name];
		}

		public string Get(string name, string defaultValue)
		{
			return Get(name) ?? defaultValue;
		}

		public T Get<T>(string name, T defaultValue)
		{
			string text = Get(name);
			if (text == null)
			{
				return defaultValue;
			}
			return (T)Convert.ChangeType(text, typeof(T), MODIFIED_INVARIANT_CULTURE);
		}

		internal void Add(string name, string value)
		{
			_parameters[name] = value;
		}

		private static IFormatProvider CreateModifiedInvariantCulture()
		{
			CultureInfo obj = (CultureInfo)CultureInfo.InvariantCulture.Clone();
			obj.NumberFormat.CurrencyGroupSeparator = string.Empty;
			obj.NumberFormat.NumberGroupSeparator = string.Empty;
			obj.NumberFormat.PercentGroupSeparator = string.Empty;
			return obj;
		}
	}
}
