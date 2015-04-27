using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsQuery;
using System.Net;

namespace monarquia
{
	public class DataLoader
	{
		string dataDirectory;
		List<Verb> spanishVerbs;
		List<Verb> englishVerbs;

		public DataLoader (string dataDirectory)
		{
			this.dataDirectory = dataDirectory;
		}

		public List<Verb> GetAllSpanishVerbs() {

			if (spanishVerbs != null)
				return spanishVerbs;

			List<Verb> results = new List<Verb> ();

			foreach(var file in Directory.GetFiles(Path.Combine(dataDirectory, "spanish-verbs"))) {

				var expectedFileEnding = ".conjugation.txt";

				if (!file.EndsWith (expectedFileEnding)) {
					continue;
				}

				var filename = new FileInfo (file).Name;
				string infinitive = filename.Substring (0, filename.Length - expectedFileEnding.Length);

				CsQuery.CQ document = File.ReadAllText (file);

				var indicativeTable = LoadConjugationTable (document, 0);
				var perfectTable = LoadConjugationTable (document, 3);

				var verb = new Verb (infinitive);

				verb.WithTenses (Conjugation.Present, GetPovLookupFromTableColumn (indicativeTable, 0));
				verb.WithTenses (Conjugation.PastPreterite, GetPovLookupFromTableColumn (indicativeTable, 1));
				verb.WithTenses (Conjugation.PastImperfect, GetPovLookupFromTableColumn (indicativeTable, 2));
				verb.WithTenses (Conjugation.Conditional, GetPovLookupFromTableColumn (indicativeTable, 3));
				verb.WithTenses (Conjugation.Future, GetPovLookupFromTableColumn (indicativeTable, 4));
				verb.WithTenses (Conjugation.PresentPerfect, GetPovLookupFromTableColumn (perfectTable, 0));


				results.Add (verb);
			}

			return spanishVerbs = results;
		}

		public List<Verb> GetAllEnglishVerbs() {

			if (englishVerbs != null)
				return englishVerbs;

			List<Verb> results = new List<Verb> ();

			foreach(var file in Directory.GetFiles(Path.Combine(dataDirectory, "english-verbs"))) {

				var expectedFileEnding = ".conjugation.txt";

				if (!file.EndsWith (expectedFileEnding)) {
					continue;
				}

				var filename = new FileInfo (file).Name;
				string infinitive = filename.Substring (0, filename.Length - expectedFileEnding.Length);

				CsQuery.CQ document = File.ReadAllText (file);

				var indicativeTable = LoadConjugationTable (document, 0);
				var perfectTable = LoadConjugationTable (document, 1);

				var verb = new Verb (infinitive);

				var futureConditional = GetEnglishPovLookupFromRow (indicativeTable, 2);
				foreach (var key in futureConditional.Keys.ToArray()) {
					futureConditional [key] = futureConditional[key].Replace("will", "would");
				}

				verb.WithTenses (Conjugation.Present, GetEnglishPovLookupFromRow (indicativeTable, 0));
				verb.WithTenses (Conjugation.PastPreterite, GetEnglishPovLookupFromRow (indicativeTable, 1));
				verb.WithTenses (Conjugation.PastImperfect, GetEnglishPovLookupFromRow (indicativeTable, 1));
				verb.WithTenses (Conjugation.Conditional, futureConditional);
				verb.WithTenses (Conjugation.Future, GetEnglishPovLookupFromRow (indicativeTable, 2));
				verb.WithTenses (Conjugation.PresentPerfect, GetEnglishPovLookupFromRow (perfectTable, 0));

				results.Add (verb);
			}

			return englishVerbs = results;
		}

		static string[][] LoadConjugationTable (CQ document, int tableIndex)
		{
			var tableRows = FindTable (document, tableIndex);

			var rowCount = tableRows [1].ChildNodes.Length - 1;
			var columnCount = tableRows.Length - 1;

			var transformed = new string[rowCount][];
			for (var initIndex = 0; initIndex < transformed.Length; initIndex++) {
				transformed [initIndex] = new string[columnCount];
			}
			for (var rowIndex = 1; rowIndex < tableRows.Length; rowIndex++) {
				var row = tableRows [rowIndex];
				var columns = row.ChildNodes.ToArray ();
				for (var columnIndex = 1; columnIndex < columns.Length; columnIndex++) {
					var column = columns [columnIndex];

					transformed [columnIndex - 1] [rowIndex - 1] = WebUtility.HtmlDecode (column.Cq().Text());
				}
			}
			return transformed;
		}

		static IDomObject[] FindTable (CQ document, int tableIndex)
		{
			var selector = ".vtable-label:eq(" + tableIndex + ") + .vtable-wrapper tr";
			var tableRows = document [selector].ToArray ();
			return tableRows;
		}

		static Dictionary<PointOfView, string> GetPovLookupFromTableColumn (string[][] indicativeTable, int columnIndex)
		{
			var presentTenses = new Dictionary<PointOfView, string> ();

			presentTenses [PointOfView.FirstPerson] = indicativeTable [columnIndex] [0];
			presentTenses [PointOfView.SecondPerson] = indicativeTable [columnIndex] [1];
			presentTenses [PointOfView.SecondPersonFormal] = indicativeTable [columnIndex] [2];
			presentTenses [PointOfView.ThirdPersonMasculine] = indicativeTable [columnIndex] [2];
			presentTenses [PointOfView.ThirdPersonFeminine] = indicativeTable [columnIndex] [2];
			presentTenses [PointOfView.FirstPersonPlural] = indicativeTable [columnIndex] [3];
			presentTenses [PointOfView.SecondPersonPlural] = indicativeTable [columnIndex] [4];
			presentTenses [PointOfView.SecondPersonPluralFormal] = indicativeTable [columnIndex] [5];
			presentTenses [PointOfView.ThirdPersonPluralMasculine] = indicativeTable [columnIndex] [5];
			presentTenses [PointOfView.ThirdPersonPluralFeminine] = indicativeTable [columnIndex] [5];
			return presentTenses;
		}

		static Dictionary<PointOfView, string> GetEnglishPovLookupFromRow (string[][] indicativeTable, int columnIndex)
		{
			var presentTenses = new Dictionary<PointOfView, string> ();

			presentTenses [PointOfView.FirstPerson] = indicativeTable [columnIndex] [0];
			presentTenses [PointOfView.SecondPerson] = indicativeTable [columnIndex] [1];
			presentTenses [PointOfView.SecondPersonFormal] = indicativeTable [columnIndex] [1];
			presentTenses [PointOfView.ThirdPersonMasculine] = indicativeTable [columnIndex] [2];
			presentTenses [PointOfView.ThirdPersonFeminine] = indicativeTable [columnIndex] [2];
			presentTenses [PointOfView.FirstPersonPlural] = indicativeTable [columnIndex] [3];
			presentTenses [PointOfView.SecondPersonPlural] = indicativeTable [columnIndex] [4];
			presentTenses [PointOfView.SecondPersonPluralFormal] = indicativeTable [columnIndex] [4];
			presentTenses [PointOfView.ThirdPersonPluralMasculine] = indicativeTable [columnIndex] [5];
			presentTenses [PointOfView.ThirdPersonPluralFeminine] = indicativeTable [columnIndex] [5];
			return presentTenses;
		}
	}
}

