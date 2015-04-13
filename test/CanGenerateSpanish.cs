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
		[TestCase("yo voy")]
		[TestCase("tú vas")]		
		[TestCase("usted va")]
		[TestCase("él va")]
		[TestCase("ella va")]

		[TestCase("nosotros vamos")]
		[TestCase("vosotros vais")]
		[TestCase("ustedes van")]
		[TestCase("ellos van")]
		[TestCase("ellas van")]

		[TestCase("yo grito")]
		public void PresentTenseIr (string expected)
		{
			Assert.Contains (expected, allPhrases);
		}
	}
}

