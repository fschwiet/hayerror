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
				case PointOfView.ThirdPersonPluralFeminine:
				case PointOfView.ThirdPersonPluralMasculine:
					return true;
			}

			return false;
		}
	}
}

