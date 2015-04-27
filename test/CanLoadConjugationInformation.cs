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
			var allVerbs = new DataLoader("../../../data").GetAllSpanishVerbs ();

			var expected = allVerbs.Single (v => v.Infinitive == "ir");

			Assert.AreEqual("voy", expected.ConjugatedForTense(Conjugation.Present, PointOfView.FirstPerson));
			Assert.AreEqual("fui", expected.ConjugatedForTense(Conjugation.PastPreterite, PointOfView.FirstPerson));
			Assert.AreEqual("iba", expected.ConjugatedForTense(Conjugation.PastImperfect, PointOfView.FirstPerson));
			Assert.AreEqual("iré", expected.ConjugatedForTense(Conjugation.Future, PointOfView.FirstPerson));
			Assert.AreEqual("iría", expected.ConjugatedForTense(Conjugation.Conditional, PointOfView.FirstPerson));
			Assert.AreEqual("he ido", expected.ConjugatedForTense(Conjugation.PresentPerfect, PointOfView.FirstPerson));
		}

		[Test]
		public void CanLoadGo () {
			var allVerbs = new DataLoader("../../../data").GetAllEnglishVerbs ();

			var expected = allVerbs.Single (v => v.Infinitive == "go");

			Assert.AreEqual("go", expected.ConjugatedForTense(Conjugation.Present, PointOfView.FirstPerson));
			Assert.AreEqual("went", expected.ConjugatedForTense(Conjugation.PastPreterite, PointOfView.FirstPerson));
			Assert.AreEqual("went", expected.ConjugatedForTense(Conjugation.PastImperfect, PointOfView.FirstPerson));
			Assert.AreEqual("will go", expected.ConjugatedForTense(Conjugation.Future, PointOfView.FirstPerson));
			Assert.AreEqual("would go", expected.ConjugatedForTense(Conjugation.Conditional, PointOfView.FirstPerson));
			Assert.AreEqual("have gone", expected.ConjugatedForTense(Conjugation.PresentPerfect, PointOfView.FirstPerson));
		}

		[Test]
		public void MiscConjugationBugs () {
			var allVerbs = new DataLoader("../../../data").GetAllSpanishVerbs ();

			var expected = allVerbs.Single (v => v.Infinitive == "estar");

			Assert.AreEqual("estoy", expected.ConjugatedForTense(Conjugation.Present, PointOfView.FirstPerson));
			Assert.AreEqual("estás", expected.ConjugatedForTense(Conjugation.Present, PointOfView.SecondPerson));
			Assert.AreEqual("está", expected.ConjugatedForTense(Conjugation.Present, PointOfView.ThirdPersonFeminine));
		}
	}
}

