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
			cannedData = new BetterCannedData (dataLoader);
		}

		[Test]
		public void CanBuildScenarios ()
		{
			var timeframe1 = new CannedTranslation ("timeframe1", "timeframe1");
			var incompatibleTimeframe = new CannedTranslation ("incompatible", "incompatible", frameFilter: f => false);
			var pachoNoun = new Noun ("pacho", "pacho");

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
                hasOneOf(Role.timeframe, timeFrames).
				hasOneOf (Role.subject, people).
                hasOneOf(Role.verbEnding, verbEndings).
				hasTranslation ("beber", "drink");

			var result = roleSelector.GetSelectionsFor (Conjugation.Present, false);

            Assert.AreEqual(2, result.Count());
		}

        [Test]
        public void DoesntMixTuAndUsted()
        {
            var roleSelector = new VerbRoleSelector(cannedData, dataLoader).
                    hasOneOf(Role.subject, CannedNouns.GetCommonPeopleSubjectPronouns()).
                    hasOneOf(Role.directObject, CannedNouns.GetCommonPeopleSubjectPronouns()).
                    hasTranslation("conocer", "know");

            var yo = CannedNouns.GetCommonPeopleSubjectPronouns().Where(r => !r.IsPlural && r.Role == Identity.Speaker).Single();
            var tu = CannedNouns.GetCommonPeopleSubjectPronouns().Where(r => !r.IsPlural && r.Role == Identity.Listener).Single();
            var usted = CannedNouns.GetCommonPeopleSubjectPronouns().Where(r => !r.IsPlural && r.Role == Identity.FormalListener).Single();

            var ustedes = CannedNouns.GetCommonPeopleSubjectPronouns().Where(r => r.IsPlural && r.Role == Identity.FormalListener).Single();

            var result = roleSelector.GetSelectionsFor(Conjugation.Present, false);

            Assert.AreEqual(1, result.Count(r => r.GetForRole(Role.subject).UnderlyingObject == yo && r.GetForRole(Role.directObject).UnderlyingObject == tu));

            Assert.AreEqual(1, result.Count(r => r.GetForRole(Role.subject).UnderlyingObject == yo && r.GetForRole(Role.directObject).UnderlyingObject == ustedes));

            Assert.AreEqual(0, result.Count(r => r.GetForRole(Role.subject).UnderlyingObject == tu && r.GetForRole(Role.directObject).UnderlyingObject == usted));

            Assert.AreEqual(0, result.Count(r => r.GetForRole(Role.subject).UnderlyingObject == usted && r.GetForRole(Role.directObject).UnderlyingObject == tu));

            Assert.AreEqual(0, result.Count(r => r.GetForRole(Role.subject).UnderlyingObject == usted && r.GetForRole(Role.directObject).UnderlyingObject == ustedes));
        }
	}
}

