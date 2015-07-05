using System;

namespace monarquia
{
	public class PossessiveAdjective : ITranslateable
	{
        private bool isPlural;
        private bool isFeminine;

		public PossessiveAdjective (
            bool isPlural,
            bool isFeminine)
		{
            this.isPlural = isPlural;
            this.isFeminine = isFeminine;
		}

		public override System.Collections.Generic.IEnumerable<ResultChunk> GetResult (Frame frame)
		{
			return new[] {
				new ResultChunk() {
                    SpanishTranslation = AsSpanish(frame),
					EnglishTranslation = AsEnglish(frame)
				}
			};
		}

        public string AsSpanish (Frame frame)
        {
            string result = null;
            
            switch (frame.PointOfView)
            {
                case PointOfView.FirstPerson:
                    result = "mi";
                    break;
                case PointOfView.FirstPersonPlural:
                    result = isFeminine ? "nuestra" : "nuestro";
                    break;
                case PointOfView.SecondPerson:
                    result = "tu";
                    break;
                case PointOfView.SecondPersonFormal:
                    result = "su";
                    break;
                case PointOfView.SecondPersonPlural:
                    result = isFeminine ? "vuestra" : "vuestro";
                    break;
                case PointOfView.SecondPersonPluralFormal:
                case PointOfView.ThirdPersonMasculine:
                case PointOfView.ThirdPersonFeminine:
                case PointOfView.ThirdPersonPluralMasculine:
                case PointOfView.ThirdPersonPluralFeminine:
                    result = "su";
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return result + (frame.PointOfView.IsPlural() ? "s" : "");
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

