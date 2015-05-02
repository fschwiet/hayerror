using NUnit.Framework;
using System;
using System.Linq;
using monarquia;


namespace test
{
	[TestFixture]
	public class CanLoadConjugationInformation
	{
		[Test]
		public void CanLoadIr ()
		{
			var allVerbs = new DataLoader("../../../data").GetAllSavedSpanishVerbs ();

			var verb = allVerbs.Single (v => v.Infinitive == "ir");

			AssertHasSpanishConjugation("voy", verb, Conjugation.Present, PointOfView.FirstPerson);
			AssertHasSpanishConjugation("fui", verb, Conjugation.PastPreterite, PointOfView.FirstPerson);
			AssertHasSpanishConjugation("iba", verb, Conjugation.PastImperfect, PointOfView.FirstPerson);
			AssertHasSpanishConjugation("iré", verb, Conjugation.Future, PointOfView.FirstPerson);
			AssertHasSpanishConjugation("iría", verb, Conjugation.Conditional, PointOfView.FirstPerson);
			AssertHasSpanishConjugation("he ido", verb, Conjugation.PresentPerfect, PointOfView.FirstPerson);
		}

		[Test]
		public void CanLoadGo () {
			var allVerbs = new DataLoader("../../../data").GetAllSavedEnglishVerbs ();

			var verb = allVerbs.Single (v => v.Infinitive == "go");

			AssertHasEnglishConjugation("go", verb, Conjugation.Present, PointOfView.FirstPerson);
			AssertHasEnglishConjugation("went", verb, Conjugation.PastPreterite, PointOfView.FirstPerson);
			AssertHasEnglishConjugation("went", verb, Conjugation.PastImperfect, PointOfView.FirstPerson);
			AssertHasEnglishConjugation("will go", verb, Conjugation.Future, PointOfView.FirstPerson);
			AssertHasEnglishConjugation("would go", verb, Conjugation.Conditional, PointOfView.FirstPerson);
			AssertHasEnglishConjugation("have gone", verb, Conjugation.PresentPerfect, PointOfView.FirstPerson);
		}

		[Test]
		public void MiscConjugationBugs () {

			var allVerbs = new DataLoader("../../../data").GetAllSavedSpanishVerbs ();

			var verb = allVerbs.Single (v => v.Infinitive == "estar");

			AssertHasSpanishConjugation("estoy", verb, Conjugation.Present, PointOfView.FirstPerson);
			AssertHasSpanishConjugation("estás", verb, Conjugation.Present, PointOfView.SecondPerson);
			AssertHasSpanishConjugation("está", verb, Conjugation.Present, PointOfView.ThirdPersonFeminine);
		}

		[Test]
		[TestCase("me corto", "cortar", PointOfView.FirstPerson)]
		[TestCase("nos cortamos", "cortar", PointOfView.FirstPersonPlural)]
		[TestCase("te cortas", "cortar", PointOfView.SecondPerson)]
		[TestCase("se corta", "cortar", PointOfView.SecondPersonFormal)]
		[TestCase("os cortáis", "cortar", PointOfView.SecondPersonPlural)]
		[TestCase("se cortan", "cortar", PointOfView.SecondPersonPluralFormal)]
		[TestCase("se corta", "cortar", PointOfView.ThirdPersonMasculine)]
		[TestCase("se corta", "cortar", PointOfView.ThirdPersonFeminine)]
		[TestCase("se cortan", "cortar", PointOfView.ThirdPersonPluralMasculine)]
		[TestCase("se cortan", "cortar", PointOfView.ThirdPersonPluralFeminine)]
		public void CanConjugateReflexively(string expected, string infinitive, PointOfView pointOfView)
		{
			var dataLoader = new DataLoader ("../../../data");
			var allVerbs = dataLoader.GetAllSavedSpanishVerbs ();

			var verb = allVerbs.SingleOrDefault (v => v.Infinitive == infinitive);

			Assert.IsNotNull (verb);

			var reflexiveVerb = new ReflexiveVerb (infinitive, dataLoader);

			var result = reflexiveVerb.ForSpanishConjugation (Conjugation.Present).AsSpanish (pointOfView);

			Assert.AreEqual (expected, result);
		}

		void AssertHasSpanishConjugation(string expected, Verb verb, Conjugation conjugation, PointOfView pointOfView)
		{
			Assert.AreEqual(expected, verb.ForSpanishConjugation(conjugation).AsSpanish(pointOfView));
		}

		void AssertHasEnglishConjugation(string expected, Verb verb, Conjugation conjugation, PointOfView pointOfView)
		{
			Assert.AreEqual(expected, verb.ForEnglishConjugation(conjugation).AsEnglish(pointOfView));
		}
	}
}

