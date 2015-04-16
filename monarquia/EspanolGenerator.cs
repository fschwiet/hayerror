using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace monarquia
{
	public class EspanolGenerator
	{
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
				results.AddRange(GetForVerb(verb, false));
			}

			return results;
		}

		public IEnumerable<string> GetForVerb(string infinitive, bool limitVariations) {
			
			var verb = allVerbs.SingleOrDefault (v => string.Equals (infinitive, v.Infinitive, StringComparison.InvariantCultureIgnoreCase));
		
			if (verb == null) {
				throw new Exception ("Verb does not have data: " + infinitive);
			}

			return GetForVerb (verb, limitVariations);
		}

		IEnumerable<string> GetForVerb (Verb verb, bool limitVariations)
		{
			List<string> results = new List<string>();

			var selectedPointsOfView = Enum.GetValues (typeof(PointOfView)).Cast<PointOfView> ();
				
			if (limitVariations) {
				// don't use vosotros
				selectedPointsOfView = selectedPointsOfView.Where (v => v != PointOfView.SecondPersonPlural);

				// only use one of el/ella/usted
				// only use one of ellos/ellas/ustedes

				Func<PointOfView> randomPointOfViewThatIsThirdPersonish = delegate() {
					switch (random.Next (3)) {
					case 0:
						return PointOfView.ThirdPersonFeminine;
					case 1:
						return PointOfView.ThirdPersonMasculine;
					case 2:
						return PointOfView.SecondPersonFormal;
					default:
						throw new Exception("random.Next() produced unexpected");
					}
				};

				Func<PointOfView> randomPointOfViewThatIsThirdPersonPluralish = delegate() {
					switch (random.Next (3)) {
					case 0:
						return PointOfView.ThirdPersonPluralFeminine;
					case 1:
						return PointOfView.ThirdPersonMasculine;
					case 2:
						return PointOfView.SecondPersonPluralFormal;
					default:
						throw new Exception("random.Next() produced unexpected");
					}
				};

				selectedPointsOfView = selectedPointsOfView
					.Where (v => v != PointOfView.ThirdPersonFeminine &&
						v != PointOfView.ThirdPersonMasculine &&
						v != PointOfView.SecondPersonFormal &&
						v != PointOfView.ThirdPersonPluralFeminine &&
						v != PointOfView.ThirdPersonPluralMasculine &&
						v != PointOfView.SecondPersonPluralFormal);

				results.AddRange(GetAllConjugationsForVerb (verb, limitVariations, randomPointOfViewThatIsThirdPersonish));
				results.AddRange(GetAllConjugationsForVerb (verb, limitVariations, randomPointOfViewThatIsThirdPersonPluralish));
			}

			foreach (PointOfView pointOfView in selectedPointsOfView) {

				results.AddRange (GetAllConjugationsForVerb (verb, limitVariations, () => pointOfView));
			}

			return results;
		}

		List<string> GetForVerbConjugation (
			Verb verb,
			bool limitVariations, 
			PointOfView pointOfView, 
			CannedData.Timeframe timeframe,
			CannedData cannedData,
			Verb.Conjugation conjugation)
		{
			List<string> results = new List<string> ();

			var selectedVerbEndings = cannedData.GetVerbEndings (verb.Infinitive).ToArray();
			if (limitVariations) {
				selectedVerbEndings = new string[] {
					selectedVerbEndings [random.Next (selectedVerbEndings.Length)]
				};
			}

			var selectedTimeframes = cannedData.GetTimeframeExpressions(timeframe).ToArray();

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

				results.Add (capitolizedSentencewithPeriod);
			}

			return results;
		}

		List<string> GetAllConjugationsForVerb (Verb verb, bool limitVariations, Func<PointOfView> pointOfViewSelector)
		{
			List<string> results = new List<string> ();

			results.AddRange (GetForVerbConjugation (verb, limitVariations, pointOfViewSelector (), CannedData.Timeframe.Present, cannedData, Verb.Conjugation.Present));
			results.AddRange (GetForVerbConjugation (verb, limitVariations, pointOfViewSelector (), CannedData.Timeframe.PastPreterite, cannedData, Verb.Conjugation.PastPreterite));
			results.AddRange (GetForVerbConjugation (verb, limitVariations, pointOfViewSelector (), CannedData.Timeframe.PastImperfect, cannedData, Verb.Conjugation.PastImperfect));
			results.AddRange (GetForVerbConjugation (verb, limitVariations, pointOfViewSelector (), CannedData.Timeframe.NotImplemented, cannedData, Verb.Conjugation.PresentPerfect));

			return results;
		}
	}
}

