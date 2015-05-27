using System;
using System.Collections.Generic;

namespace monarquia
{
	public class PotentialNegationPreposition
	{
		public static IEnumerable<ITranslateable> Get ()
		{
			return new [] {
				new CannedTranslation ("no", "", f => f.IsNegated),
				new CannedTranslation ("", "", f => !f.IsNegated)
			};
		}
	}
}

