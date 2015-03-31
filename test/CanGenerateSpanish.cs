using NUnit.Framework;
using System;
using System.Linq;

namespace test
{
	[TestFixture ()]
	public class CanGenerateSpanish
	{
		[Test]
		[NUnit.Framework.TestCase("yo voy")]
		[NUnit.Framework.TestCase("tú vas")]
		[NUnit.Framework.TestCase("él va")]
		[NUnit.Framework.TestCase("ella va")]
		[NUnit.Framework.TestCase("usted va")]
		[NUnit.Framework.TestCase("vosotros vais")]
		[NUnit.Framework.TestCase("ellos van")]
		[NUnit.Framework.TestCase("ellas van")]
		[NUnit.Framework.TestCase("ustedes van")]
		public void PresentTenseIr (string expected)
		{
			var results = new monarquia.EspanolGenerator().GetAll();

			Assert.Contains (expected, results.ToArray());
		}
	}
}

