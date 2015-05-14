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

		public CannedTranslation (string spanishText, string englishText, bool addAsHintText = false, Func<Frame, bool> frameFilter = null)
		{
			this.spanishText = spanishText;
			this.englishText = englishText;
			this.frameFilter = frameFilter ?? delegate(Frame f) { return true; };

			if (addAsHintText) {
				englishHints.Add (spanishText);
			}
		}

		static public CannedTranslation WithPointOfView (string spanishText, string englishText, PointOfView pointOfView) 
		{
			return new CannedTranslation (spanishText, englishText, false,
				frame => frame.PointOfView == pointOfView);
		}


		public override IEnumerable<ResultChunk> GetResult (Frame frame)
		{
			return new [] {
				new ResultChunk () {
					SpanishTranslation = spanishText,
					EnglishTranslation = englishText,
					EnglishHint = englishHints 
				}
			};
		}

		public override bool AllowsFraming (Frame frame)
		{
			return frameFilter (frame);
		}
	}
}

