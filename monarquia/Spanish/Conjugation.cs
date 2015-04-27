using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{
	public enum Conjugation {
		Present,
		PastPreterite,
		PastImperfect,
		Future,
		Conditional,
		PresentPerfect
	}

	static public class ConjugationHelpers {
		static public string AsFriendlyString(this Conjugation conjugation) {
			var characters = conjugation.ToString ()
				.SelectMany (c => Char.IsUpper (c) ? new [] { ' ', Char.ToLower (c) } : new [] { c })
				.ToArray ();

			return new string (characters).Trim ();
		}
	}
}
