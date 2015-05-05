using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{
	public interface ICannedData {
		IEnumerable<ITranslateable> GetVerbEndings (string verbInfinitive, PointOfView pointOfView);
		IEnumerable<ITranslateable> GetTimeframeExpressions ();
		Verb TranslateVerbFromSpanishToEnglish(DataLoader loader, Verb verb, Conjugation conjugation);
		IEnumerable<string> GetReflexiveVerbs (DataLoader dataLoader);
		IEnumerable<RoleSelection> GetAllRoleScenariosForVerbAndFrame (Random random, Verb verb, bool limitVariations, DataLoader dataLoader, Frame frame);
		IEnumerable<VerbRoleSelector> GetAllVerbRoleSelectors();
	}
	
}
