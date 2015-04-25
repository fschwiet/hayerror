using System;

namespace monarquia
{

	public class CannedTranslation : NotComposed
	{
		string spanishText;
		string englishText;

		public CannedTranslation (string spanishText, string englishText)
		{
			this.spanishText = spanishText;
			this.englishText = englishText;
		}

		public override string AsSpanish(PointOfView pointOfView) {
			return spanishText;
		}

		public override string AsEnglish(PointOfView pointOfView) {
			return englishText;
		}
	}
}

