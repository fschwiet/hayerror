using System;

namespace monarquia
{
	public class Noun
	{
		public string Masculine;
		public string Feminine;
		public string PluralMasculine;
		public string PluralFeminine;

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

		public string For(PointOfView pov) {
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
	}
}

