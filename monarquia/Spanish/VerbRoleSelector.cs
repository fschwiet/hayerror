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
		Dictionary<string, List<ITranslateable>> roleOptions = new Dictionary<string, List<ITranslateable>>();
	
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
            if (!roleOptions.ContainsKey(roleName))
                roleOptions[roleName] = new List<ITranslateable>();

			roleOptions [roleName].AddRange(values);

			return this;
		}

        public VerbRoleSelector hasOneOf<T>(string roleName, Func<T, IEnumerable<ITranslateable>> transform, IEnumerable<T> values)
        {
            return hasOneOf<T>(roleName, transform, transform, values);
        }

        public VerbRoleSelector hasOneOf<T>(string roleName, Func<T, IEnumerable<ITranslateable>> spanishTransform, Func<T, IEnumerable<ITranslateable>> englishTransform, IEnumerable<T> values)
        {
            List<ITranslateable> transformedValues = new List<ITranslateable>();

            foreach (var value in values)
            {
                transformedValues.Add(new Composed(spanishTransform(value), englishTransform(value)));
            }

            return hasOneOf(roleName, transformedValues);
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

            hasOneOf("verbPhrase", new[] { new VerbInstance(spanishVerb, englishVerb, framing) });

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
