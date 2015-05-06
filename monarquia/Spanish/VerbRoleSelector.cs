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

		public IEnumerable<RoleSelection> GetSelectionsFor(Frame frame, bool limitVariations, Random random)
		{
			var seedRoleSelection = new RoleSelection (frame);
			seedRoleSelection = seedRoleSelection.WithRole("verbPhrase", spanishVerb.Conjugation(frame.Conjugation,englishVerb));

			List<RoleSelection> results = new List<RoleSelection> ();
			results.Add (seedRoleSelection);

			foreach (var key in roleOptions.Keys) {

				List<RoleSelection> newResults = new List<RoleSelection> ();

				foreach (var existingResult in results) {

					var options = roleOptions [key].Where (o => o.AllowsFraming (frame)).ToArray();

					if (!options.Any ())
						continue;

					if (limitVariations) {
						options = new [] { options [random.Next (options.Length)] };
					}

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
