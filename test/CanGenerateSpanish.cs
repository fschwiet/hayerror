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
		[NUnit.Framework.TestCase("usted va")]
		[NUnit.Framework.TestCase("él va")]
		[NUnit.Framework.TestCase("ella va")]

		[NUnit.Framework.TestCase("nosotros vamos")]
		[NUnit.Framework.TestCase("vosotros vais")]
		[NUnit.Framework.TestCase("ustedes van")]
		[NUnit.Framework.TestCase("ellos van")]
		[NUnit.Framework.TestCase("ellas van")]

		[NUnit.Framework.TestCase("yo grito")]
		public void PresentTenseIr (string expected)
		{
			var results = new monarquia.EspanolGenerator().GetAll();

			Assert.Contains (expected, results.ToArray());
		}
	}
}

