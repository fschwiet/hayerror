using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{

	public class CannedVerbConjugator : VerbConjugator
	{
		readonly string infinitive;
		Dictionary<Conjugation, Dictionary<PointOfView, string>> tenses;

		public CannedVerbConjugator(string infinitiv) {
			this.infinitive = infinitiv;
			this.tenses = new Dictionary<Conjugation, Dictionary<PointOfView, string>> ();
		}

		public VerbConjugator WithTenses (Conjugation conjugation, Dictionary<PointOfView, string> value) {

			if (value.Values.Any(v => v.Contains(","))) {
				throw new Exception(String.Format("WithTenses passed invalid value for verb {0} for conjugation {1}.", infinitive, conjugation));
			}

			tenses [conjugation] = value;
			return this;
		}

		public override string Infinitive {
			get {
				return infinitive;
			}
		}

		public override string ConjugatedForTense(Frame frame) {
			return tenses [frame.Conjugation][frame.PointOfView];
		}

		public void MakeThirdPersonPluralMatchSingular() {
			//  http://spanish.about.com/cs/verbs/a/haber_as_there.htm

			foreach (var conjugation in tenses.Keys) {

				tenses [conjugation] [PointOfView.ThirdPersonPluralMasculine] = 
					tenses [conjugation] [PointOfView.ThirdPersonMasculine];

				tenses [conjugation] [PointOfView.ThirdPersonPluralFeminine] = 
					tenses [conjugation] [PointOfView.ThirdPersonFeminine];
			}
		}
	}
	
}
