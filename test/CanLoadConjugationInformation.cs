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
			var allVerbs = new DataLoader("../../../data").GetAllVerbs ();

			var expected = allVerbs.Single (v => v.Infinitive == "ir");

			Assert.AreEqual("voy", expected.ConjugatedForTense(Verb.Conjugation.Present, PointOfView.FirstPerson));
			Assert.AreEqual("fui", expected.ConjugatedForTense(Verb.Conjugation.PastPreterite, PointOfView.FirstPerson));
			Assert.AreEqual("iba", expected.ConjugatedForTense(Verb.Conjugation.PastImperfect, PointOfView.FirstPerson));
			Assert.AreEqual("iré", expected.ConjugatedForTense(Verb.Conjugation.Future, PointOfView.FirstPerson));
			Assert.AreEqual("iría", expected.ConjugatedForTense(Verb.Conjugation.Conditional, PointOfView.FirstPerson));
			Assert.AreEqual("he ido", expected.ConjugatedForTense(Verb.Conjugation.PresentPerfect, PointOfView.FirstPerson));
		}

		[Test]
		public void MiscConjugationBugs () {
			var allVerbs = new DataLoader("../../../data").GetAllVerbs ();

			var expected = allVerbs.Single (v => v.Infinitive == "estar");

			Assert.AreEqual("estoy", expected.ConjugatedForTense(Verb.Conjugation.Present, PointOfView.FirstPerson));
			Assert.AreEqual("estás", expected.ConjugatedForTense(Verb.Conjugation.Present, PointOfView.SecondPerson));
			Assert.AreEqual("está", expected.ConjugatedForTense(Verb.Conjugation.Present, PointOfView.ThirdPersonFeminine));
		}
	}
}

