using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{

	public class ReflexiveVerbConjugator : VerbConjugator
	{
		VerbConjugator underlyingVerb;

		public ReflexiveVerbConjugator (string infinitive, DataLoader dataLoader)
		{
			this.underlyingVerb = dataLoader.GetAllSavedSpanishVerbs ().Single (v => v.Infinitive == infinitive);
		}

		public override string Infinitive {
			get {
				return this.underlyingVerb.Infinitive + "se";
			}
		}

		public override string ConjugatedForTense (Frame frame)
		{
			var original = this.underlyingVerb.ConjugatedForTense (frame);

			return GetReflexiveArticle (frame.PointOfView) + " " + original;
		}

		string GetReflexiveArticle(PointOfView pointOfView) 
		{
			switch (pointOfView) {
			case PointOfView.FirstPerson:
				return "me";
			case PointOfView.FirstPersonPlural:
				return "nos";
			case PointOfView.SecondPerson:
				return "te";
			case PointOfView.SecondPersonFormal:
				return "se";
			case PointOfView.SecondPersonPlural:
				return "os";
			case PointOfView.SecondPersonPluralFormal:
				return "se";

			case PointOfView.ThirdPersonFeminine:	
			case PointOfView.ThirdPersonMasculine:
			case PointOfView.ThirdPersonPluralFeminine:
			case PointOfView.ThirdPersonPluralMasculine:
				return "se";
			default:
				throw new InvalidOperationException ();
			}
		}
	}
	
}
