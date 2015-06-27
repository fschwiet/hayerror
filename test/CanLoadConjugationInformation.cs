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
		public void CanLoadGoAndNegate () {
			var allVerbs = dataLoader.GetAllSavedEnglishVerbs ();

			var verb = allVerbs.Single (v => v.Infinitive == "go");

			AssertHasNegativeEnglishConjugation("do not go", verb, Conjugation.Present, PointOfView.FirstPerson);
			AssertHasNegativeEnglishConjugation("did not go", verb, Conjugation.PastPreterite, PointOfView.FirstPerson);
			AssertHasNegativeEnglishConjugation("did not go", verb, Conjugation.PastImperfect, PointOfView.FirstPerson);
			AssertHasNegativeEnglishConjugation("will not go", verb, Conjugation.Future, PointOfView.FirstPerson);
			AssertHasNegativeEnglishConjugation("would not go", verb, Conjugation.Conditional, PointOfView.FirstPerson);
			AssertHasNegativeEnglishConjugation("have not gone", verb, Conjugation.PresentPerfect, PointOfView.FirstPerson);

            // modal verbs are negating by appending "no"
            // https://en.wikipedia.org/wiki/English_modal_verbs

            var modalVerb = allVerbs.Single(v => v.Infinitive == "be");

            AssertHasNegativeEnglishConjugation("am not", modalVerb, Conjugation.Present, PointOfView.FirstPerson);
            AssertHasNegativeEnglishConjugation("was not", modalVerb, Conjugation.PastPreterite, PointOfView.FirstPerson);
            AssertHasNegativeEnglishConjugation("was not", modalVerb, Conjugation.PastImperfect, PointOfView.FirstPerson);
            AssertHasNegativeEnglishConjugation("will not be", modalVerb, Conjugation.Future, PointOfView.FirstPerson);
            AssertHasNegativeEnglishConjugation("would not be", modalVerb, Conjugation.Conditional, PointOfView.FirstPerson);
            AssertHasNegativeEnglishConjugation("have not been", modalVerb, Conjugation.PresentPerfect, PointOfView.FirstPerson);
		}
		

		[Test]
		public void CanLoadHaber() {
			var allVerbs = dataLoader.GetAllSavedSpanishVerbs ();

			var verb = allVerbs.Single (v => v.Infinitive == "haber");

			AssertHasSpanishConjugation("hay", verb, Conjugation.Present, PointOfView.ThirdPersonMasculine);
			AssertHasSpanishConjugation("hubo", verb, Conjugation.PastPreterite, PointOfView.ThirdPersonMasculine);
			AssertHasSpanishConjugation("había", verb, Conjugation.PastImperfect, PointOfView.ThirdPersonMasculine);
			AssertHasSpanishConjugation("habrá", verb, Conjugation.Future, PointOfView.ThirdPersonMasculine);
			AssertHasSpanishConjugation("habría", verb, Conjugation.Conditional, PointOfView.ThirdPersonMasculine);
			AssertHasSpanishConjugation("ha habido", verb, Conjugation.PresentPerfect, PointOfView.ThirdPersonMasculine);

			//  Haber is used in the singular sense
			//    http://spanish.about.com/cs/verbs/a/haber_as_there.htm
			AssertHasSpanishConjugation("habría", verb, Conjugation.Conditional, PointOfView.ThirdPersonPluralFeminine);
			AssertHasSpanishConjugation("habría", verb, Conjugation.Conditional, PointOfView.ThirdPersonPluralMasculine);
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

			var reflexiveVerb = new ReflexiveVerbConjugator (infinitive, dataLoader);
			var frame = new Frame (pointOfView, Conjugation.Present);
			var result = reflexiveVerb.ConjugatedForTense (frame);

			Assert.AreEqual (expected, result);
		}

		void AssertHasSpanishConjugation(string expected, VerbConjugator verb, Conjugation conjugation, PointOfView pointOfView)
		{
			Assert.AreEqual(expected, verb.ConjugatedForTense(new Frame(pointOfView, conjugation)));
		}

		void AssertHasEnglishConjugation(string expected, VerbConjugator verb, Conjugation conjugation, PointOfView pointOfView)
		{
			Assert.AreEqual(expected, verb.ConjugatedForTense(new Frame(pointOfView, conjugation)));
		}

		void AssertHasNegativeEnglishConjugation(string expected, VerbConjugator verb, Conjugation conjugation, PointOfView pointOfView)
		{
			Assert.AreEqual(expected, verb.ConjugatedForTense(new Frame(pointOfView, conjugation, isNegated : true)));
		}
	}
}

