using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace monarquia
{
	public class EspanolGenerator : ExerciseGenerator
	{
		CannedData cannedData;

		public EspanolGenerator (CannedData cannedData, string dataDirectory)
			: base(dataDirectory)
		{
			this.cannedData = cannedData;
		}

		public IEnumerable<Exercise> GetAll(){

			List<Exercise> results = new List<Exercise>();
			foreach(var verb in allVerbs) {
				results.AddRange(GetForVerb(verb, false));
			}

			return results;
		}

		public IEnumerable<Exercise> GetForVerb (Verb verb, bool limitVariations)
		{
			List<Exercise> results = new List<Exercise>();

			var selectedPointsOfView = Enum.GetValues (typeof(PointOfView)).Cast<PointOfView> ().ToList();
				
			if (limitVariations) {
				selectedPointsOfView = ChoosePointOfViewsForDrill ();
			}

			foreach (PointOfView pointOfView in selectedPointsOfView) {
				results.AddRange (GetAllConjugationsForVerb (verb, limitVariations, () => pointOfView));
			}

			return results;
		}

		List<Exercise> GetForVerbConjugation (
			Verb verb,
			bool limitVariations, 
			PointOfView pointOfView,
			CannedData cannedData,
			Verb.Conjugation conjugation)
		{
			Exercise resultTemplate = new Exercise ();

			resultTemplate.ExtraInfo = verb.Infinitive;

			if (pointOfView.IsSecondPerson ())
				resultTemplate.HintsForTranslated.Add (pointOfView.AsSubjectPronoun());
			
			resultTemplate.Tags.Add ("conjugation:" + conjugation);
			resultTemplate.Tags.Add ("verb:" + verb.Infinitive);

			List<Exercise> results = new List<Exercise> ();

			var selectedVerbEndings = cannedData.GetVerbEndings (verb.Infinitive, pointOfView).ToArray();
			if (limitVariations) {
				selectedVerbEndings = new [] {
					selectedVerbEndings [random.Next (selectedVerbEndings.Length)]
				};
			}

			var selectedTimeframes = cannedData.GetTimeframeExpressions(conjugation).ToArray();

			if (limitVariations) {
				selectedTimeframes = new [] {
					selectedTimeframes[random.Next(selectedTimeframes.Length)]
				};
			}


			foreach (var scenario in 
				from verbEnding in selectedVerbEndings
				from tf in selectedTimeframes
				select new { verbEnding, timeframe = tf }) 
			{
				//  some accumulated words may be empty strings
				List<string> accumulatedWords = new List<string>();

				accumulatedWords.Add (scenario.timeframe.AsSpanish(pointOfView));

				accumulatedWords.Add(pointOfView.AsSubjectPronoun());
				accumulatedWords.Add(verb.ConjugatedForTense (conjugation, pointOfView));

				accumulatedWords.Add(scenario.verbEnding.AsSpanish(pointOfView));

				var englishVerb = cannedData.TranslateVerbFromSpanishToEnglish (dataLoader, verb);

				var result = resultTemplate.Clone ();
				result.Original = MakeSentenceFromWords (accumulatedWords);

				if (englishVerb != null && 
					!(scenario.verbEnding is TranslationNotImplemented) &&
					!(scenario.timeframe is TranslationNotImplemented)) {

					List<string> accumulatedTranslation = new List<string> ();

					accumulatedTranslation.Add (scenario.timeframe.AsEnglish (pointOfView));
					accumulatedTranslation.Add (pointOfView.AsEnglishSubjectPronoun ());
					accumulatedTranslation.Add (englishVerb.ConjugatedForTense (conjugation, pointOfView));
					accumulatedTranslation.Add (scenario.verbEnding.AsEnglish(pointOfView));

					result.Translated = MakeSentenceFromWords (accumulatedTranslation);
				}

				results.Add (result);
			}

			return results;
		}

		static string MakeSentenceFromWords (List<string> accumulatedWords)
		{
			var nonemptyWordsJoinedBySpaces = string.Join (" ", accumulatedWords.Where (w => !string.IsNullOrEmpty (w)));
			var capitolizedSentencewithPeriod = nonemptyWordsJoinedBySpaces.First ().ToString ().ToUpper () + nonemptyWordsJoinedBySpaces.Substring (1) + ".";
			return capitolizedSentencewithPeriod;
		}

		List<Exercise> GetAllConjugationsForVerb (Verb verb, bool limitVariations, Func<PointOfView> pointOfViewSelector)
		{
			List<Exercise> results = new List<Exercise> ();

			results.AddRange (GetForVerbConjugation (verb, limitVariations, pointOfViewSelector (), cannedData, Verb.Conjugation.Present));
			results.AddRange (GetForVerbConjugation (verb, limitVariations, pointOfViewSelector (), cannedData, Verb.Conjugation.PastPreterite));
			results.AddRange (GetForVerbConjugation (verb, limitVariations, pointOfViewSelector (), cannedData, Verb.Conjugation.PastImperfect));
			results.AddRange (GetForVerbConjugation (verb, limitVariations, pointOfViewSelector (), cannedData, Verb.Conjugation.Future));
			results.AddRange (GetForVerbConjugation (verb, limitVariations, pointOfViewSelector (), cannedData, Verb.Conjugation.Conditional));
			results.AddRange (GetForVerbConjugation (verb, limitVariations, pointOfViewSelector (), cannedData, Verb.Conjugation.PresentPerfect));

			return results;
		}
	}
}

