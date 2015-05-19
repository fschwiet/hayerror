using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{
	public abstract class VerbConjugator {
		
		protected VerbConjugator() {
		}

		public abstract string Infinitive {
			get;
		}

		public abstract string ConjugatedForTense (Conjugation conjugation, PointOfView pointOfView);

		public ITranslateable Conjugation(Conjugation conjugation, VerbConjugator englishVerb) 
		{
			return new VerbInstance (this, englishVerb, conjugation);
		}
	}
}

