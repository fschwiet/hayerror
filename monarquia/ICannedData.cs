using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{
	public interface ICannedData {
		IEnumerable<ITranslateable> GetVerbEndings (string verbInfinitive, PointOfView pointOfView);
		IEnumerable<ITranslateable> GetTimeframeExpressions (Conjugation conjugation);
		Verb TranslateVerbFromSpanishToEnglish(DataLoader loader, Verb verb, Conjugation conjugation);
		IEnumerable<string> GetReflexiveVerbs (DataLoader dataLoader);
	}
	
}