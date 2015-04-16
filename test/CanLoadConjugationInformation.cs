using NUnit.Framework;
using System;
using System.Linq;
using monarquia;
using CsQuery;


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
	}
}

