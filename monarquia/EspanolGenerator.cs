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
			var verb = allVerbs.Single (v => string.Equals (infinitive, v.Infinitive, StringComparison.InvariantCultureIgnoreCase));
		
			return GetForVerb (verb, limitVariations);
		}

		IEnumerable<string> GetForVerb (Verb verb, bool limitVariations)
		{
			List<string> results = new List<string>();

			var verbEndings = cannedData.GetVerbEndings (verb.Infinitive).ToArray();

			foreach (PointOfView pointOfView in Enum.GetValues (typeof(PointOfView))) {

				results.AddRange (GetForVerbConjugation (limitVariations, pointOfView, verbEndings, v => verb.ConjugatedPresentTense(v)));
				results.AddRange (GetForVerbConjugation (limitVariations, pointOfView, verbEndings, v => verb.ConjugatedPresentPerfectTense(v)));
			}
			return results;
		}

		List<string> GetForVerbConjugation (
			bool limitVariations, 
			PointOfView pointOfView, 
			IEnumerable<string> verbEndings, 
			Func<PointOfView,string> specificVerbConjugation)
		{
			List<string> results = new List<string> ();
			var selectedVerbEndings = verbEndings.ToArray ();
			if (limitVariations) {
				selectedVerbEndings = new string[] {
					selectedVerbEndings [random.Next (selectedVerbEndings.Length)]
				};
			}
			foreach (var verbEnding in selectedVerbEndings) {
				var result = SubjectPronounFor (pointOfView) + " " + specificVerbConjugation (pointOfView);
				if (!string.IsNullOrEmpty (verbEnding))
					result += " " + verbEnding;
				results.Add (result);
			}
			return results;
		}
	}
}

