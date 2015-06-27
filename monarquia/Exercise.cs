using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;

namespace monarquia
{
	public class Exercise {
		public string Original;
		public string Translation;
		public string ExtraInfo;
		public List<string> Tags = new List<string>();

		public Exercise Clone() {
			return new Exercise () {
				Original = this.Original,
				Translation = this.Translation,
				ExtraInfo = this.ExtraInfo,
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
				csv.WriteField (exercise.Translation);
				csv.WriteField (exercise.ExtraInfo);
				csv.WriteField (string.Join(" ", exercise.Tags));
				csv.NextRecord ();
			}

			textWriter.Flush ();
		}
	}
	
}
