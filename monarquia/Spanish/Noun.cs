using System;

namespace monarquia
{
	public class Noun : NotComposed
	{
		public string Masculine;
		public string Feminine;
		public string PluralMasculine;
		public string PluralFeminine;

		public string EnglishSingular;
		public string EnglishPlural;

		public Noun (string masculine, string feminine, string pluralMasculine, string pluralFeminine)
		{
			Masculine = masculine;
			Feminine = feminine;
			PluralMasculine = pluralMasculine;
			PluralFeminine = pluralFeminine;
		}

		public Noun (string singular, string plural) {
			Masculine = singular;
			Feminine = singular;
			PluralMasculine = plural;
			PluralFeminine = plural;
		}

		public Noun WithTranslation(string singular, string plural) {
			EnglishSingular = singular;
			EnglishPlural = plural;
			return this;
		}

		public override string AsSpanish(PointOfView pov) {
			if (pov.IsFeminine ()) {
				if (pov.IsPlural ()) {
					return PluralFeminine;
				} else {
					return Feminine;
				}
			} else {
				if (pov.IsPlural ()) {
					return PluralMasculine;
				} else {
					return Masculine;
				}
			}
		}

		public override string AsEnglish(PointOfView pointOfView) {
			if (pointOfView.IsPlural())
				return EnglishPlural;
			else
				return EnglishSingular;
		}
	}
}

