using System;
using System.Collections.Generic;

namespace monarquia
{
	public class Verb {

		public enum Conjugation {
			Present,
			PastPreterite,
			PastImperfect,
			Future,
			Conditional,
			PresentPerfect
		}

		public readonly string Infinitive;
		public Dictionary<Conjugation, Dictionary<PointOfView, string>> Tenses;

		public Verb(string infinitiv) {
			Infinitive = infinitiv;
			Tenses = new Dictionary<Conjugation, Dictionary<PointOfView, string>> ();
		}

		public Verb WithTenses (Conjugation conjugation, Dictionary<PointOfView, string> value) {
			Tenses [conjugation] = value;
			return this;
		}

		public string ConjugatedForTense(Conjugation conjugation, PointOfView pointOfView) {
			return Tenses [conjugation][pointOfView];
		}

		public string EnglishConjugatedForTense(Conjugation conjugation, PointOfView pointOfView) {
			switch (conjugation) {
			case Conjugation.Present:
				if (pointOfView.IsThirdPerson () && !pointOfView.IsPlural()) {
					return "goes";
				}

				return "go";
			case Conjugation.PastPreterite:
				return "went";
			case Conjugation.PastImperfect:
				return "went";
			case Conjugation.Future:
				break;
			case Conjugation.Conditional:
				break;
			case Conjugation.PresentPerfect:
				break;
			}

			return "<not implemented>";
		}
	}
}

