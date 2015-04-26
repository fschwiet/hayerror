using System;

namespace monarquia
{
	public enum PointOfView {
		FirstPerson,
		SecondPerson,
		SecondPersonFormal,
		ThirdPersonMasculine,
		ThirdPersonFeminine,

		FirstPersonPlural,
		SecondPersonPlural,
		SecondPersonPluralFormal,
		ThirdPersonPluralMasculine,
		ThirdPersonPluralFeminine
	};

	public static class PointOfViewUtils {
		public static bool IsFeminine(this PointOfView pointOfView) {
			switch(pointOfView) {
				case PointOfView.ThirdPersonFeminine:
				case PointOfView.ThirdPersonPluralFeminine:
					return true;
				default:
					return false;
			}
		}

		public static bool IsPlural(this PointOfView pointOfView) {
			switch (pointOfView) {
				case PointOfView.FirstPersonPlural:
				case PointOfView.SecondPersonPlural:
				case PointOfView.SecondPersonPluralFormal:
				case PointOfView.ThirdPersonPluralFeminine:
				case PointOfView.ThirdPersonPluralMasculine:
					return true;
			}

			return false;
		}

		public static bool IsSecondPerson(this PointOfView pointOfView) {
			switch (pointOfView) {
			case PointOfView.SecondPerson:
			case PointOfView.SecondPersonFormal:
			case PointOfView.SecondPersonPlural:
			case PointOfView.SecondPersonPluralFormal:
				return true;
			}

			return false;
		}

		public static bool IsThirdPerson(this PointOfView pointOfView) {
			switch (pointOfView) {
			case PointOfView.ThirdPersonMasculine:
			case PointOfView.ThirdPersonFeminine:
			case PointOfView.ThirdPersonPluralMasculine:
			case PointOfView.ThirdPersonPluralFeminine:
				return true;
			}

			return false;
		}

		public static string AsSubjectPronoun(this PointOfView v) {
			switch (v) {
			case PointOfView.FirstPerson:
				return "yo";
			case PointOfView.SecondPerson:
				return "tú";
			case PointOfView.SecondPersonFormal:
				return "usted";
			case PointOfView.ThirdPersonMasculine:
				return "él";
			case PointOfView.ThirdPersonFeminine:
				return "ella";

			case PointOfView.FirstPersonPlural:
				return "nosotros";
			case PointOfView.SecondPersonPlural:
				return "vosotros";
			case PointOfView.SecondPersonPluralFormal:
				return "ustedes";
			case PointOfView.ThirdPersonPluralMasculine:
				return "ellos";
			case PointOfView.ThirdPersonPluralFeminine:
				return "ellas";
			default:
				throw new Exception ("Unrecognized PointOfView");
			}
		}
		public static string AsEnglishSubjectPronoun(this PointOfView v) {
			switch (v) {
			case PointOfView.FirstPerson:
				return "i";
			case PointOfView.SecondPerson:
				return "you";
			case PointOfView.SecondPersonFormal:
				return "you";
			case PointOfView.ThirdPersonMasculine:
				return "he";
			case PointOfView.ThirdPersonFeminine:
				return "she";

			case PointOfView.FirstPersonPlural:
				return "we";
			case PointOfView.SecondPersonPlural:
				return "you all";
			case PointOfView.SecondPersonPluralFormal:
				return "you all";
			case PointOfView.ThirdPersonPluralMasculine:
				return "they";
			case PointOfView.ThirdPersonPluralFeminine:
				return "they";
			default:
				throw new Exception ("Unrecognized PointOfView");
			}
		}
	}
}

