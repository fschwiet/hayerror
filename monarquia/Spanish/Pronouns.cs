using System;

namespace monarquia
{
	public static class Pronouns
	{
		public static ITranslateable GetSubjectNoun(this PointOfView v) {
			switch (v) {
			case PointOfView.FirstPerson:
				return new CannedTranslation ("yo", "I");
			case PointOfView.SecondPerson:
				return new CannedTranslation ("tú", "you");
			case PointOfView.SecondPersonFormal:
				return new CannedTranslation ("usted", "you");
			case PointOfView.ThirdPersonMasculine:
				return new CannedTranslation ("él", "he");
			case PointOfView.ThirdPersonFeminine:
				return new CannedTranslation ("ella", "she");

			case PointOfView.FirstPersonPlural:
				return new CannedTranslation ("nosotros", "we");
			case PointOfView.SecondPersonPlural:
				return new CannedTranslation ("vosotros", "you all");
			case PointOfView.SecondPersonPluralFormal:
				return new CannedTranslation ("ustedes", "you all");
			case PointOfView.ThirdPersonPluralMasculine:
				return new CannedTranslation ("ellos", "they");
			case PointOfView.ThirdPersonPluralFeminine:
				return new CannedTranslation ("ellas", "they");
			default:
				throw new Exception ("Unrecognized PointOfView");
			}
		}
	}
}

