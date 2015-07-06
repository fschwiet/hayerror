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
		public static bool? IsFeminine(this PointOfView pointOfView) {

            if (pointOfView.IsFirstPerson() || pointOfView.IsSecondPerson())
                return null;

			switch(pointOfView) {
				case PointOfView.ThirdPersonFeminine:
				case PointOfView.ThirdPersonPluralFeminine:
					return true;
				default:
					return false;
			}
		}

		public static bool IsSingular(this PointOfView pointOfView) {
			return !pointOfView.IsPlural();
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

		public static bool IsFirstPerson(this PointOfView pointOfView) {
			return pointOfView == PointOfView.FirstPerson || pointOfView == PointOfView.FirstPersonPlural;
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

        public static bool IsFormal(this PointOfView pointOfView)
        {
            return pointOfView == PointOfView.SecondPersonFormal || 
                pointOfView == PointOfView.SecondPersonPluralFormal;
        }
	}
}

