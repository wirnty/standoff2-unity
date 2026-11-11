using System;

namespace NUnit.Framework
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	[Obsolete("Use OneTimeSetUpAttribute")]
	public class TestFixtureSetUpAttribute : OneTimeSetUpAttribute
	{
	}
}
