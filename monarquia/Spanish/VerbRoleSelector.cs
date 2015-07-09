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
        Dictionary<Role, List<RoleSelection>> roleOptions = new Dictionary<Role, List<RoleSelection>>();
        List<Func<RoleSelections, RoleSelections>> transforms = new List<Func<RoleSelections, RoleSelections>>();
        bool needsDebugging;

		public VerbRoleSelector(ICannedData cannedData, DataLoader dataLoader) 
		{
			this.cannedData = cannedData;
			this.dataLoader = dataLoader;

			this.hasOneOf (Role.spanishonlyNoPreposition, new ITranslateable[] {
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

        VerbRoleSelector hasSelections(Role roleName, IEnumerable<RoleSelection> values)
		{
            if (!roleOptions.ContainsKey(roleName))
                roleOptions[roleName] = new List<RoleSelection>();

			roleOptions [roleName].AddRange(values);

			return this;
		}

        public VerbRoleSelector hasOneOf<T>(Role roleName, 
            IEnumerable<T> values,
            Func<T, ITranslateable> defaultToTranslateable = null,
            Func<T, ITranslateable> spanishToTranslateable = null,
            Func<T, ITranslateable> englishToTranslateable = null,
            IEnumerable<Func<T, ITranslateable>> additionalToTranslateables = null,
            Func<ITranslateable, ITranslateable> sharedDecorator = null,
            Func<ITranslateable, ITranslateable> spanishDecorator = null,
            Func<ITranslateable, ITranslateable> englishDecorator = null)
        {
            List<RoleSelection> variations = new List<RoleSelection>();

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
                    variations.Add(new RoleSelection(
                        spanishDecorator(translateable).WithEnglishAlternative(englishDecorator(translateable)), 
                        value as Noun));
                }
            }

            return hasSelections(roleName, variations);
        }

        public VerbRoleSelector hasTranslation(string spanishInfinitive, string englishInfinitive)
        {
            return hasTranslation(spanishInfinitive, englishInfinitive, f => true);
        }

        public VerbRoleSelector hasTransform(Func<RoleSelections, RoleSelections> transform)
        {
            transforms.Add(transform);
            return this;
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

            var translateable = new VerbInstance(spanishVerb, englishVerb, framing);

            var selection = new RoleSelection(translateable);

            hasSelections(Role.verbPhrase, new [] { selection });

			return this;
		}

		public IEnumerable<RoleSelections> GetSelectionsFor(Conjugation conjugation, bool isNegated)
		{
			var seedRoleSelection = new RoleSelections ();

			List<RoleSelections> results = new List<RoleSelections> ();
			results.Add (seedRoleSelection);

            {
                List<RoleSelections> newResults = new List<RoleSelections>();

                foreach (var existingResult in results)
                {
                    var options = roleOptions[Role.subject].ToArray();

                    foreach (var option in options)
                    {
                        var newResult = existingResult.WithRole(Role.subject, option);
                        newResults.Add(newResult);
                    }
                }

                results = newResults;
            }

			foreach (var key in roleOptions.Keys.Where(k => k != Role.subject)) {

				List<RoleSelections> newResults = new List<RoleSelections> ();

				foreach (var existingResult in results) {

                    var frame = new Frame(existingResult.GetForRole(Role.subject).UnderlyingObject.GetPointOfView(), conjugation, isNegated);

					var options = roleOptions [key].Where (o => o.Value.AllowsFraming (frame)).ToArray();

                    //  only put in an empty default if there were no options,
                    //  if they're all incompatible with the frame then we don't
                    //  want to force this frame.
                    if (!roleOptions[key].Any())
                    {
                        options = new RoleSelection[] { new RoleSelection(new EmptyTranslateable()) };
                    }

					foreach (var option in options) {

                        newResults.Add(existingResult.WithRole(key, option));
					}
				}
			
				results = newResults;
			}

            foreach(var transform in transforms)
            {
                results = results.Select(r => transform(r)).ToList();
            }

			return results;
		}
	}
	
}
