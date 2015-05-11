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
			List<Exercise> results = new List<Exercise> ();

			foreach (var roleSelector in cannedData.GetAllVerbRoleSelectors())
			{
				var frames = Frame.SelectAllFrames ();

				foreach (var frame in frames) {
					results.AddRange(BuildExercisesFromRoles (
						roleSelector.VerbRoleSelector.GetSelectionsFor (frame), 
						roleSelector.SpanishRolePattern,
						roleSelector.EnglishRolePattern,
						frame));
				}
			}
				
			var tagPrefix = "verb:";
			var verbsToConsiderFinished = results.SelectMany (r => r.Tags)
				.Where (t => t.StartsWith (tagPrefix))
				.Distinct()
				.Select (t => t.Substring (tagPrefix.Length));

			// Console.WriteLine ("Have good data for verbs: " + string.Join (", ", verbsToConsiderFinished));

			List<Verb> verbs = new List<Verb> ();

			verbs.AddRange (dataLoader.GetAllSavedSpanishVerbs ());
			verbs.AddRange (cannedData.GetReflexiveVerbs (dataLoader).Select (inf => new ReflexiveVerb (inf, dataLoader)));

			if (verb != null)
				verbs = verbs.Where (v => v.Infinitive == verb).ToList();

			foreach(var v in verbs.Where(v => !verbsToConsiderFinished.Contains(v.Infinitive))) 
			{
				var frames = Frame.SelectAllFrames ();

				foreach (var framing in frames) {

					var roleSelecton = cannedData.GetAllRoleScenariosForVerbAndFrame (random, v, dataLoader, framing);
					results.AddRange (BuildExercisesFromRoles (roleSelecton, 
						new [] { "timeframe", "subject","verbPhrase", "verbEnding"},
						new [] { "timeframe", "subject","verbPhrase", "verbEnding"},
						framing));
				}
			}

			if (verb != null) {
				results = results.Where (r => r.Tags.Contains ("verb:" + verb)).ToList ();
			}

			if (limitVariations) {
				var allTags = results.SelectMany (r => r.Tags).Distinct ().AsEnumerable ();
				var allVerbTags = allTags.Where (t => t.StartsWith ("verb:")).AsEnumerable ();
				var allConjugationTags = allTags.Where (t => t.StartsWith ("conjugation:")).AsEnumerable ();
				var allPointOfViewTags = Frame.FramesCoveringEachConjugationForm ().Select (r => r.PointOfView).Distinct ()
					.Select (p => p.AsTagString ()).AsEnumerable ();

				List<Exercise> limitedExercises = new List<Exercise> ();

				foreach (var resultsGroup in
					from v in allVerbTags
					from c in allConjugationTags
					from p in allPointOfViewTags
					select results.Where(r => r.Tags.Contains(v) && r.Tags.Contains(c) && r.Tags.Contains(p)).ToArray()) {

					if (!resultsGroup.Any ())
						continue;

					limitedExercises.Add (resultsGroup [random.Next (resultsGroup.Length)]);
				}

				results = limitedExercises;
			}

			return results;
		}

		List<Exercise> BuildExercisesFromRoles (
			IEnumerable<RoleSelection> roleSelections,
			IEnumerable<string> spanishTemplate,
			IEnumerable<string> englishTemplate,
			Frame frame)
		{
			List<Exercise> results = new List<Exercise> ();

			foreach (var roleSelection in roleSelections)
			{
				var spanishPhrase = spanishTemplate.Select (t => roleSelection.GetForRole (t));
				var spanishResultChunks = spanishPhrase.SelectMany (s => s.GetResult (frame));

				var result = new Exercise();
				result.Original = MakeSentenceFromWords (spanishResultChunks.Select(p => p.SpanishTranslation));
				result.HintsForTranslated = spanishPhrase.SelectMany (p => spanishResultChunks.SelectMany(r => r.EnglishHint)).ToList();
				result.Tags = spanishResultChunks.SelectMany (p => p.Tags).Distinct().ToList ();
				result.ExtraInfo = string.Join (", ", spanishResultChunks.SelectMany(s => s.ExtraInfo));

				try {
					var englishPhrase = englishTemplate.Select (t => roleSelection.GetForRole (t));
					var englishChunks = englishPhrase.SelectMany(e => e.GetResult(frame));

					result.Translated = MakeEnglishSentenceFromWords (phoneticData, englishChunks.Select(e => e.EnglishTranslation));					
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
				input = transform (input.SelectMany(w => w.Split(' ')).Where (w => !string.IsNullOrWhiteSpace (w))).ToList ();
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

