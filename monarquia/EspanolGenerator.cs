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

			var pointOfViewGenerators = Enum.GetValues (typeof(PointOfView)).Cast<PointOfView> ()
				.Select<PointOfView,Func<PointOfView>>(pov => delegate() { return pov; }).ToList();
				
			if (limitVariations) {
				pointOfViewGenerators = ChoosePointOfViewsForDrill ();
			}

			foreach (var pointOfView in pointOfViewGenerators) {
				results.AddRange (GetAllConjugationsForVerb (verb, limitVariations, pointOfView));
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

			var subject = pointOfView.GetSubjectNoun ();

			resultTemplate.ExtraInfo = verb.Infinitive + " (" + conjugation.AsFriendlyString() + " tense)";

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
				List<ITranslateable> spanishPhrase = new List<ITranslateable> ();
				spanishPhrase.Add (scenario.timeframe);
				spanishPhrase.Add (subject);
				spanishPhrase.Add (verb.ForConjugation (conjugation));
				spanishPhrase.Add (scenario.verbEnding);

				//  some accumulated words may be empty strings
				IEnumerable<string> accumulatedWords = spanishPhrase.Select(p => p.AsSpanish(pointOfView));

				var result = resultTemplate.Clone ();
				result.Original = MakeSentenceFromWords (accumulatedWords);

				result.HintsForTranslated = spanishPhrase.SelectMany (p => p.GetEnglishHints ()).ToList();

				var englishVerb = cannedData.TranslateVerbFromSpanishToEnglish (dataLoader, verb);

				if (englishVerb != null) {

					try {
						List<string> accumulatedTranslation = new List<string> ();

						accumulatedTranslation.Add (scenario.timeframe.AsEnglish (pointOfView));
						accumulatedTranslation.Add (subject.AsEnglish(pointOfView));
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

		public List<Func<PointOfView>> ChoosePointOfViewsForDrill ()
		{
			List<Func<PointOfView>> results = new List<Func<PointOfView>> ();

			results.Add (() => PointOfView.FirstPerson);
			results.Add (() => PointOfView.SecondPerson);

			results.Add (() => new [] {
				PointOfView.SecondPersonFormal,
				PointOfView.ThirdPersonMasculine,
				PointOfView.ThirdPersonFeminine
			} [random.Next (3)]);

			results.Add (() => PointOfView.FirstPersonPlural);


			results.Add (() => new [] {
				PointOfView.SecondPersonPluralFormal,
				PointOfView.ThirdPersonPluralMasculine,
				PointOfView.ThirdPersonPluralFeminine
			} [random.Next (3)]);

			return results;
		}
	}
}

