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

		public EspanolGenerator (string dataDirectory)
		{
			var dataLoader = new DataLoader (dataDirectory);
			allVerbs = dataLoader.GetAllVerbs ();
		}

		public IEnumerable<Verb> GetAllVerbs() {
			return allVerbs;
		}

		public IEnumerable<string> GetAll(){

			var cannedData = new CannedData ();

			List<string> results = new List<string>();
			foreach(var verb in GetAllVerbs()) {

				var verbEndings = cannedData.GetVerbEndings (verb.Infinitive);

				foreach (var verbEnding in verbEndings) {
					foreach (PointOfView pointOfView in Enum.GetValues(typeof(PointOfView))) {

						var result = SubjectPronounFor (pointOfView) + " " + verb.ConjugatedPresentTense (pointOfView);

						if (!string.IsNullOrEmpty (verbEnding))
							result += " " + verbEnding;
						
						results.Add(result);
					}
				}
			}

			return results;
		}
	}
}

