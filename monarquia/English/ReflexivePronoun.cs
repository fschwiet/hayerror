using System;

namespace monarquia
{
	public class ReflexivePronoun : EnglishOnly
	{
		public override System.Collections.Generic.IEnumerable<ResultChunk> GetResult (Frame frame)
		{
			return new [] {
				new ResultChunk() {
					EnglishTranslation = AsEnglish(frame)
				}
			};
		}

		public string AsEnglish (Frame frame)
		{
			switch (frame.PointOfView) 
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

