using System;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal.Filters;

namespace NUnit.Framework.Internal
{
	[Serializable]
	public abstract class TestFilter : ITestFilter, IXmlNodeBuilder
	{
		[Serializable]
		private class EmptyFilter : TestFilter
		{
			public override bool Match(ITest test)
			{
				return true;
			}

			public override bool Pass(ITest test)
			{
				return true;
			}

			public override bool IsExplicitMatch(ITest test)
			{
				return false;
			}

			public override TNode AddToXml(TNode parentNode, bool recursive)
			{
				return parentNode.AddElement("filter");
			}
		}

		public static readonly TestFilter Empty = new EmptyFilter();

		public bool IsEmpty
		{
			get
			{
				return this is EmptyFilter;
			}
		}

		public bool TopLevel { get; set; }

		public virtual bool Pass(ITest test)
		{
			if (!Match(test) && !MatchParent(test))
			{
				return MatchDescendant(test);
			}
			return true;
		}

		public virtual bool IsExplicitMatch(ITest test)
		{
			if (!Match(test))
			{
				return MatchDescendant(test);
			}
			return true;
		}

		public abstract bool Match(ITest test);

		public bool MatchParent(ITest test)
		{
			if (test.Parent != null)
			{
				if (!Match(test.Parent))
				{
					return MatchParent(test.Parent);
				}
				return true;
			}
			return false;
		}

		protected virtual bool MatchDescendant(ITest test)
		{
			if (test.Tests == null)
			{
				return false;
			}
			foreach (ITest test2 in test.Tests)
			{
				if (Match(test2) || MatchDescendant(test2))
				{
					return true;
				}
			}
			return false;
		}

		public static TestFilter FromXml(string xmlText)
		{
			TNode tNode = TNode.FromXml(xmlText);
			if (tNode.Name != "filter")
			{
				throw new Exception("Expected filter element at top level");
			}
			TestFilter testFilter;
			switch (tNode.ChildNodes.Count)
			{
			default:
				testFilter = FromXml(tNode);
				break;
			case 1:
				testFilter = FromXml(tNode.FirstChild);
				break;
			case 0:
				testFilter = Empty;
				break;
			}
			testFilter.TopLevel = true;
			return testFilter;
		}

		public static TestFilter FromXml(TNode node)
		{
			bool isRegex = node.Attributes["re"] == "1";
			switch (node.Name)
			{
			case "filter":
			case "and":
			{
				AndFilter andFilter = new AndFilter();
				{
					foreach (TNode childNode in node.ChildNodes)
					{
						andFilter.Add(FromXml(childNode));
					}
					return andFilter;
				}
			}
			case "or":
			{
				OrFilter orFilter = new OrFilter();
				{
					foreach (TNode childNode2 in node.ChildNodes)
					{
						orFilter.Add(FromXml(childNode2));
					}
					return orFilter;
				}
			}
			case "not":
				return new NotFilter(FromXml(node.FirstChild));
			case "id":
				return new IdFilter(node.Value);
			case "test":
				return new FullNameFilter(node.Value)
				{
					IsRegex = isRegex
				};
			case "name":
				return new TestNameFilter(node.Value)
				{
					IsRegex = isRegex
				};
			case "method":
				return new MethodNameFilter(node.Value)
				{
					IsRegex = isRegex
				};
			case "class":
				return new ClassNameFilter(node.Value)
				{
					IsRegex = isRegex
				};
			case "cat":
				return new CategoryFilter(node.Value)
				{
					IsRegex = isRegex
				};
			case "prop":
			{
				string text = node.Attributes["name"];
				if (text != null)
				{
					return new PropertyFilter(text, node.Value)
					{
						IsRegex = isRegex
					};
				}
				break;
			}
			}
			throw new ArgumentException("Invalid filter element: " + node.Name, "xmlNode");
		}

		public TNode ToXml(bool recursive)
		{
			return AddToXml(new TNode("dummy"), recursive);
		}

		public abstract TNode AddToXml(TNode parentNode, bool recursive);
	}
}
