using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{

	public class VerbInstance : ITranslateable
	{
		VerbConjugator spanishVerb;
		VerbConjugator englishVerb;
		Conjugation conjugation;

		public VerbInstance(VerbConjugator spanishVerb, VerbConjugator englishVerb, Conjugation conjugation)
		{
			this.spanishVerb = spanishVerb;
			this.englishVerb = englishVerb;
			this.conjugation = conjugation;
		}

		public override IEnumerable<ResultChunk> GetResult (Frame frame)
		{
			return new [] { new ResultChunk () {
				SpanishTranslation = spanishVerb.ConjugatedForTense (frame.Conjugation, frame.PointOfView),
				EnglishTranslation = englishVerb == null ? null : englishVerb.ConjugatedForTense (frame.Conjugation, frame.PointOfView),
				SpanishHint = new string[0],
				EnglishHint = new string[0],
				Tags = new [] { "verb:" + spanishVerb.Infinitive, "conjugation:" + conjugation, frame.PointOfView.AsTagString () },
				ExtraInfo = new [] { "verb " + spanishVerb.Infinitive, conjugation.AsFriendlyString () }
				}
			};
		}

		public override bool AllowsFraming (Frame frame)
		{
			return frame.Conjugation == this.conjugation;
		}
	}
}
