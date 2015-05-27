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

			this.hasOneOf ("spanishonlyNoPreposition", new ITranslateable[] {
				new CannedTranslation("no", "", frame => frame.IsNegated),
				new CannedTranslation("", "", frame => !frame.IsNegated)
			});
		}

		public VerbRoleSelector hasOneOf(string roleName, IEnumerable<ITranslateable> values)
		{
			roleOptions [roleName] = values;
			return this;
		}

		public VerbRoleSelector hasTranslation(string spanishInfinitive, string englishInfinitive)
		{
			return hasTranslation (spanishInfinitive, englishInfinitive, f => true);
		}

		public VerbRoleSelector hasTranslation(
			string spanishInfinitive,
			string englishInfinitive,
			Func<Frame, bool> framing) {

			var spanishVerb = dataLoader.GetAllSavedSpanishVerbs ()
				.Where (v => v.Infinitive == spanishInfinitive).SingleOrDefault();

			if (spanishVerb == null)
				throw new Exception ("No scraped data for spanish verb: " + spanishInfinitive);

			var englishVerb = dataLoader.GetAllSavedEnglishVerbs ()
				.Where (v => v.Infinitive == englishInfinitive).SingleOrDefault ();

			if (englishVerb == null)
				throw new Exception ("No scraped data for english verb: " + englishInfinitive);

			List<ITranslateable> options = new List<ITranslateable> ();
			if (roleOptions.ContainsKey ("verbPhrase"))
				options.AddRange (roleOptions ["verbPhrase"]);

			options.Add (new VerbInstance (spanishVerb, englishVerb, framing));

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
