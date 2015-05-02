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

		List<Verb> GetAllVerbs ()
		{
			List<Verb> verbs = new List<Verb> ();
			verbs.AddRange (dataLoader.GetAllSavedSpanishVerbs ());
			var reflexiveVerbs = cannedData.GetReflexiveVerbs (dataLoader).Select (inf => new ReflexiveVerb (inf, dataLoader));
			verbs.AddRange (cannedData.GetReflexiveVerbs (dataLoader).Select (inf => new ReflexiveVerb (inf, dataLoader)));
			return verbs;
		}

		public Verb LookupVerb(string infinitive) {

			var verb =  GetAllVerbs().SingleOrDefault (v => string.Equals (infinitive, v.Infinitive, StringComparison.InvariantCultureIgnoreCase));

			if (verb == null) {
				throw new Exception ("Verb does not have data: " + infinitive);
			}

			return verb;
		}

		public IEnumerable<Exercise> GetAll()
		{
			var verbs = GetAllVerbs ();

			List<Exercise> results = new List<Exercise>();

			foreach(var verb in verbs) 
			{
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
			ICannedData cannedData,
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
				spanishPhrase.Add (verb.GetTranslateable (conjugation, cannedData, dataLoader));
				spanishPhrase.Add (scenario.verbEnding);

				var result = resultTemplate.Clone ();
				result.Original = MakeSentenceFromWords (spanishPhrase.Select(p => p.AsSpanish(pointOfView)));

				result.HintsForTranslated = spanishPhrase.SelectMany (p => p.GetEnglishHints ()).ToList();

				try {
					result.Translated = MakeEnglishSentenceFromWords (phoneticData, spanishPhrase.Select(p => p.AsEnglish(pointOfView)));					
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

