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

					var availableRoleSelections = roleSelector.VerbRoleSelector.GetSelectionsFor (frame);

					results.AddRange(BuildExercisesFromRoles (
						availableRoleSelections, 
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

			List<VerbConjugator> verbs = new List<VerbConjugator> ();

			verbs.AddRange (dataLoader.GetAllSavedSpanishVerbs ());
			verbs.AddRange (cannedData.GetReflexiveVerbs (dataLoader).Select (inf => new ReflexiveVerbConjugator (inf, dataLoader)));

			if (verb != null)
				verbs = verbs.Where (v => v.Infinitive == verb).ToList();

			foreach(var v in verbs.Where(v => !verbsToConsiderFinished.Contains(v.Infinitive))) 
			{
				var frames = Frame.SelectAllFrames ();

				foreach (var framing in frames) {

					var roleSelecton = cannedData.GetAllRoleScenariosForVerbAndFrame (random, v, dataLoader, framing);

					results.AddRange (BuildExercisesFromRoles (roleSelecton, 
						new [] { "timeframe", "subject", "spanishonlyNoPreposition","verbPhrase", "verbEnding"},
						new [] { "timeframe", "subject","verbPhrase", "verbEnding"},
						framing));
				}
			}

			if (verb != null) {
				results = results.Where (r => r.Tags.Contains ("verb:" + verb)).ToList ();
			}

			if (limitVariations) {
				
				var allTags = results.SelectMany (r => r.Tags).Distinct ().AsEnumerable ();
				var allUsageTags = allTags.Where (t => t.StartsWith ("usage:")).AsEnumerable ();

				List<Exercise> limitedExercises = new List<Exercise> ();

				foreach (var resultsGroup in
					from u in allUsageTags
					select results.Where(r => r.Tags.Contains(u)).ToArray()) {

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
				result.Original = MakeSpanishSentence (spanishResultChunks);
				result.Tags = spanishResultChunks.SelectMany (p => p.Tags).Distinct().ToList ();
				result.ExtraInfo = string.Join (", ", spanishResultChunks.SelectMany(s => s.ExtraInfo));

				try {
					var englishPhrase = englishTemplate.Select (t => roleSelection.GetForRole (t));
					var englishChunks = englishPhrase.SelectMany(e => e.GetResult(frame));

					result.Translated = MakeEnglishSentence (phoneticData, englishChunks);					
				}
				catch(Exception) {
					// ignore
				}

				results.Add (result);
			}

			return results;
		}

		static string MakeSpanishSentence (IEnumerable<ResultChunk> chunks)
		{
			var accumulator = new HintedLanguageAccumulator();

			foreach (var chunk in chunks)
				accumulator.Add (chunk.SpanishTranslation, chunk.SpanishHint);

			return accumulator.GetResult ();
		}

		public static string MakeEnglishSentence (CachedPhoneticData phoneticData, IEnumerable<ResultChunk> chunks)
		{
			var accumulator = new HintedLanguageAccumulator();

			foreach (var chunk in chunks)
				accumulator.Add (chunk.EnglishTranslation, chunk.EnglishHint);

			accumulator.ApplyTransform ("a", w => "aɔæɪɛ".Contains (phoneticData.GetEnglishPhonetics (w) [0]), "an");

			return accumulator.GetResult ();
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

