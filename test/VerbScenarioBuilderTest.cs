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
			cannedData = new BigCannedData ();
			dataLoader = new DataLoader (dataDirectory);
		}

		[Test]
		public void CanBuildScenarios ()
		{
			var timeFrames = new [] {
				new CannedTranslation ("", ""),
				new CannedTranslation ("timeframe1", "timeframe1"),
				new CannedTranslation ("incompatible", "incompatible", frameFilter: f => false)
			};

			var people = new [] {
				new CannedTranslation ("pacho", "pacho")
			};

			var verbEndings = new [] {
				new CannedTranslation ("", "")
			};

			var roleSelector = new VerbRoleSelector ("beber").
				hasOneOf ("timeframe", timeFrames).
				hasOneOf ("subject", people).
				hasOneOf ("verbEnding", verbEndings)
				.hasTranslation ("drink", cannedData, dataLoader);

			var pointOfView = PointOfView.ThirdPersonMasculine;
			var frame = new Frame (pointOfView, Conjugation.Present);

			var result = roleSelector.GetSelectionsFor (frame);

			var expectedSelection = result.First (roleSelection => 
				roleSelection.GetForRole ("timeframe").AsSpanish (pointOfView).Equals ("timeframe1")
			    && roleSelection.GetForRole ("subject").AsSpanish (pointOfView).Equals ("pacho"));

			Assert.AreEqual ("bebe", expectedSelection.GetForRole ("verbPhrase").AsSpanish (pointOfView));
			Assert.AreEqual ("drinks", expectedSelection.GetForRole ("verbPhrase").AsEnglish (pointOfView));

			Assert.That (!result.Any (r => r.GetForRole ("timeframe").AsSpanish (pointOfView).Equals ("incompatible")));
		}
	}
}

