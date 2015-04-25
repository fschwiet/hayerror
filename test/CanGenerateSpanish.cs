﻿using NUnit.Framework;
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

		[TestFixtureSetUp]
		public void LoadVerbs() {

			List<ExerciseGenerator.Exercise> everyExercise = new List<ExerciseGenerator.Exercise> ();

			allExercises = new monarquia.EspanolGenerator("../../../data").GetAll().ToArray();
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
			var exercise = allExercises.Where (e => e.Original == expected).SingleOrDefault ();

			if (exercise == null) {
				throw new Exception ("Original text not found: " + expected);
			}

			if (!string.IsNullOrEmpty (translation)) {
				Assert.AreEqual (translation, exercise.Translated);
			}
		}

		[TestCase("Yo grito.", "I shout.")]
		[TestCase("Ella está en frente.", "She is in front.")]
		[TestCase("Yo estoy en frente.", "I am in front.")]
		public void MiscBugs (string expected, string translation)
		{
			var exercise = allExercises.Where (e => e.Original == expected).SingleOrDefault ();

			if (exercise == null) {
				throw new Exception ("Original text not found: " + expected);
			}

			if (!string.IsNullOrEmpty (translation)) {
				Assert.AreEqual (translation, exercise.Translated);
			}
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

		[TestCase("Él es actor.")]
		[TestCase("Ella es actriz.")]
		[TestCase("Ellos son actores.")]
		[TestCase("Ellas son actrizes.")]
		public void TransferGenderAndNumberForSer(string expected){
			Assert.Contains (expected, allPhrases);
		}
	}
}

