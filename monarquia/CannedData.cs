using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{
	public class CannedData {

		Dictionary<string, List<ITranslateable>> AllVerbEndings = new Dictionary<string, List<ITranslateable>>(StringComparer.InvariantCultureIgnoreCase);
		Dictionary<Conjugation, List<ITranslateable>> TimeExpressions = new Dictionary<Conjugation, List<ITranslateable>>();
		Dictionary<string,Func<Conjugation,string>> VerbTranslations = new Dictionary<string, Func<Conjugation,string>> (StringComparer.InvariantCultureIgnoreCase);

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

		public Verb TranslateVerbFromSpanishToEnglish(DataLoader loader, Verb verb, Conjugation conjugation) {

			if (!VerbTranslations.ContainsKey (verb.Infinitive))
				return null;

			var englishInfinitive = VerbTranslations [verb.Infinitive](conjugation);

			return loader.GetAllEnglishVerbs ().Single (v =>  v.Infinitive == englishInfinitive);
		}
	}
	
}
