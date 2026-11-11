using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Compatibility;

namespace NUnit.Framework.Constraints
{
	public class NUnitEqualityComparer
	{
		public class FailurePoint
		{
			public long Position;

			public object ExpectedValue;

			public object ActualValue;

			public bool ExpectedHasData;

			public bool ActualHasData;
		}

		private bool caseInsensitive;

		private bool compareAsCollection;

		private List<EqualityAdapter> externalComparers = new List<EqualityAdapter>();

		private List<FailurePoint> failurePoints;

		private static readonly int BUFFER_SIZE = 4096;

		public static NUnitEqualityComparer Default
		{
			get
			{
				return new NUnitEqualityComparer();
			}
		}

		public bool IgnoreCase
		{
			get
			{
				return caseInsensitive;
			}
			set
			{
				caseInsensitive = value;
			}
		}

		public bool CompareAsCollection
		{
			get
			{
				return compareAsCollection;
			}
			set
			{
				compareAsCollection = value;
			}
		}

		public IList<EqualityAdapter> ExternalComparers
		{
			get
			{
				return externalComparers;
			}
		}

		public IList<FailurePoint> FailurePoints
		{
			get
			{
				return failurePoints;
			}
		}

		public bool WithSameOffset { get; set; }

		public bool AreEqual(object x, object y, ref Tolerance tolerance)
		{
			failurePoints = new List<FailurePoint>();
			if (x == null && y == null)
			{
				return true;
			}
			if (x == null || y == null)
			{
				return false;
			}
			if (x == y)
			{
				return true;
			}
			Type type = x.GetType();
			Type type2 = y.GetType();
			Type type3 = (type.GetTypeInfo().IsGenericType ? type.GetGenericTypeDefinition() : null);
			Type type4 = (type2.GetTypeInfo().IsGenericType ? type2.GetGenericTypeDefinition() : null);
			EqualityAdapter externalComparer = GetExternalComparer(x, y);
			if (externalComparer != null)
			{
				return externalComparer.AreEqual(x, y);
			}
			if (type.IsArray && type2.IsArray && !compareAsCollection)
			{
				return ArraysEqual((Array)x, (Array)y, ref tolerance);
			}
			if (x is IDictionary && y is IDictionary)
			{
				return DictionariesEqual((IDictionary)x, (IDictionary)y, ref tolerance);
			}
			if (x is DictionaryEntry && y is DictionaryEntry)
			{
				return DictionaryEntriesEqual((DictionaryEntry)x, (DictionaryEntry)y, ref tolerance);
			}
			if (type3 == typeof(KeyValuePair<, >) && type4 == typeof(KeyValuePair<, >))
			{
				Tolerance tolerance2 = Tolerance.Exact;
				object value = type.GetProperty("Key").GetValue(x, null);
				object value2 = type2.GetProperty("Key").GetValue(y, null);
				object value3 = type.GetProperty("Value").GetValue(x, null);
				object value4 = type2.GetProperty("Value").GetValue(y, null);
				if (AreEqual(value, value2, ref tolerance2))
				{
					return AreEqual(value3, value4, ref tolerance);
				}
				return false;
			}
			if (x is string && y is string)
			{
				return StringsEqual((string)x, (string)y);
			}
			if (x is Stream && y is Stream)
			{
				return StreamsEqual((Stream)x, (Stream)y);
			}
			if (x is char && y is char)
			{
				return CharsEqual((char)x, (char)y);
			}
			if (x is DirectoryInfo && y is DirectoryInfo)
			{
				return DirectoriesEqual((DirectoryInfo)x, (DirectoryInfo)y);
			}
			if (Numerics.IsNumericType(x) && Numerics.IsNumericType(y))
			{
				return Numerics.AreEqual(x, y, ref tolerance);
			}
			if (x is DateTimeOffset && y is DateTimeOffset)
			{
				DateTimeOffset dateTimeOffset = (DateTimeOffset)x;
				DateTimeOffset dateTimeOffset2 = (DateTimeOffset)y;
				bool flag;
				if (tolerance != null && tolerance.Value is TimeSpan)
				{
					TimeSpan timeSpan = (TimeSpan)tolerance.Value;
					flag = (dateTimeOffset - dateTimeOffset2).Duration() <= timeSpan;
				}
				else
				{
					flag = dateTimeOffset == dateTimeOffset2;
				}
				if (flag && WithSameOffset)
				{
					flag = dateTimeOffset.Offset == dateTimeOffset2.Offset;
				}
				return flag;
			}
			if (tolerance != null && tolerance.Value is TimeSpan)
			{
				TimeSpan timeSpan2 = (TimeSpan)tolerance.Value;
				if (x is DateTime && y is DateTime)
				{
					return ((DateTime)x - (DateTime)y).Duration() <= timeSpan2;
				}
				if (x is TimeSpan && y is TimeSpan)
				{
					return ((TimeSpan)x - (TimeSpan)y).Duration() <= timeSpan2;
				}
			}
			MethodInfo methodInfo = FirstImplementsIEquatableOfSecond(type, type2);
			if (methodInfo != null)
			{
				return InvokeFirstIEquatableEqualsSecond(x, y, methodInfo);
			}
			if (type != type2 && (methodInfo = FirstImplementsIEquatableOfSecond(type2, type)) != null)
			{
				return InvokeFirstIEquatableEqualsSecond(y, x, methodInfo);
			}
			if (x is IEnumerable && y is IEnumerable)
			{
				return EnumerablesEqual((IEnumerable)x, (IEnumerable)y, ref tolerance);
			}
			return x.Equals(y);
		}

