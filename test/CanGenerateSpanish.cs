using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using monarquia;

namespace test
{
	[TestFixture ()]
	public class CanGenerateSpanish
	{
		string[] allPhrases;

		[TestFixtureSetUp]
		public void LoadVerbs() {
			allPhrases = new monarquia.EspanolGenerator("../../../data").GetAll().ToArray();
		}

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
			Assert.Contains (expected, allPhrases);
		}
	}
}

