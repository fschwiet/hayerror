using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace monarquia
{
	public class EspanolGenerator : ExerciseGenerator
	{
		CannedData cannedData;
		CachedPhoneticData phoneticData;

		public EspanolGenerator (CannedData cannedData, string dataDirectory)
			: base(dataDirectory)
		{
			this.phoneticData = new CachedPhoneticData(dataDirectory);
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
			Conjugation conjugation)
		{
			Exercise resultTemplate = new Exercise ();

			resultTemplate.ExtraInfo = verb.Infinitive + " (" + conjugation + ")";

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

				if (englishVerb != null) {

					try {
						List<string> accumulatedTranslation = new List<string> ();

						accumulatedTranslation.Add (scenario.timeframe.AsEnglish (pointOfView));
						accumulatedTranslation.Add (pointOfView.AsEnglishSubjectPronoun ());
						accumulatedTranslation.Add (englishVerb.ConjugatedForTense (conjugation, pointOfView));
						accumulatedTranslation.Add (scenario.verbEnding.AsEnglish(pointOfView));

						result.Translated = MakeEnglishSentenceFromWords (phoneticData, accumulatedTranslation);					
					}
					catch(TranslationNotImplemented.TranslatedNotImplementedException) {
						// ignore
					}
				}

				results.Add (result);
			}

			return results;
		}

		static string MakeSentenceFromWords (IEnumerable<string> input, Func<IEnumerable<string>,IEnumerable<string>> transform = null)
		{
			if (transform != null) {
				input = transform (input.SelectMany(w => w.Split(' ')).Where (w => !string.IsNullOrEmpty (w))).ToList ();
			}

			var nonemptyWordsJoinedBySpaces = string.Join (" ", input.Where (w => !string.IsNullOrEmpty (w)));
			var capitolizedSentencewithPeriod = nonemptyWordsJoinedBySpaces.First ().ToString ().ToUpper () + nonemptyWordsJoinedBySpaces.Substring (1) + ".";
			return capitolizedSentencewithPeriod;
		}

		public static string MakeEnglishSentenceFromWords (CachedPhoneticData phoneticData, IEnumerable<string> accumulatedTranslation)
		{
			return MakeSentenceFromWords (accumulatedTranslation, input =>  {
				var words = input.ToArray ();
				for (var i = 0; i < words.Length - 1; i++) {
					if (words [i].Equals ("a", StringComparison.InvariantCultureIgnoreCase)) {
						var nextWordPhonemes = phoneticData.GetEnglishPhonetics(words[i+1]);

						if ("aɔæɪɛ".Contains(nextWordPhonemes[0])) {
							words[i] = "an";
						}
					}
				}
				return words;
			});
		}

		List<Exercise> GetAllConjugationsForVerb (Verb verb, bool limitVariations, Func<PointOfView> pointOfViewSelector)
		{
			List<Exercise> results = new List<Exercise> ();

			results.AddRange (GetForVerbConjugation (verb, limitVariations, pointOfViewSelector (), cannedData, Conjugation.Present));
			results.AddRange (GetForVerbConjugation (verb, limitVariations, pointOfViewSelector (), cannedData, Conjugation.PastPreterite));
			results.AddRange (GetForVerbConjugation (verb, limitVariations, pointOfViewSelector (), cannedData, Conjugation.PastImperfect));
			results.AddRange (GetForVerbConjugation (verb, limitVariations, pointOfViewSelector (), cannedData, Conjugation.Future));
			results.AddRange (GetForVerbConjugation (verb, limitVariations, pointOfViewSelector (), cannedData, Conjugation.Conditional));
			results.AddRange (GetForVerbConjugation (verb, limitVariations, pointOfViewSelector (), cannedData, Conjugation.PresentPerfect));

			return results;
		}
	}
}

