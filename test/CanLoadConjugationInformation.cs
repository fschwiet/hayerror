using NUnit.Framework;
using System;
using System.Linq;
using monarquia;


namespace test
{
	[TestFixture]
	public class CanLoadConjugationInformation
	{
		DataLoader dataLoader;
		ICannedData cannedData;

		[TestFixtureSetUp]
		public void Setup()
		{
			this.dataLoader = new DataLoader ("../../../data");
			this.cannedData = new BigCannedData (this.dataLoader);
		}

		[Test]
		public void CanLoadIr ()
		{
			var allVerbs = dataLoader.GetAllSavedSpanishVerbs ();

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
			var allVerbs = dataLoader.GetAllSavedEnglishVerbs ();

			var verb = allVerbs.Single (v => v.Infinitive == "go");

			AssertHasEnglishConjugation("go", verb, Conjugation.Present, PointOfView.FirstPerson);
			AssertHasEnglishConjugation("went", verb, Conjugation.PastPreterite, PointOfView.FirstPerson);
			AssertHasEnglishConjugation("went", verb, Conjugation.PastImperfect, PointOfView.FirstPerson);
			AssertHasEnglishConjugation("will go", verb, Conjugation.Future, PointOfView.FirstPerson);
			AssertHasEnglishConjugation("would go", verb, Conjugation.Conditional, PointOfView.FirstPerson);
			AssertHasEnglishConjugation("have gone", verb, Conjugation.PresentPerfect, PointOfView.FirstPerson);
		}

		[Test]
		public void CanLoadHaber() {
			var allVerbs = dataLoader.GetAllSavedSpanishVerbs ();

			var verb = allVerbs.Single (v => v.Infinitive == "haber");

			AssertHasEnglishConjugation("hay", verb, Conjugation.Present, PointOfView.ThirdPersonMasculine);
			AssertHasEnglishConjugation("hubo", verb, Conjugation.PastPreterite, PointOfView.ThirdPersonMasculine);
			AssertHasEnglishConjugation("había", verb, Conjugation.PastImperfect, PointOfView.ThirdPersonMasculine);
			AssertHasEnglishConjugation("habrá", verb, Conjugation.Future, PointOfView.ThirdPersonMasculine);
			AssertHasEnglishConjugation("habría", verb, Conjugation.Conditional, PointOfView.ThirdPersonMasculine);
			AssertHasEnglishConjugation("ha habido", verb, Conjugation.PresentPerfect, PointOfView.ThirdPersonMasculine);

			//  Haber is used in the singular sense
			//    http://spanish.about.com/cs/verbs/a/haber_as_there.htm
			AssertHasEnglishConjugation("habría", verb, Conjugation.Conditional, PointOfView.ThirdPersonPluralFeminine);
			AssertHasEnglishConjugation("habría", verb, Conjugation.Conditional, PointOfView.ThirdPersonPluralMasculine);
		}

		[Test]
		public void MiscConjugationBugs () {

			var allVerbs = dataLoader.GetAllSavedSpanishVerbs ();

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
			var allVerbs = dataLoader.GetAllSavedSpanishVerbs ();

			var verb = allVerbs.SingleOrDefault (v => v.Infinitive == infinitive);

			Assert.IsNotNull (verb);

			var reflexiveVerb = new ReflexiveVerb (infinitive, dataLoader);

			var result = reflexiveVerb.Conjugation (Conjugation.Present, null).AsSpanish (pointOfView);

			Assert.AreEqual (expected, result);
		}

		void AssertHasSpanishConjugation(string expected, Verb verb, Conjugation conjugation, PointOfView pointOfView)
		{
			Assert.AreEqual(expected, verb.ConjugatedForTense(conjugation, pointOfView));
		}

		void AssertHasEnglishConjugation(string expected, Verb verb, Conjugation conjugation, PointOfView pointOfView)
		{
			Assert.AreEqual(expected, verb.ConjugatedForTense(conjugation, pointOfView));
		}
	}
}

