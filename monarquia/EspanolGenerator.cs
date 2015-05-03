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

			var pointsOfView = Enum.GetValues (typeof(PointOfView)).Cast<PointOfView> ();
				
			if (limitVariations) {
				pointsOfView = ChoosePointOfViewsForDrill ();
			}

			var framings = from p in pointsOfView
				from c in Enum.GetValues(typeof(Conjugation)).Cast<Conjugation>()
				select new Frame(p, c);

			foreach (var framing in framings) {
				results.AddRange (GetForVerbConjugation (verb, limitVariations, framing.PointOfView, cannedData, framing.Conjugation));
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
			var rootRoleSelection = new RoleSelection (new Frame (pointOfView, conjugation));
			rootRoleSelection = rootRoleSelection.WithRole ("verbPhrase", verb.GetTranslateable (conjugation, cannedData, dataLoader));
			IEnumerable<RoleSelection> roleSelections = new [] { rootRoleSelection };

			roleSelections = roleSelections.SelectMany (selection => {
				return Pronouns.GetSubjectNouns().Where(n => n.AllowsFraming(new Frame(pointOfView, conjugation)))
					.Select(n => selection.WithRole("subject", n));
			});

			roleSelections = roleSelections.SelectMany (selection => {
				var verbEndings = cannedData.GetVerbEndings (verb.Infinitive, pointOfView).ToArray ();
				if (limitVariations) {
					verbEndings = new [] {
						verbEndings [random.Next (verbEndings.Length)]
					};
				}

				return verbEndings.Select (ve => selection.WithRole ("verbEnding", ve));
			});

			roleSelections = roleSelections.SelectMany (selection => {
				var timeframes = cannedData.GetTimeframeExpressions(conjugation).ToArray();

				if (limitVariations) {
					timeframes = new [] {
						timeframes[random.Next(timeframes.Length)]
					};
				}			

				return timeframes.Select (tf => selection.WithRole ("timeframe", tf));
			});

			List<Exercise> results = new List<Exercise> ();

			foreach (var roleSelection in roleSelections)
			{
				List<ITranslateable> spanishPhrase = new List<ITranslateable> ();
				spanishPhrase.Add (roleSelection.GetForRole("timeframe"));
				spanishPhrase.Add (roleSelection.GetForRole("subject"));
				spanishPhrase.Add (roleSelection.GetForRole("verbPhrase"));
				spanishPhrase.Add (roleSelection.GetForRole("verbEnding"));

				var result = new Exercise();
				result.Original = MakeSentenceFromWords (spanishPhrase.Select(p => p.AsSpanish(pointOfView)));
				result.HintsForTranslated = spanishPhrase.SelectMany (p => p.GetEnglishHints ()).ToList();
				result.Tags = spanishPhrase.SelectMany (p => p.GetTags()).ToList ();
				result.ExtraInfo = string.Join (", ", spanishPhrase.SelectMany (p => p.GetExtraHints ()));

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

