using System;
using System.Collections.Generic;

namespace monarquia
{
	public class Verb {

		public readonly string Infinitive;
		public Dictionary<PointOfView, string> PresentTense;
		public Dictionary<PointOfView, string> PresentPerfectTense;

		public Verb(string infinitiv) {
			Infinitive = infinitiv;
		}

		public Verb WithPresentTenses (Dictionary<PointOfView, string> value)
		{
			PresentTense = value;

			return this;
		}
		public Verb WithPresentPerfectTenses (Dictionary<PointOfView, string> value)
		{
			PresentPerfectTense = value;

			return this;
		}

		public string ConjugatedPresentTense(PointOfView v) {
			return PresentTense [v];
		}

		public string ConjugatedPresentPerfectTense(PointOfView v) {
			return PresentPerfectTense [v];
		}
	}
}

