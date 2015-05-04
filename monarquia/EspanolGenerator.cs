using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace monarquia
{
	public class EspanolGenerator : ExerciseGenerator
	{
		ICannedData cannedData;
		CachedPhoneticData phoneticData;

		public EspanolGenerator (ICannedData cannedData, string dataDirectory)
			: base(dataDirectory)
		{
			this.phoneticData = new CachedPhoneticData(dataDirectory);
			this.cannedData = cannedData;
		}

		public IEnumerable<Exercise> GetExercises(string verb = null, bool limitVariations = false)
		{
			List<Verb> verbs = new List<Verb> ();

			verbs.AddRange (dataLoader.GetAllSavedSpanishVerbs ());
			verbs.AddRange (cannedData.GetReflexiveVerbs (dataLoader).Select (inf => new ReflexiveVerb (inf, dataLoader)));

			if (verb != null)
				verbs = verbs.Where (v => v.Infinitive == verb).ToList();

			var frames = limitVariations ? 
				Frame.FramesCoveringEachConjugationForm () :
				Frame.SelectAllFrames ();

			List<Exercise> results = new List<Exercise> ();

			foreach(var v in verbs) 
			{
				foreach (var framing in frames) {
					results.AddRange (GetForVerbConjugation (v, limitVariations, cannedData, framing));
				}
			}

			return results;
		}

		List<Exercise> GetForVerbConjugation (
			Verb verb,
			bool limitVariations, 
			ICannedData cannedData,
			Frame frame)
		{
			var roleSelections = cannedData.GetAllRoleScenariosForVerbAndFrame (random, verb, limitVariations, dataLoader, frame);

			List<Exercise> results = new List<Exercise> ();

			foreach (var roleSelection in roleSelections)
			{
				List<ITranslateable> spanishPhrase = new List<ITranslateable> ();
				spanishPhrase.Add (roleSelection.GetForRole("timeframe"));
				spanishPhrase.Add (roleSelection.GetForRole("subject"));
				spanishPhrase.Add (roleSelection.GetForRole("verbPhrase"));
				spanishPhrase.Add (roleSelection.GetForRole("verbEnding"));

				var result = new Exercise();
				result.Original = MakeSentenceFromWords (spanishPhrase.Select(p => p.AsSpanish(frame.PointOfView)));
				result.HintsForTranslated = spanishPhrase.SelectMany (p => p.GetEnglishHints ()).ToList();
				result.Tags = spanishPhrase.SelectMany (p => p.GetTags()).ToList ();
				result.ExtraInfo = string.Join (", ", spanishPhrase.SelectMany (p => p.GetExtraHints ()));

				try {
					result.Translated = MakeEnglishSentenceFromWords (phoneticData, spanishPhrase.Select(p => p.AsEnglish(frame.PointOfView)));					
				}
				catch(Exception) {
					// ignore
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

		public IEnumerable<PointOfView> ChoosePointOfViewsForDrill ()
		{
			List<PointOfView> results = new List<PointOfView> ();

			results.Add (PointOfView.FirstPerson);
			results.Add (PointOfView.SecondPerson);

			results.Add (new [] {
				PointOfView.SecondPersonFormal,
				PointOfView.ThirdPersonMasculine,
				PointOfView.ThirdPersonFeminine
			} [random.Next (3)]);

			results.Add (PointOfView.FirstPersonPlural);


			results.Add (new [] {
				PointOfView.SecondPersonPluralFormal,
				PointOfView.ThirdPersonPluralMasculine,
				PointOfView.ThirdPersonPluralFeminine
			} [random.Next (3)]);

			return results;
		}
	}
}

