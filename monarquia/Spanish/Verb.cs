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

		public VerbInstance ForConjugation(Conjugation conjugation) 
		{
			return new VerbInstance (this, conjugation);
		}
	}

	public class VerbInstance : ITranslateable
	{
		Verb verb;
		Conjugation conjugation;

		public VerbInstance(Verb verb, Conjugation conjugation)
		{
			this.verb = verb;
			this.conjugation = conjugation;
		}

		public override string AsSpanish (PointOfView pointOfView)
		{
			return verb.ConjugatedForTense (conjugation, pointOfView);
		}

		public override string AsEnglish (PointOfView pointOfView)
		{
			return verb.ConjugatedForTense (conjugation, pointOfView);
		}
	}
}

