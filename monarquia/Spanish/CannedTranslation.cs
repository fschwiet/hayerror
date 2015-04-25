using System;

namespace monarquia
{

	public class CannedTranslation : ITranslateable
	{
		string spanishText;
		string englishText;

		public CannedTranslation (string spanishText, string englishText)
		{
			this.spanishText = spanishText;
			this.englishText = englishText;
		}

		public string AsSpanish(PointOfView pointOfView) {
			return spanishText;
		}

		public string AsEnglish(PointOfView pointOfView) {
			return englishText;
		}
	}
}

