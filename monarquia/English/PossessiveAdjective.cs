using System;

namespace monarquia
{
	public class PossessiveAdjective : EnglishOnly
	{
		public PossessiveAdjective ()
		{
		}

		public override System.Collections.Generic.IEnumerable<ResultChunk> GetResult (Frame frame)
		{
			return new[] {
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
				return "my";
			case PointOfView.FirstPersonPlural:
				return "our";
			case PointOfView.SecondPerson:
			case PointOfView.SecondPersonFormal:
			case PointOfView.SecondPersonPlural:
			case PointOfView.SecondPersonPluralFormal:
				return "your";
			case PointOfView.ThirdPersonMasculine:
				return "his";
			case PointOfView.ThirdPersonFeminine:
				return "her";
			case PointOfView.ThirdPersonPluralMasculine:
			case PointOfView.ThirdPersonPluralFeminine:
				return "their";
			default:
				throw new InvalidOperationException ();
			}
		}
	}
}

