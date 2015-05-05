using System;
using System.Collections.Generic;
using System.Linq;
using monarquia;

namespace monarquia
{
	public class VerbRoleSelector
	{
		string infinitive;
		Dictionary<string, IEnumerable<ITranslateable>> roleOptions = new Dictionary<string, IEnumerable<ITranslateable>>();
		Verb spanishVerb;
		Verb englishVerb;

		public VerbRoleSelector(string infinitive) 
		{
			this.infinitive = infinitive;
		}

		public VerbRoleSelector hasOneOf(string roleName, IEnumerable<ITranslateable> values)
		{
			roleOptions [roleName] = values;
			return this;
		}

		public VerbRoleSelector hasTranslation(string english, ICannedData cannedData, DataLoader dataLoader)
		{
			spanishVerb = dataLoader.GetAllSavedSpanishVerbs ().Single (v => v.Infinitive == this.infinitive);
			englishVerb = dataLoader.GetAllSavedEnglishVerbs ().Single (v => v.Infinitive == english);

			return this;
		}

		public IEnumerable<RoleSelection> GetSelectionsFor(Frame frame)
		{
			var seedRoleSelection = new RoleSelection (frame);
			seedRoleSelection = seedRoleSelection.WithRole("verbPhrase", spanishVerb.Conjugation(frame.Conjugation,englishVerb));

			List<RoleSelection> results = new List<RoleSelection> ();
			results.Add (seedRoleSelection);

			foreach (var key in roleOptions.Keys) {

				List<RoleSelection> newResults = new List<RoleSelection> ();

				foreach (var existingResult in results) {
					foreach (var option in roleOptions[key]) {

						if (option.AllowsFraming (frame)) {
							newResults.Add (existingResult.WithRole (key, option));
						}
					}
				}
			
				results = newResults;
			}

			return results;
		}
	}
	
}
