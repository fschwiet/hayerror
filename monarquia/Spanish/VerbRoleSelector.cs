using System;
using System.Collections.Generic;
using System.Linq;
using monarquia;

namespace monarquia
{
	public class VerbRoleSelector
	{
		ICannedData cannedData;
		DataLoader dataLoader;
		Dictionary<string, IEnumerable<ITranslateable>> roleOptions = new Dictionary<string, IEnumerable<ITranslateable>>();
	
		public VerbRoleSelector(ICannedData cannedData, DataLoader dataLoader) 
		{
			this.cannedData = cannedData;
			this.dataLoader = dataLoader;
		}

		public VerbRoleSelector hasOneOf(string roleName, IEnumerable<ITranslateable> values)
		{
			roleOptions [roleName] = values;
			return this;
		}

		public VerbRoleSelector hasTranslation(string spanishInfinitive, string englishInfinitive)
		{
			return hasTranslation (spanishInfinitive, c => englishInfinitive);
		}

		public VerbRoleSelector hasTranslation(
			string spanishInfinitive, 
			Func<Conjugation, string> englishInfinitiveSelector) {

			var matchingSpanishVerbs = dataLoader.GetAllSavedSpanishVerbs ().Where (v => v.Infinitive == spanishInfinitive);

			if (!matchingSpanishVerbs.Any ())
				throw new Exception ("No scraped data for spanish verb: " + spanishInfinitive);

			var spanishVerb = matchingSpanishVerbs.Single();
			Dictionary<string, VerbConjugator> englishVerbCached = new Dictionary<string, VerbConjugator>();

			List<ITranslateable> options = new List<ITranslateable> ();

			foreach (var conjugation in Enum.GetValues(typeof(Conjugation)).Cast<Conjugation>()) {

				var englishInfinitive = englishInfinitiveSelector(conjugation);

				if (!englishVerbCached.ContainsKey(englishInfinitive)) {

					var matchingEnglishWords = dataLoader.GetAllSavedEnglishVerbs ().Where (v => v.Infinitive == englishInfinitive);

					if (!matchingEnglishWords.Any ())
						throw new Exception ("No scraped data for english verb: " + englishInfinitive);

					englishVerbCached[englishInfinitive] = matchingEnglishWords.Single();
				}

				var englishVerb = englishVerbCached[englishInfinitive];

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
