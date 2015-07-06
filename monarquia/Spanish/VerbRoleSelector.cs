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
        bool needsDebugging;

		public VerbRoleSelector(ICannedData cannedData, DataLoader dataLoader) 
		{
			this.cannedData = cannedData;
			this.dataLoader = dataLoader;

			this.hasOneOf ("spanishonlyNoPreposition", new ITranslateable[] {
				new CannedTranslation("no", "", frame => frame.IsNegated),
				new CannedTranslation("", "", frame => !frame.IsNegated)
			});
		}

        public VerbRoleSelector MarkNeedsDebugging()
        {
            needsDebugging = true;
            return this;
        }

        public VerbRoleSelector CheckNeedsDebugging()
        {
            if (needsDebugging)
            {
                int i = 0;
            }

            return this;
        }

		public VerbRoleSelector hasOneOf(string roleName, IEnumerable<ITranslateable> values)
		{
            if (!roleOptions.ContainsKey(roleName))
                roleOptions[roleName] = new List<ITranslateable>();

			roleOptions [roleName].AddRange(values);

			return this;
		}

        public VerbRoleSelector hasOneOf<T>(string roleName, 
            IEnumerable<T> values,
            Func<T, ITranslateable> defaultToTranslateable = null,
            Func<T, ITranslateable> spanishToTranslateable = null,
            Func<T, ITranslateable> englishToTranslateable = null,
            IEnumerable<Func<T, ITranslateable>> additionalToTranslateables = null,
            Func<ITranslateable, ITranslateable> sharedDecorator = null,
            Func<ITranslateable, ITranslateable> spanishDecorator = null,
            Func<ITranslateable, ITranslateable> englishDecorator = null)
        {
            List<ITranslateable> variations = new List<ITranslateable>();

            List<Func<T, ITranslateable>> allToTranslateables = new List<Func<T, ITranslateable>>();

            spanishToTranslateable = spanishToTranslateable ?? defaultToTranslateable;
            englishToTranslateable = englishToTranslateable ?? defaultToTranslateable;

            if (spanishToTranslateable != null && englishToTranslateable != null)
            {
                allToTranslateables.Add(n => spanishToTranslateable(n).WithEnglishAlternative(englishToTranslateable(n)));
            }
            else if (spanishToTranslateable != null || englishToTranslateable != null)
            {
                throw new Exception("Invalid call to hasOneOf<T>");
            }

            if (additionalToTranslateables != null)
                allToTranslateables.AddRange(additionalToTranslateables);
            
            if (!allToTranslateables.Any())
            {
                allToTranslateables.Add(t => t as ITranslateable );
            }

            sharedDecorator = sharedDecorator ?? delegate(ITranslateable v) { return v; };
            spanishDecorator = spanishDecorator ?? sharedDecorator;
            englishDecorator = englishDecorator ?? sharedDecorator;

            foreach (var value in values)
            {
                foreach (var translateable in allToTranslateables.Select(f => f(value)))
                {
                    variations.Add(
                        spanishDecorator(translateable).
                        WithEnglishAlternative(englishDecorator(translateable)));
                }
            }

            return hasOneOf(roleName, variations);
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
			var seedRoleSelection = new RoleSelection ();

			List<RoleSelection> results = new List<RoleSelection> ();
			results.Add (seedRoleSelection);

			foreach (var key in roleOptions.Keys) {

				List<RoleSelection> newResults = new List<RoleSelection> ();

				foreach (var existingResult in results) {

					var options = roleOptions [key].Where (o => o.AllowsFraming (frame)).ToArray();

                    //  only put in an empty default if there were no options,
                    //  if they're all incompatible with the frame then we don't
                    //  want to force this frame.
                    if (!roleOptions[key].Any())
                    {
                        options = new ITranslateable[] { new EmptyTranslateable() };
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
