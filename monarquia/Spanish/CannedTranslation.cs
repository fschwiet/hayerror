using System;
using System.Collections.Generic;

namespace monarquia
{

	public class CannedTranslation : NotComposed
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

		public override string AsSpanish(PointOfView pointOfView) {
			return spanishText;
		}

		public override string AsEnglish(PointOfView pointOfView) {
			return englishText;
		}

		public override IEnumerable<string> GetEnglishHints ()
		{
			return englishHints;
		}

		public override bool AllowsFraming (Frame frame)
		{
			return frameFilter (frame);
		}
	}
}

