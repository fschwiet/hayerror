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

			Assert.AreEqual("voy", expected.ConjugatedPresentTense(PointOfView.FirstPerson));
			Assert.AreEqual("he ido", expected.ConjugatedPresentPerfectTense(PointOfView.FirstPerson));
		}
	}
}

