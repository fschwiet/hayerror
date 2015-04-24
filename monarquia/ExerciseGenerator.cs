using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;

namespace monarquia
{
	public class ExerciseGenerator {

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

			public int GetWeight() {
				return WeighString (Original);
			}

			public static int WeighString(string text) {
				//  At points we try to pick 'nicer' exercises.
				//    - We prefer shorter exercises.
				//    - We prefer fewer numbers
				//    - We prefer not ALL-CAPS

				return text.Length
					+ text.Select (c => Char.IsDigit (c)).Count() * 4
					+ text.Select (c => Char.IsUpper (c)).Count() * 4;
			}

			static public void WriteAsCsv(TextWriter textWriter, IEnumerable<Exercise> exercises) {

				var csv = new CsvWriter(textWriter);

				foreach (var exercise in exercises) {

					csv.WriteField (exercise.Original);

					var translated = exercise.Translated;
					if (exercise.HintsForTranslated.Any()) {
						translated = "(" + string.Join(", ", exercise.HintsForTranslated) + ") " + exercise.Translated;
					}

					csv.WriteField (translated);
					csv.WriteField (exercise.ExtraInfo);
					csv.WriteField (string.Join(" ", exercise.Tags));
					csv.NextRecord ();
				}

				textWriter.Flush ();
			}
		}

		protected List<Verb> allVerbs;
		protected Random random  = new Random ();

		public ExerciseGenerator (string dataDirectory)
		{
			var dataLoader = new DataLoader (dataDirectory);
			allVerbs = dataLoader.GetAllVerbs ();
		}

		public Verb LookupVerb(string infinitive) {
			var verb = allVerbs.SingleOrDefault (v => string.Equals (infinitive, v.Infinitive, StringComparison.InvariantCultureIgnoreCase));

			if (verb == null) {
				throw new Exception ("Verb does not have data: " + infinitive);
			}

			return verb;
		}

		public List<PointOfView> ChoosePointOfViewsForDrill ()
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
	}
	
}
