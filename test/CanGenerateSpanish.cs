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
		ExerciseGenerator.Exercise[] allExercises;
		string[] allPhrases;

		public void AssertHasTranslation (string expected, string translation)
		{
			ExerciseGenerator.Exercise exercise;

			try {
				exercise = allExercises.Where (e => e.Original == expected).SingleOrDefault ();
			} catch(InvalidOperationException) {
				
				var matchingExercises = allExercises.Where (e => e.Original == expected);
				ExerciseGenerator.Exercise.WriteAsCsv (Console.Out, matchingExercises);

				throw;
			}

			if (exercise == null) {
				throw new Exception ("Original text not found: " + expected);
			}

			if (!string.IsNullOrEmpty (translation)) {
				Assert.AreEqual (translation, exercise.Translated);
			}
		}

		[TestFixtureSetUp]
		public void LoadVerbs() {

			var dataDirectory = "../../../data";
			allExercises = new monarquia.EspanolGenerator(new BigCannedData( new DataLoader(dataDirectory)), dataDirectory).GetExercises().ToArray();
			allPhrases = allExercises.Select (e => e.Original).ToArray ();
		}

		[Test]
		[TestCase("Yo voy al cine.", "I go to the movies.")]
		[TestCase("Tú vas al cine.", "You go to the movies.")]		
		[TestCase("Usted va al cine.", "You go to the movies.")]
		[TestCase("Él va al cine.", "He goes to the movies.")]
		[TestCase("Ella va al cine.", "She goes to the movies.")]

		[TestCase("Nosotros vamos al cine.", "We go to the movies.")]
		[TestCase("Vosotros vais al cine.", "You all go to the movies.")]
		[TestCase("Ustedes van al cine.", "You all go to the movies.")]
		[TestCase("Ellos van al cine.", "They go to the movies.")]
		[TestCase("Ellas van al cine.", "They go to the movies.")]
		public void PresentTenseIntransitive (string expected, string translation)
		{
			AssertHasTranslation (expected, translation);
		}

		[TestCase("Yo grito.", "I shout.")]
		[TestCase("Ella está en frente.", "She is in front.")]
		[TestCase("Yo estoy en frente.", "I am in front.")]
		public void MiscBugs (string expected, string translation)
		{
			AssertHasTranslation (expected, translation);
		}

		[TestCase("Yo soy cocinero.", "I am a cook.")]
		[TestCase("Ella es actriz.", "She is an actor.")]
		[TestCase("Tú eres cocinero.", "You are a cook.")]
		[TestCase("Nosotros somos cocineros.", "We are cooks.")]
		[TestCase("Ellos son cocineros.", "They are cooks.")]
		public void IsProfessionExpressionsOftenLackAnArticle (string expected, string translation)
		{
			AssertHasTranslation (expected, translation);
		}

		[TestCase("Yo preparo la cena.", null /*"I prepare the dinner."  prepare doesn't have conjugation downloadable on SD */)]
		[TestCase("Ellos suben la escalera.", "They climb the stairs.")]
		[TestCase("Ellas beben leche.", "They drink milk.")]
		[TestCase("Él suma la cuenta.", null/*, "He adds up the check."*/)]
		[TestCase("Ella habla a la reportera.", "She talks to the reporter.")]
		[TestCase("Él teme a los críticos.", "He fears the critics.")]
		[TestCase("Ellos comen fajitas.", "They eat fajitas.")]
		public void PresentTenseTransitive (string expected, string translation)
		{
			AssertHasTranslation (expected, translation);
		}

		[TestCase("Yo he hablado con él.", "I have talked to him.")]
		public void PresentPerfectTense (string expected, string translation)
		{
			AssertHasTranslation (expected, translation);
		}

		[TestCase("Ahora yo preparo la cena.")]
		[TestCase("Normalmente yo preparo la cena.")]
		[TestCase("Esta mes yo preparo la cena.")]
		[TestCase("Anoche yo preparé la cena.")]
		[TestCase("Algunas veces yo preparaba la cena.")]
		[TestCase("Yo prepararé la cena.")]
		[TestCase("Mañana yo prepararé la cena.")]
		[TestCase("Por supuesto yo prepararía la cena.")]
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


		[TestCase("Yo conozco a ella.", "I know her.")]
		[TestCase("Ayer yo conocí a ella.", "Yesterday I met her.")]
		[TestCase("Todos los años yo conocía a ella.", "Every year I knew her.")]
		[TestCase("Yo conocería a ella.", "I would know her.")]
		[TestCase("Yo conoceré a ella.", "I will meet her.")]
		[TestCase("Ellas han conocido a ella.", "They have known her.")]

		// nunca ha conocido -> has never met
		public void HandlesVerbsThatChangeInterpretationWithTense(string expected, string translation)
		{
			AssertHasTranslation (expected, translation);
		}

		[TestCase("Yo me corto el pelo.", "I cut my hair.")]
		[TestCase("Nosotros nos duchamos.", "We shower.")]
		[TestCase("Tú te miras en el espejo.", "You look at yourself in the mirror.")]
		public void CanDoReflexiveVerbs(string expected, string translation)
		{
			AssertHasTranslation (expected, translation);
		}

		[TestCase("Yo voy al cine.", new [] {"verb:ir", "conjugation:present"})]
		public void ExercisesIncludeTags(string expectedPhrase, IEnumerable<string> expectedTags)
		{
			var exercise = allExercises.Single (e => e.Original == expectedPhrase);

			Assert.Contains ("conjugation:Present", exercise.Tags);
			Assert.Contains ("verb:ir", exercise.Tags);
		}

		[TestCase("Hay confusión.", "There is confusion.")]
		[TestCase("Hay tres pasos.", "There are three steps.")]
		[TestCase("Ayer hubo confusión.", "Yesterday there was confusion.")]
		[TestCase("Ayer hubo tres pasos.", "Yesterday there were three steps.")]
		[TestCase("Todos los años había confusión.", "Every year there was confusion.")]
		[TestCase("Todos los años había tres pasos.", "Every year there were three steps.")]
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
	}
}

