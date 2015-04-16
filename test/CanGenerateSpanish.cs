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
		[TestCase("Yo voy.")]
		[TestCase("Tú vas.")]		
		[TestCase("Usted va.")]
		[TestCase("Él va.")]
		[TestCase("Ella va.")]

		[TestCase("Nosotros vamos.")]
		[TestCase("Vosotros vais.")]
		[TestCase("Ustedes van.")]
		[TestCase("Ellos van.")]
		[TestCase("Ellas van.")]

		[TestCase("Yo grito.")]
		public void PresentTenseIntransitive (string expected)
		{
			Assert.Contains (expected, allPhrases);
		}

		[TestCase("Yo preparo la cena.")]
		[TestCase("Ellos suben la escalera.")]
		[TestCase("Ellas beben leche.")]
		[TestCase("Él suma la cuenta.")]
		[TestCase("Ella habla a la reportera.")]
		[TestCase("Él teme a los críticos.")]
		[TestCase("Ellos comen fajitas.")]
		public void PresentTenseTransitive (string expected)
		{
			Assert.Contains (expected, allPhrases);
		}

		[TestCase("Yo he hablado con él.")]
		public void PresentPerfectTense (string expected) {

			var filtered = allPhrases.Where (p => p.Contains ("he hablado")).ToArray();
			Console.WriteLine (String.Join (", ", filtered));

			Assert.Contains (expected, allPhrases);
		}

		[TestCase("Ahora yo preparo la cena.")]
		[TestCase("Normalmente yo preparo la cena.")]
		[TestCase("Esta mes yo preparo la cena.")]
		[TestCase("Anoche yo preparé la cena.")]
		[TestCase("Casi nunca yo preparaba la cena.")]
		// other conjugations?
		public void TimeframesAreUsedWithAppropriateTense(string expected) {
			Assert.Contains (expected, allPhrases);
		}
	}
}

