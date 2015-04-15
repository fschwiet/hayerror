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
		public void PresentTenseIntransitive (string expected)
		{
			Assert.Contains (expected, allPhrases);
		}

		[TestCase("yo preparo la cena")]
		[TestCase("ellos suben la escalera")]
		[TestCase("ellas beben leche")]
		[TestCase("él suma la cuenta")]
		[TestCase("ella habla a la reportera")]
		[TestCase("él teme a los críticos")]
		[TestCase("ellos comen fajitas")]
		public void PresentTenseTransitive (string expected)
		{
			Assert.Contains (expected, allPhrases);
		}

		[TestCase("yo he hablado con él")]
		public void PresentPerfectTense (string expected) {

			var filtered = allPhrases.Where (p => p.Contains ("he hablado")).ToArray();
			Console.WriteLine (String.Join (", ", filtered));

			Assert.Contains (expected, allPhrases);
		}
	}
}

