using System;
using System.Collections.Generic;

namespace monarquia
{
	public class EspanolGenerator
	{
		public enum PointOfView {
			FirstPerson,
			SecondPerson,
			SecondPersonFormal,
			ThirdPersonMasculine,
			ThirdPersonFeminine,

			FirstPersonPlural,
			SecondPersonPlural,
			SecondPersonPluralFormal,
			ThirdPersonPluralMasculine,
			ThirdPersonPluralFeminine
		};

		public string SubjectPronounFor(PointOfView v) {
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

		public EspanolGenerator ()
		{
		}

		public IEnumerable<Verb> GetAllVerbs() {
			return new [] {
				new Verb("ir").withPresentTense("voy", "vas", "va", "va", "va", "vamos","vais", "van", "van", "van"),
				new Verb("gritar").withPresentTense("grito", "gritas", "grita", "grita", "grita", "gritamos", "gritáis", "van", "van", "van")
			};
		}

		public class Verb {
		
			public readonly string Preterite;
			public Dictionary<PointOfView, string> PresentTense;

			public Verb(string preterite) {
				Preterite = preterite;
			}

			public Verb withPresentTense(params string[] values) {

				var expectedParamCounts = Enum.GetValues (typeof(PointOfView)).Length;
				if (values.Length != Enum.GetValues (typeof(PointOfView)).Length)
					throw new Exception ("Expected " + expectedParamCounts + " parameters.");

				PresentTense = new Dictionary<PointOfView,string> ();
				PresentTense[PointOfView.FirstPerson] = values[0];
				PresentTense[PointOfView.SecondPerson] = values[1];
				PresentTense[PointOfView.SecondPersonFormal] = values[2];
				PresentTense[PointOfView.ThirdPersonMasculine] = values[3];
				PresentTense[PointOfView.ThirdPersonFeminine] = values[4];
				PresentTense[PointOfView.FirstPersonPlural] = values[5];
				PresentTense[PointOfView.SecondPersonPlural] = values[6];
				PresentTense[PointOfView.SecondPersonPluralFormal] = values[7];
				PresentTense[PointOfView.ThirdPersonPluralMasculine] = values[8];
				PresentTense[PointOfView.ThirdPersonPluralFeminine] = values[9];

				return this;
			}

			public string ConjugatedPresentTense(PointOfView v) {
				return PresentTense [v];
			}
		}


		public IEnumerable<string> GetAll(){

			List<string> results = new List<string>();
			foreach(var verb in GetAllVerbs()) {

				foreach (PointOfView pointOfView in Enum.GetValues(typeof(PointOfView))) {
					Console.WriteLine (pointOfView);
					results.Add(SubjectPronounFor(pointOfView) + " " + verb.ConjugatedPresentTense(pointOfView));
				}
			}

			return results;
		}
	}
}

