using System;
using System.Collections.Generic;

namespace monarquia
{
	public class Verb {

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
	}
}

