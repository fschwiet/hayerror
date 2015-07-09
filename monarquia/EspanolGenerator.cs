using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace monarquia
{
	public class EspanolGenerator
	{
		protected Random random  = new Random ();
		protected DataLoader dataLoader;
		ICannedData cannedData;
		CachedPhoneticData phoneticData;

		public EspanolGenerator (ICannedData cannedData, string dataDirectory)
		{
			this.dataLoader = new DataLoader (dataDirectory);
			this.phoneticData = new CachedPhoneticData(dataDirectory);
			this.cannedData = cannedData;
		}

		public IEnumerable<Exercise> GetExercises(string verb = null, bool limitVariations = false)
		{
			List<Exercise> results = new List<Exercise> ();

			foreach (var roleSelector in cannedData.GetAllVerbRoleSelectors())
			{
                roleSelector.VerbRoleSelector.CheckNeedsDebugging();

				foreach (var conjugation in Enum.GetValues(typeof(Conjugation)).Cast<Conjugation>())
                foreach (var isNegated in new[] { false, true})
                {
					var availableRoleSelections = roleSelector.VerbRoleSelector.GetSelectionsFor (conjugation, isNegated);
                    
					results.AddRange(BuildExercisesFromRoles (
						availableRoleSelections, 
						roleSelector.SpanishRolePattern,
						roleSelector.EnglishRolePattern,
						conjugation, isNegated));
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
			IEnumerable<RoleSelections> roleSelections,
			IEnumerable<Role> spanishTemplate,
			IEnumerable<Role> englishTemplate,
            Conjugation conjugation,
            bool isNegated)
		{
			List<Exercise> results = new List<Exercise> ();

			foreach (var roleSelection in roleSelections)
			{
                var frame = new Frame(roleSelection.GetForRole(Role.subject).UnderlyingObject.GetPointOfView(), conjugation, isNegated); 

                var spanishPhrase = spanishTemplate.Select(t => roleSelection.GetForSpanishRole(t) ?? new EmptyTranslateable());
				var spanishResultChunks = spanishPhrase.SelectMany (s => s.GetResult (frame));

				var result = new Exercise();
				result.Original = MakeSpanishSentence (spanishResultChunks);
				result.Tags = spanishResultChunks.SelectMany (p => p.Tags).Distinct().ToList ();

				var englishPhrase = englishTemplate.Select(t => roleSelection.GetForEnglishRole(t) ?? new EmptyTranslateable());
				var englishChunks = englishPhrase.SelectMany(e => e.GetResult(frame));

				result.Translation = MakeEnglishSentence (phoneticData, englishChunks);					
			
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

