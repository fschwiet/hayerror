using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using monarquia;


namespace test
{
	[TestFixture]
	public class VerbScenarioBuilderTest
	{
		ICannedData cannedData;
		DataLoader dataLoader;

		[TestFixtureSetUp]
		public void LoadVerbs() {

			var dataDirectory = "../../../data";
			dataLoader = new DataLoader (dataDirectory);
			cannedData = new BigCannedData (dataLoader);
		}

		[Test]
		public void CanBuildScenarios ()
		{
			var timeframe1 = new CannedTranslation ("timeframe1", "timeframe1");
			var incompatibleTimeframe = new CannedTranslation ("incompatible", "incompatible", frameFilter: f => false);
			var pachoNoun = new CannedTranslation ("pacho", "pacho");

			var timeFrames = new [] {
				new CannedTranslation ("", ""),
				timeframe1,
				incompatibleTimeframe
			};

			var people = new [] {
				pachoNoun
			};

			var verbEndings = new [] {
				new CannedTranslation ("", "")
			};

			var roleSelector = new VerbRoleSelector (cannedData, dataLoader).
				hasOneOf ("timeframe", timeFrames).
				hasOneOf ("subject", people).
				hasOneOf ("verbEnding", verbEndings).
				hasTranslation ("beber", "drink");

			var frame = new Frame (PointOfView.ThirdPersonMasculine, Conjugation.Present);

			var result = roleSelector.GetSelectionsFor (frame);

            Assert.AreEqual(2, result.Count());
		}
	}
}

