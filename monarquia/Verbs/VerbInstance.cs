using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{
	public class VerbInstance : ITranslateable
	{
		VerbConjugator spanishVerb;
		VerbConjugator englishVerb;
		Func<Frame, bool> framing;

		public VerbInstance(VerbConjugator spanishVerb, VerbConjugator englishVerb, Func<Frame,bool> framing)
		{
			this.spanishVerb = spanishVerb;
			this.englishVerb = englishVerb;
			this.framing = framing;
		}

		public override IEnumerable<ResultChunk> GetResult (Frame frame)
		{
			var usageString = spanishVerb.Infinitive + "-" + frame.Conjugation + "-";

            switch(frame.PointOfView)
            {
                case PointOfView.FirstPerson:
                    usageString += "firstperson";
                    break;
                case PointOfView.FirstPersonPlural:
                    usageString += "firstpersonplural";
                    break;
                case PointOfView.SecondPerson:
                    usageString += "secondperson";
                    break;
                case PointOfView.SecondPersonPlural:
                    usageString += "secondpersonplural";
                    break;
                case PointOfView.ThirdPersonMasculine:
                case PointOfView.ThirdPersonFeminine:
                case PointOfView.SecondPersonFormal:
                    usageString += "thirdperson";
                    break;
                case PointOfView.ThirdPersonPluralMasculine:
                case PointOfView.ThirdPersonPluralFeminine:
                case PointOfView.SecondPersonPluralFormal:
                    usageString += "thirdpersonplural";
                    break;
            }

			return new [] { new ResultChunk () {
				SpanishTranslation = spanishVerb.ConjugatedForTense (frame),
				EnglishTranslation = englishVerb == null ? null : englishVerb.ConjugatedForTense (frame),
				SpanishHint = new string[0],
				EnglishHint = new string[0],
				Tags = new [] { 
						"verb:" + spanishVerb.Infinitive, 
						"verb:" + spanishVerb.Infinitive + "-" + englishVerb.Infinitive,
						"usage:" + usageString }
				}
			};
		}

		public override bool AllowsFraming (Frame frame)
		{
			return this.framing (frame);
		}
	}
}
