using System;
using NUnit.Framework;
using monarquia;


namespace test
{
	[TestFixture]
	public class VerbScenarioBuilderTest
	{
		[Test]
		public void CanBuildScenarios ()
		{
			/*
			   var roleSelector = new VerbRoleSelector("beber")
				.hasOneOf("timeframe", timeframes)
				.hasOneOf("subject", people)
				.hasOneOf("verbEnding", new CannedTranslation("leche", "milk"), new CannedTranslation("agua", "water"))
				.hasTranslation("drink");


			var frame = new Frame(PointOfView.FirstPerson, Conjugation.Present);

			var result = roleSelector(frame);

			var expectedSelection = result.Single(roleSelection =>
			  roleSelection.hasrolevalues...
			  */

			// scenario spec results in function that takes a frame and produces zero or more
			// RoleSelections for that frame
		}

	}
}

