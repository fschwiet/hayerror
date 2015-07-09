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
		Exercise[] allExercises;
		string[] allPhrases;

		[TestFixtureSetUp]
		public void LoadVerbs() {

			var dataDirectory = "../../../data";
			allExercises = new monarquia.EspanolGenerator(new BigCannedData( new DataLoader(dataDirectory)), dataDirectory).GetExercises().ToArray();
			allPhrases = allExercises.Select (e => e.Original).ToArray ();
		}

		[Test]
		[TestCase("Yo voy al cine.", "I go to the movies.")]
		[TestCase("Tú vas al cine.", "You go to the movies.")]		
		[TestCase("Usted va al cine.", "(usted) You go to the movies.")]
		[TestCase("Él va al cine.", "He goes to the movies.")]
		[TestCase("Ella va al cine.", "She goes to the movies.")]

		[TestCase("Nosotros vamos al cine.", "We go to the movies.")]
		[TestCase("Vosotros vais al cine.", "(vosotros) All of you go to the movies.")]
		[TestCase("Ustedes van al cine.", "All of you go to the movies.")]
		[TestCase("Ellos van al cine.", "(ellos) They go to the movies.")]
		[TestCase("Ellas van al cine.", "(ellas) They go to the movies.")]
		public void PresentTenseIntransitive (string expected, string translation)
		{
			AssertHasTranslation (expected, translation);
		}

		[TestCase("Yo grito.", "I shout.")]
		[TestCase("Ella está en frente.", "She is in front.")]
		[TestCase("Yo estoy en frente.", "I am in front.")]
        [TestCase("A esa hora nosotros no estuvimos en frente.", "At that hour we were not in front.")]
        [TestCase("Ella no hace un viaje.", "She does not take a trip.")]
        [TestCase("Ayer ella no hizo un viaje.", "Yesterday she did not take a trip.")]
        [TestCase("Ellos no conocen a mi madre.", "(ellos) They do not know my mother.")]
        public void MiscBugs (string expected, string translation)
		{
			AssertHasTranslation (expected, translation);
		}

		[TestCase("Yo soy cocinero.", "I am a cook.")]
		[TestCase("Ella es actriz.", "She is an actor.")]
		[TestCase("Tú eres cocinero.", "You are a cook.")]
		[TestCase("Nosotros somos cocineros.", "We are cooks.")]
		[TestCase("Ellos son cocineros.", "(ellos) They are cooks.")]
		public void IsProfessionExpressionsOftenLackAnArticle (string expected, string translation)
		{
			AssertHasTranslation (expected, translation);
		}

		[TestCase("Yo preparo la cena.", null /*"I prepare the dinner."  prepare doesn't have conjugation downloadable on SD */)]
		[TestCase("Ellos suben la escalera.", "(ellos) They climb the stairs.")]
		[TestCase("Ellas beben leche.", "(ellas) They drink milk.")]
        [TestCase("Él suma su voz a los otros.", "He adds his voice to the others.")]
		[TestCase("Él suma la comida a la cuenta.", "He adds the food to the bill.")]
		[TestCase("Ella habla a la reportera.", "She talks to the reporter.")]
		[TestCase("Él teme a los críticos.", "He fears the critics.")]
		[TestCase("Ellos comen fajitas.", "(ellos) They eat fajitas.")]
		public void PresentTenseTransitive (string expected, string translation)
		{
			AssertHasTranslation (expected, translation);
		}

		[TestCase("Yo he hablado con mi amigo.", "I have talked with my friend.")]
		public void PresentPerfectTense (string expected, string translation)
		{
			AssertHasTranslation (expected, translation);
		}

		[TestCase("Ayer yo grité.")]
        // other conjugations?
		public void TimeframesAreUsedWithAppropriateTense(string expected) {
			Assert.Contains (expected, allPhrases);
		}

		[TestCase("Él es actor.")]
		[TestCase("Ella es actriz.")]
		[TestCase("Ellos son actores.")]
		[TestCase("Ellas son actrizes.")]
		public void TransferGenderAndNumberForSer(string expected){
			Assert.Contains (expected, allPhrases);
		}


		[TestCase("Yo conozco a mi madre.", "I know my mother.")]
		[TestCase("Ayer yo conocí a mi madre.", "Yesterday I met my mother.")]
        [TestCase("Él me dijo que yo conocía a mi madre.", "He told me that I knew my mother.")]
		[TestCase("Yo conocería a mi madre.", "I would know my mother.")]
		[TestCase("Yo conoceré a mi madre.", "I will meet my mother.")]
		[TestCase("Ellas han conocido a mi madre.", "(ellas) They have known my mother.")]

		// nunca ha conocido -> has never met
		public void HandlesVerbsThatChangeInterpretationWithTense(string expected, string translation)
		{
			AssertHasTranslation (expected, translation);
		}

		[TestCase("Yo me corto el pelo.", "I cut my hair.")]
		[TestCase("Nosotros nos duchamos con agua caliente.", "We shower with hot water.")]
		[TestCase("Tú te miras en el espejo.", "You look at yourself in the mirror.")]
		public void CanDoReflexiveVerbs(string expected, string translation)
		{
			AssertHasTranslation (expected, translation);
		}

		[TestCase("Hay confusión.", "There is confusion.")]
		[TestCase("Hay tres pasos.", "There are three steps.")]
		[TestCase("Ayer hubo confusión.", "Yesterday there was confusion.")]
		[TestCase("Ayer hubo tres pasos.", "Yesterday there were three steps.")]
		[TestCase("Él me dijo que había confusión.", "He told me that there was confusion.")]
        [TestCase("Él me dijo que había tres pasos.", "He told me that there were three steps.")]
		[TestCase("Habrá confusión.", "There will be confusion.")]
		[TestCase("Habrá tres pasos.", "There will be three steps.")]
		[TestCase("Habría confusión.", "There would be confusion.")]
		[TestCase("Habría tres pasos.", "There would be three steps.")]
		[TestCase("Ha habido confusión.", "There has been confusion.")]
		[TestCase("Ha habido tres pasos.", "There have been three steps.")]
		public void HaberIsTricky (string expected, string translation)
		{
			AssertHasTranslation (expected, translation);
		}

		[Test]
		[TestCase ("Tú me haces daño.", "You hurt me.")]
		[TestCase ("Yo me hago daño.", "I hurt myself.")]
		public void CanGivePain (string expected, string translation) 
		{
			AssertHasTranslation (expected, translation);
		}

		[Test]
		[TestCase ("Ayer yo hice un viaje.", 
			new [] {
				"verb:hacer", 
				"verb:hacer-take",
				"usage:hacer-PastPreterite-firstperson"})]
		public void TagsReflectReflexivity (string expected, IEnumerable<string> tags) 
		{
			AssertHasTranslation (expected, expectedTags: tags);
		}

		public void AssertHasTranslation (
			string expected, 
			string translation = null,
			IEnumerable<string> expectedTags = null)
		{
			Exercise exercise;

			try {
				exercise = allExercises.Where (e => e.Original == expected).SingleOrDefault ();
			} catch(InvalidOperationException) {

				var matchingExercises = allExercises.Where (e => e.Original == expected);
				Exercise.WriteAsCsv (Console.Out, matchingExercises);

				throw;
			}

			if (exercise == null) {
				throw new Exception ("Original text not found: " + expected);
			}

			if (!string.IsNullOrEmpty (translation)) {
				Assert.AreEqual (translation, exercise.Translation);
			}

			if (expectedTags != null) {
				foreach (var tag in expectedTags) {
					Assert.Contains (tag, exercise.Tags);
				}
			}
		}
	}
}

