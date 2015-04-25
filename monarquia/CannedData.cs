using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{
	public class CannedData {

		Dictionary<string, List<ITranslateable>> AllVerbEndings = new Dictionary<string, List<ITranslateable>>(StringComparer.InvariantCultureIgnoreCase);
		Dictionary<Verb.Conjugation, List<string>> TimeExpressions = new Dictionary<Verb.Conjugation, List<string>>();
		Dictionary<string,string> VerbTranslations = new Dictionary<string, string> (StringComparer.InvariantCultureIgnoreCase);

		protected void AddVerbEnding(string verbInfinitive, string ending) {
			AddVerbEnding(verbInfinitive, new CannedTranslation(ending, "<not translated>"));
		}

		protected void AddVerbEnding(string verbInfinitive, CannedTranslation ending) {

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

		protected void AddTimeframeExpression(Verb.Conjugation conjugation, params string[] expressions) {

			if (!TimeExpressions.ContainsKey (conjugation)) {
				TimeExpressions [conjugation] = new List<string> ();
			}

			foreach (var expression in expressions) {
				TimeExpressions[conjugation].Add (expression);
			}
		}

		public IEnumerable<string> GetTimeframeExpressions(Verb.Conjugation conjugation) {

			if (!TimeExpressions.ContainsKey (conjugation))
				return new string[] { "" };

			return TimeExpressions [conjugation];
		}

		public void AddVerbTranslation(string spanishInfinitive, string englishInfinitive) {
			VerbTranslations.Add (spanishInfinitive, englishInfinitive);
		}

		public Verb TranslateVerbFromSpanishToEnglish(DataLoader loader, Verb verb) {

			if (!VerbTranslations.ContainsKey (verb.Infinitive))
				return null;

			var englishInfinitive = VerbTranslations [verb.Infinitive];

			return loader.GetAllEnglishVerbs ().Single (v => v.Infinitive == englishInfinitive);
		}
	}
	
}
