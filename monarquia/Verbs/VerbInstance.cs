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
			var usageString = spanishVerb.Infinitive + "-" + frame.Conjugation + "-" + frame.PointOfView;

			return new [] { new ResultChunk () {
				SpanishTranslation = spanishVerb.ConjugatedForTense (frame),
				EnglishTranslation = englishVerb == null ? null : englishVerb.ConjugatedForTense (frame),
				SpanishHint = new string[0],
				EnglishHint = new string[0],
				Tags = new [] { 
						"verb:" + spanishVerb.Infinitive, 
						"verb:" + spanishVerb.Infinitive + "-" + englishVerb.Infinitive,
						"usage:" + usageString },
				ExtraInfo = new [] { "verb " + spanishVerb.Infinitive, frame.Conjugation.AsFriendlyString () }
				}
			};
		}

		public override bool AllowsFraming (Frame frame)
		{
			return this.framing (frame);
		}
	}
}
