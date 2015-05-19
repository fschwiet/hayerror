using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{
	public interface ICannedData {
		IEnumerable<ITranslateable> GetVerbEndings (string verbInfinitive, PointOfView pointOfView);
		IEnumerable<ITranslateable> GetTimeframeExpressions ();
		VerbConjugator TranslateVerbFromSpanishToEnglish(DataLoader loader, VerbConjugator verb, Conjugation conjugation);
		IEnumerable<string> GetReflexiveVerbs (DataLoader dataLoader);
		IEnumerable<RoleSelection> GetAllRoleScenariosForVerbAndFrame (Random random, VerbConjugator verb, DataLoader dataLoader, Frame frame);
		IEnumerable<ExpressableVerbRoleSelection> GetAllVerbRoleSelectors();
	}
	
}
