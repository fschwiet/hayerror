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
			var spanishVerb = dataLoader.GetAllSavedSpanishVerbs ().Single (v => v.Infinitive == this.infinitive);
			var englishVerb = dataLoader.GetAllSavedEnglishVerbs ().Single (v => v.Infinitive == english);

			List<ITranslateable> options = new List<ITranslateable> ();

			foreach (var conjugation in Enum.GetValues(typeof(Conjugation)).Cast<Conjugation>()) {
				options.Add (spanishVerb.Conjugation (conjugation, englishVerb));
			}

			roleOptions ["verbPhrase"] = options;

			return this;
		}

		public IEnumerable<RoleSelection> GetSelectionsFor(Frame frame)
		{
			var seedRoleSelection = new RoleSelection (frame);

			List<RoleSelection> results = new List<RoleSelection> ();
			results.Add (seedRoleSelection);

			foreach (var key in roleOptions.Keys) {

				List<RoleSelection> newResults = new List<RoleSelection> ();

				foreach (var existingResult in results) {

					var options = roleOptions [key].Where (o => o.AllowsFraming (frame)).ToArray();

					if (!options.Any ())
						continue;

					foreach (var option in options) {
						
						newResults.Add (existingResult.WithRole (key, option));
					}
				}
			
				results = newResults;
			}

			return results;
		}
	}
	
}