		private static MethodInfo FirstImplementsIEquatableOfSecond(Type first, Type second)
		{
			KeyValuePair<Type, MethodInfo> keyValuePair = default(KeyValuePair<Type, MethodInfo>);
			foreach (KeyValuePair<Type, MethodInfo> equatableGenericArgument in GetEquatableGenericArguments(first))
			{
				if (equatableGenericArgument.Key.IsAssignableFrom(second) && (keyValuePair.Key == null || keyValuePair.Key.IsAssignableFrom(equatableGenericArgument.Key)))
				{
					keyValuePair = equatableGenericArgument;
				}
			}
			return keyValuePair.Value;
		}

		private static IList<KeyValuePair<Type, MethodInfo>> GetEquatableGenericArguments(Type type)
		{
			List<KeyValuePair<Type, MethodInfo>> list = new List<KeyValuePair<Type, MethodInfo>>();
			Type[] interfaces = type.GetInterfaces();
			foreach (Type type2 in interfaces)
			{
				if (type2.GetTypeInfo().IsGenericType && type2.GetGenericTypeDefinition().Equals(typeof(IEquatable<>)))
				{
					list.Add(new KeyValuePair<Type, MethodInfo>(type2.GetGenericArguments()[0], type2.GetMethod("Equals")));
				}
			}
			return list;
		}

		private static bool InvokeFirstIEquatableEqualsSecond(object first, object second, MethodInfo equals)
		{
			if (equals == null)
			{
				return false;
			}
			return (bool)equals.Invoke(first, new object[1] { second });
		}

		private EqualityAdapter GetExternalComparer(object x, object y)
		{
			foreach (EqualityAdapter externalComparer in externalComparers)
			{
				if (externalComparer.CanCompare(x, y))
				{
					return externalComparer;
				}
			}
			return null;
		}

		private bool ArraysEqual(Array x, Array y, ref Tolerance tolerance)
		{
			int rank = x.Rank;
			if (rank != y.Rank)
			{
				return false;
			}
			for (int i = 1; i < rank; i++)
			{
				if (x.GetLength(i) != y.GetLength(i))
				{
					return false;
				}
			}
			return EnumerablesEqual(x, y, ref tolerance);
		}

		private bool DictionariesEqual(IDictionary x, IDictionary y, ref Tolerance tolerance)
		{
			if (x.Count != y.Count)
			{
				return false;
			}
			CollectionTally collectionTally = new CollectionTally(this, x.Keys);
			if (!collectionTally.TryRemove(y.Keys) || collectionTally.Count > 0)
			{
				return false;
			}
			foreach (object key in x.Keys)
			{
				if (!AreEqual(x[key], y[key], ref tolerance))
				{
					return false;
				}
			}
			return true;
		}

		private bool DictionaryEntriesEqual(DictionaryEntry x, DictionaryEntry y, ref Tolerance tolerance)
		{
			Tolerance tolerance2 = Tolerance.Exact;
			if (AreEqual(x.Key, y.Key, ref tolerance2))
			{
				return AreEqual(x.Value, y.Value, ref tolerance);
			}
			return false;
		}

