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

			public string GetTranslatedWithHints() {
				var result = Translated;
				if (HintsForTranslated.Any()) {
					result = "(" + string.Join(", ", HintsForTranslated) + ") " + Translated;
				}
				return result;
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
					csv.WriteField (exercise.GetTranslatedWithHints());
					csv.WriteField (exercise.ExtraInfo);
					csv.WriteField (string.Join(" ", exercise.Tags));
					csv.NextRecord ();
				}

				textWriter.Flush ();
			}
		}

		protected List<Verb> allVerbs;
		protected Random random  = new Random ();
		protected DataLoader dataLoader;

		public ExerciseGenerator (string dataDirectory)
		{
			dataLoader = new DataLoader (dataDirectory);
			allVerbs = dataLoader.GetAllSpanishVerbs ();
		}

		public Verb LookupVerb(string infinitive) {
			var verb = allVerbs.SingleOrDefault (v => string.Equals (infinitive, v.Infinitive, StringComparison.InvariantCultureIgnoreCase));

			if (verb == null) {
				throw new Exception ("Verb does not have data: " + infinitive);
			}

			return verb;
		}
	}
	
}
