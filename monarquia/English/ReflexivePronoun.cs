using System;

namespace monarquia
{
	public class ReflexivePronoun : EnglishOnly
	{
		public override string AsEnglish (PointOfView pointOfView)
		{
			switch (pointOfView) 
			{
			case PointOfView.FirstPerson:
				return "myself";
			case PointOfView.FirstPersonPlural:
				return "ourselves";
			case PointOfView.SecondPerson:
			case PointOfView.SecondPersonFormal:
				return "yourself";
			case PointOfView.SecondPersonPlural:
			case PointOfView.SecondPersonPluralFormal:
				return "yourselves";
			case PointOfView.ThirdPersonMasculine:
				return "himself";
			case PointOfView.ThirdPersonFeminine:
				return "herself";
			case PointOfView.ThirdPersonPluralMasculine:
			case PointOfView.ThirdPersonPluralFeminine:
				return "themselves";
			default:
				throw new InvalidOperationException ();
			}
		}
	}
}