		private bool CollectionsEqual(ICollection x, ICollection y, ref Tolerance tolerance)
		{
			IEnumerator enumerator = null;
			IEnumerator enumerator2 = null;
			try
			{
				enumerator = x.GetEnumerator();
				enumerator2 = y.GetEnumerator();
				int num = 0;
				bool flag;
				bool flag2;
				while (true)
				{
					flag = enumerator.MoveNext();
					flag2 = enumerator2.MoveNext();
					if (!flag && !flag2)
					{
						return true;
					}
					if (flag != flag2 || !AreEqual(enumerator.Current, enumerator2.Current, ref tolerance))
					{
						break;
					}
					num++;
				}
				FailurePoint failurePoint = new FailurePoint();
				failurePoint.Position = num;
				failurePoint.ExpectedHasData = flag;
				if (flag)
				{
					failurePoint.ExpectedValue = enumerator.Current;
				}
				failurePoint.ActualHasData = flag2;
				if (flag2)
				{
					failurePoint.ActualValue = enumerator2.Current;
				}
				failurePoints.Insert(0, failurePoint);
				return false;
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
				IDisposable disposable2 = enumerator2 as IDisposable;
				if (disposable2 != null)
				{
					disposable2.Dispose();
				}
			}
		}

		private bool StringsEqual(string x, string y)
		{
			string obj = (caseInsensitive ? x.ToLower() : x);
			string value = (caseInsensitive ? y.ToLower() : y);
			return obj.Equals(value);
		}

		private bool CharsEqual(char x, char y)
		{
			char num = (caseInsensitive ? char.ToLower(x) : x);
			char c = (caseInsensitive ? char.ToLower(y) : y);
			return num == c;
		}

		private bool EnumerablesEqual(IEnumerable x, IEnumerable y, ref Tolerance tolerance)
		{
			IEnumerator enumerator = null;
			IEnumerator enumerator2 = null;
			try
			{
				enumerator = x.GetEnumerator();
				enumerator2 = y.GetEnumerator();
				int num = 0;
				bool flag;
				bool flag2;
				while (true)
				{
					flag = enumerator.MoveNext();
					flag2 = enumerator2.MoveNext();
					if (!flag && !flag2)
					{
						return true;
					}
					if (flag != flag2 || !AreEqual(enumerator.Current, enumerator2.Current, ref tolerance))
					{
						break;
					}
					num++;
				}
				FailurePoint failurePoint = new FailurePoint();
				failurePoint.Position = num;
				failurePoint.ExpectedHasData = flag;
				if (flag)
				{
					failurePoint.ExpectedValue = enumerator.Current;
				}
				failurePoint.ActualHasData = flag2;
				if (flag2)
				{
					failurePoint.ActualValue = enumerator2.Current;
				}
				failurePoints.Insert(0, failurePoint);
				return false;
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
				IDisposable disposable2 = enumerator2 as IDisposable;
				if (disposable2 != null)
				{
					disposable2.Dispose();
				}
			}
		}

		private static bool DirectoriesEqual(DirectoryInfo x, DirectoryInfo y)
		{
			if (x.Attributes != y.Attributes || x.CreationTime != y.CreationTime || x.LastAccessTime != y.LastAccessTime)
			{
				return false;
			}
			return new SamePathConstraint(x.FullName).ApplyTo(y.FullName).IsSuccess;
		}

		private bool StreamsEqual(Stream x, Stream y)
		{
			if (x == y)
			{
				return true;
			}
			if (!x.CanRead)
			{
				throw new ArgumentException("Stream is not readable", "expected");
			}
			if (!y.CanRead)
			{
				throw new ArgumentException("Stream is not readable", "actual");
			}
			if (!x.CanSeek)
			{
				throw new ArgumentException("Stream is not seekable", "expected");
			}
			if (!y.CanSeek)
			{
				throw new ArgumentException("Stream is not seekable", "actual");
			}
			if (x.Length != y.Length)
			{
				return false;
			}
			byte[] array = new byte[BUFFER_SIZE];
			byte[] array2 = new byte[BUFFER_SIZE];
			BinaryReader binaryReader = new BinaryReader(x);
			BinaryReader binaryReader2 = new BinaryReader(y);
			long position = x.Position;
			long position2 = y.Position;
			try
			{
				binaryReader.BaseStream.Seek(0L, SeekOrigin.Begin);
				binaryReader2.BaseStream.Seek(0L, SeekOrigin.Begin);
				for (long num = 0L; num < x.Length; num += BUFFER_SIZE)
				{
					binaryReader.Read(array, 0, BUFFER_SIZE);
					binaryReader2.Read(array2, 0, BUFFER_SIZE);
					for (int i = 0; i < BUFFER_SIZE; i++)
					{
						if (array[i] != array2[i])
						{
							FailurePoint failurePoint = new FailurePoint();
							failurePoint.Position = num + i;
							failurePoint.ExpectedHasData = true;
							failurePoint.ExpectedValue = array[i];
							failurePoint.ActualHasData = true;
							failurePoint.ActualValue = array2[i];
							failurePoints.Insert(0, failurePoint);
							return false;
						}
					}
				}
			}
			finally
			{
				x.Position = position;
				y.Position = position2;
			}
			return true;
		}
	}
}
