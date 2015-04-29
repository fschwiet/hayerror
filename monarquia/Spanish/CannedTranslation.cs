using System;
using System.Collections.Generic;

namespace monarquia
{

	public class CannedTranslation : NotComposed
	{
		string spanishText;
		string englishText;
		List<string> englishHints = new List<string>();

		public CannedTranslation (string spanishText, string englishText, bool addAsHintText = false)
		{
			this.spanishText = spanishText;
			this.englishText = englishText;

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
	}
}

