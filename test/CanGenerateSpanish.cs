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
		[TestCase("Yo voy al cine.")]
		[TestCase("Tú vas al cine.")]		
		[TestCase("Usted va al cine.")]
		[TestCase("Él va al cine.")]
		[TestCase("Ella va al cine.")]

		[TestCase("Nosotros vamos al cine.")]
		[TestCase("Vosotros vais al cine.")]
		[TestCase("Ustedes van al cine.")]
		[TestCase("Ellos van al cine.")]
		[TestCase("Ellas van al cine.")]
		public void PresentTenseIntransitive (string expected)
		{
			Assert.Contains (expected, allPhrases);
		}

		[TestCase("Yo grito.")]
		[TestCase("Ella está en frente.")]
		[TestCase("Yo estoy en frente.")]
		public void MiscBugs (string expected)
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
		[TestCase("Yo prepararé la cena.")]
		[TestCase("Mañana yo prepararé la cena.")]
		[TestCase("Por supuesto yo prepararía la cena.")]
		// other conjugations?
		public void TimeframesAreUsedWithAppropriateTense(string expected) {
			Assert.Contains (expected, allPhrases);
		}
	}
}

