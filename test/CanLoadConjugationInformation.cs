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


		}
	}
}

