using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{

	public class CannedDataBuilder : ICannedData {

		Dictionary<string, List<ITranslateable>> AllVerbEndings = new Dictionary<string, List<ITranslateable>>(StringComparer.InvariantCultureIgnoreCase);
		Dictionary<Conjugation, List<ITranslateable>> TimeExpressions = new Dictionary<Conjugation, List<ITranslateable>>();
		Dictionary<string,Func<Conjugation,string>> VerbTranslations = new Dictionary<string, Func<Conjugation,string>> (StringComparer.InvariantCultureIgnoreCase);
		Dictionary<string,Func<Conjugation,string>> ReflexiveVerbTranslations = new Dictionary<string, Func<Conjugation,string>> (StringComparer.InvariantCultureIgnoreCase);

		protected void AddVerbEnding(string verbInfinitive, ITranslateable ending) {

			if (!AllVerbEndings.ContainsKey (verbInfinitive)) {
				AllVerbEndings.Add (verbInfinitive, new List<ITranslateable> ());
			}

			AllVerbEndings [verbInfinitive].Add (ending);
		}

		protected void AddVerbEnding(string verbInfinitive, Noun noun) {

			if (!AllVerbEndings.ContainsKey (verbInfinitive)) {
				AllVerbEndings.Add (verbInfinitive, new List<ITranslateable> ());
			}

			AllVerbEndings [verbInfinitive].Add (noun);
		}

		public IEnumerable<ITranslateable> GetVerbEndings(string verbInfinitive, PointOfView pointOfView) {

			if (!AllVerbEndings.ContainsKey (verbInfinitive)) {
				return new [] { new CannedTranslation("","") };
			}

			return AllVerbEndings [verbInfinitive];
		}

		protected void AddTimeframeExpression(Conjugation conjugation, ITranslateable expression) {

			if (!TimeExpressions.ContainsKey (conjugation)) {
				TimeExpressions [conjugation] = new List<ITranslateable> ();
			}

			TimeExpressions[conjugation].Add (expression);
		}

		public IEnumerable<ITranslateable> GetTimeframeExpressions(Conjugation conjugation) {

			if (!TimeExpressions.ContainsKey (conjugation))
				return new [] { new CannedTranslation("","") };

			return TimeExpressions [conjugation];
		}

		public void HasEnglishTranslation(string spanishInfinitive, string englishInfinitive) {
			VerbTranslations.Add (spanishInfinitive, p => englishInfinitive);
		}

		public void HasEnglishTranslation(string spanishInfinitive, Func<Conjugation,string> infinitiveSelector) {
			VerbTranslations.Add (spanishInfinitive, infinitiveSelector);
		}

		public void ReflexiveHasEnglishTranslation(string spanishInfinitive, string englishInfinitive) {
			ReflexiveVerbTranslations.Add (spanishInfinitive, p => englishInfinitive);
		}

		public IEnumerable<string> GetReflexiveVerbs (DataLoader dataLoader) {
			return ReflexiveVerbTranslations.Select (t => t.Key);
		}

		public Verb TranslateVerbFromSpanishToEnglish(DataLoader loader, Verb verb, Conjugation conjugation) {

			if (verb.Infinitive.EndsWith ("se")) {
				var baseVerb = verb.Infinitive.Substring (0, verb.Infinitive.Length - 2);

				if (ReflexiveVerbTranslations.ContainsKey (baseVerb)) {

					var englishInfinitive = ReflexiveVerbTranslations [baseVerb] (conjugation);

					return loader.GetAllSavedEnglishVerbs ().Single (v => v.Infinitive == englishInfinitive);
				}
			}

			if (VerbTranslations.ContainsKey (verb.Infinitive)) {

				var englishInfinitive = VerbTranslations [verb.Infinitive](conjugation);

				return loader.GetAllSavedEnglishVerbs ().Single (v =>  v.Infinitive == englishInfinitive);
			}

			return null;

		}


		public IEnumerable<RoleSelection> GetAllRoleScenariosForVerbAndFrame (Random random, Verb verb, bool limitVariations, DataLoader dataLoader, Frame frame)
		{
			var rootRoleSelection = new RoleSelection (frame);
			rootRoleSelection = rootRoleSelection.WithRole ("verbPhrase", verb.GetTranslateable (frame.Conjugation, this, dataLoader));
			IEnumerable<RoleSelection> roleSelections = new[] {
				rootRoleSelection
			};
			roleSelections = roleSelections.SelectMany (selection =>  {
				return Pronouns.GetSubjectNouns ().Where (n => n.AllowsFraming (frame)).Select (n => selection.WithRole ("subject", n));
			});
			roleSelections = roleSelections.SelectMany (selection =>  {
				var verbEndings = this.GetVerbEndings (verb.Infinitive, frame.PointOfView).ToArray ();
				if (limitVariations) {
					verbEndings = new[] {
						verbEndings [random.Next (verbEndings.Length)]
					};
				}
				return verbEndings.Select (ve => selection.WithRole ("verbEnding", ve));
			});
			roleSelections = roleSelections.SelectMany (selection =>  {
				var timeframes = this.GetTimeframeExpressions (frame.Conjugation).ToArray ();
				if (limitVariations) {
					timeframes = new[] {
						timeframes [random.Next (timeframes.Length)]
					};
				}
				return timeframes.Select (tf => selection.WithRole ("timeframe", tf));
			});
			return roleSelections;
		}
	}
	
}
