using System;
using System.Collections.Generic;

namespace monarquia
{

	public class CannedTranslation : ITranslateable
	{
		string spanishText;
		string englishText;
		Func<Frame,bool> frameFilter;
		List<string> englishHints = new List<string>();
		List<string> spanishHints = new List<string>();
        List<string> tags = new List<string>();

		public CannedTranslation (string spanishText, string englishText, Func<Frame, bool> frameFilter = null)
		{
			this.spanishText = spanishText;
			this.englishText = englishText;
			this.frameFilter = frameFilter ?? delegate(Frame f) { return true; };

		}

        public CannedTranslation WithTag(params string[] tags)
        {
            this.tags.AddRange(tags);
            return this;
        }

		public CannedTranslation WithEnglishHint(string text = null) {
			englishHints.Add (text ?? spanishText);
			return this;
		}

		public CannedTranslation WithSpanishHint(string text = null) {
			spanishHints.Add (text ?? englishText);
			return this;
		}

		static public CannedTranslation WithPointOfView (string spanishText, string englishText, PointOfView pointOfView) 
		{
			return new CannedTranslation (spanishText, englishText,
				frame => frame.PointOfView == pointOfView);
		}

		static public CannedTranslation WithoutPointOfView (string spanishText, string englishText, PointOfView pointOfView) 
		{
			return new CannedTranslation (spanishText, englishText,
				frame => frame.PointOfView != pointOfView);
		}

		public override IEnumerable<ResultChunk> GetResult (Frame frame)
		{
			return new [] {
				new ResultChunk () {
					SpanishTranslation = spanishText,
					EnglishTranslation = englishText,
					EnglishHint = englishHints,
                    Tags = tags
				}
			};
		}

		public override bool AllowsFraming (Frame frame)
		{
			return frameFilter (frame);
		}
	}
}

