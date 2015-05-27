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

		public abstract string ConjugatedForTense (Frame frame);

		public ITranslateable MakeTranslateable(VerbConjugator englishVerb, Func<Frame,bool> framing) 
		{
			return new VerbInstance (this, englishVerb, framing);
		}
	}
}

