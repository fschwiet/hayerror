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
				return new CannedTranslation ("tú", "you", true);
			case PointOfView.SecondPersonFormal:
				return new CannedTranslation ("usted", "you", true);
			case PointOfView.ThirdPersonMasculine:
				return new CannedTranslation ("él", "he");
			case PointOfView.ThirdPersonFeminine:
				return new CannedTranslation ("ella", "she");

			case PointOfView.FirstPersonPlural:
				return new CannedTranslation ("nosotros", "we");
			case PointOfView.SecondPersonPlural:

				//  We don't use vosotros now, but when used we might
				//  want better disambiguation from ustedes
				return new CannedTranslation ("vosotros", "you all", true);
			case PointOfView.SecondPersonPluralFormal:
				return new CannedTranslation ("ustedes", "you all");
			case PointOfView.ThirdPersonPluralMasculine:
				return new CannedTranslation ("ellos", "they", true);
			case PointOfView.ThirdPersonPluralFeminine:
				return new CannedTranslation ("ellas", "they", true);
			default:
				throw new Exception ("Unrecognized PointOfView");
			}
		}
	}
}

