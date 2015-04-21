using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace monarquia
{
	public class EspanolGenerator
	{
		public class Exercise {
			public string Original;
			public string Translated;
			public string ExtraInfo;
			public List<string> HintsForTranslated = new List<string>();
			public List<string> Tags = new List<string>();

			public Exercise Clone() {
				return new Exercise () {
					Original = this.Original,
					Translated = this.Translated,
					ExtraInfo = this.ExtraInfo,
					HintsForTranslated = new List<string> (this.HintsForTranslated),
					Tags = new List<string> (this.Tags)
				};
			}
		}

		public static string SubjectPronounFor(PointOfView v) {
			switch (v) {
			case PointOfView.FirstPerson:
				return "yo";
			case PointOfView.SecondPerson:
				return "tú";
			case PointOfView.SecondPersonFormal:
				return "usted";
			case PointOfView.ThirdPersonMasculine:
				return "él";
			case PointOfView.ThirdPersonFeminine:
				return "ella";

			case PointOfView.FirstPersonPlural:
				return "nosotros";
			case PointOfView.SecondPersonPlural:
				return "vosotros";
			case PointOfView.SecondPersonPluralFormal:
				return "ustedes";
			case PointOfView.ThirdPersonPluralMasculine:
				return "ellos";
			case PointOfView.ThirdPersonPluralFeminine:
				return "ellas";
			default:
				throw new Exception ("Unrecognized PointOfView");
			}
		}

		List<Verb> allVerbs;
		CannedData cannedData;
		Random random;

		public EspanolGenerator (string dataDirectory)
		{
			var dataLoader = new DataLoader (dataDirectory);
			allVerbs = dataLoader.GetAllVerbs ();
			cannedData = new CannedData ();
			random = new Random ();
		}

		public IEnumerable<Verb> GetAllVerbs() {
			return allVerbs;
		}

		public IEnumerable<string> GetAll(){

			List<string> results = new List<string>();
			foreach(var verb in GetAllVerbs()) {
				results.AddRange(GetForVerb(verb, false).Select(e => e.Original));
			}

			return results;
		}

		public IEnumerable<Exercise> GetForVerb(string infinitive, bool limitVariations) {
			
			var verb = allVerbs.SingleOrDefault (v => string.Equals (infinitive, v.Infinitive, StringComparison.InvariantCultureIgnoreCase));
		
			if (verb == null) {
				throw new Exception ("Verb does not have data: " + infinitive);
			}

			return GetForVerb (verb, limitVariations);
		}

		IEnumerable<Exercise> GetForVerb (Verb verb, bool limitVariations)
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

		List<PointOfView> ChoosePointOfViewsForDrill ()
		{
			var results = Enum.GetValues (typeof(PointOfView)).Cast<PointOfView> ().ToList();

			// don't use vosotros
			results = results.Where (v => v != PointOfView.SecondPersonPlural).ToList ();

			// only use one of el/ella/usted
			// only use one of ellos/ellas/ustedes
			results = results.Where (v => v != PointOfView.ThirdPersonFeminine && 
				v != PointOfView.ThirdPersonMasculine && 
				v != PointOfView.SecondPersonFormal && 
				v != PointOfView.ThirdPersonPluralFeminine && 
				v != PointOfView.ThirdPersonPluralMasculine && 
				v != PointOfView.SecondPersonPluralFormal).ToList ();
		
			switch (random.Next (3)) {
			case 0:
				results.Add (PointOfView.ThirdPersonFeminine);
				break;
			case 1:
				results.Add (PointOfView.ThirdPersonMasculine);
				break;
			case 2:
				results.Add (PointOfView.SecondPersonFormal);
				break;
			}

			switch (random.Next (3)) {
			case 0:
				results.Add (PointOfView.ThirdPersonPluralFeminine);
				break;
			case 1:
				results.Add (PointOfView.ThirdPersonMasculine);
				break;
			case 2:
				results.Add (PointOfView.SecondPersonPluralFormal);
				break;
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
				resultTemplate.HintsForTranslated.Add (SubjectPronounFor (pointOfView));
			
			resultTemplate.Tags.Add ("conjugation:" + conjugation);
			resultTemplate.Tags.Add ("verb:" + verb.Infinitive);

			List<Exercise> results = new List<Exercise> ();

			var selectedVerbEndings = cannedData.GetVerbEndings (verb.Infinitive, pointOfView).ToArray();
			if (limitVariations) {
				selectedVerbEndings = new string[] {
					selectedVerbEndings [random.Next (selectedVerbEndings.Length)]
				};
			}

			var selectedTimeframes = cannedData.GetTimeframeExpressions(conjugation).ToArray();

			if (limitVariations) {
				selectedTimeframes = new string[] {
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

				accumulatedWords.Add (scenario.timeframe);

				accumulatedWords.Add(SubjectPronounFor (pointOfView));
				accumulatedWords.Add(verb.ConjugatedForTense (conjugation, pointOfView));

				accumulatedWords.Add(scenario.verbEnding);
						
				var nonemptyWordsJoinedBySpaces = 
					string.Join (" ", accumulatedWords.Where (w => !string.IsNullOrEmpty (w)));

				var capitolizedSentencewithPeriod = 
					nonemptyWordsJoinedBySpaces.First ().ToString ().ToUpper () + nonemptyWordsJoinedBySpaces.Substring (1) + ".";

				var result = resultTemplate.Clone ();
				result.Original = capitolizedSentencewithPeriod;

				results.Add (result);
			}

			return results;
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

