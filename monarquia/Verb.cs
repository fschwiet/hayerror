using System;
using System.Collections.Generic;

namespace monarquia
{
	public class Verb {

		public readonly string Infinitive;
		public Dictionary<PointOfView, string> PresentTense;

		public Verb(string infinitiv) {
			Infinitive = infinitiv;
		}

		public Verb WithPresentTenses (Dictionary<PointOfView, string> value)
		{
			PresentTense = value;

			return this;
		}

		public string ConjugatedPresentTense(PointOfView v) {
			return PresentTense [v];
		}
	}
}

