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

		public DataLoader (string dataDirectory)
		{
			this.dataDirectory = dataDirectory;
		}

		public List<Verb> GetAllVerbs() {

			List<Verb> results = new List<Verb> ();

			foreach(var file in Directory.GetFiles(dataDirectory)) {

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

				verb.WithTenses (Verb.Conjugation.Present, GetPovLookupFromTableColumn (indicativeTable, 0));
				verb.WithTenses (Verb.Conjugation.PastPreterite, GetPovLookupFromTableColumn (indicativeTable, 1));
				verb.WithTenses (Verb.Conjugation.PastImperfect, GetPovLookupFromTableColumn (indicativeTable, 2));
				verb.WithTenses (Verb.Conjugation.Conditional, GetPovLookupFromTableColumn (indicativeTable, 3));
				verb.WithTenses (Verb.Conjugation.Future, GetPovLookupFromTableColumn (indicativeTable, 4));
				verb.WithTenses (Verb.Conjugation.PresentPerfect, GetPovLookupFromTableColumn (perfectTable, 0));


				results.Add (verb);
			}

			return results;
		}

		static string[][] LoadConjugationTable (CQ document, int tableIndex)
		{
			var selector = "a.vtable-label:eq(" + tableIndex + ") + .vtable-wrapper tr";
			//Console.WriteLine ("selector:" + selector);
			var tableRows = document [selector].ToArray ();

			var transformed = new string[5][];
			for (var initIndex = 0; initIndex < transformed.Length; initIndex++) {
				transformed [initIndex] = new string[6];
			}
			for (var rowIndex = 1; rowIndex < tableRows.Length; rowIndex++) {
				var row = tableRows [rowIndex];
				var columns = row.ChildNodes.ToArray ();
				for (var columnIndex = 1; columnIndex < columns.Length; columnIndex++) {
					var column = columns [columnIndex];
					transformed [columnIndex - 1] [rowIndex - 1] = WebUtility.HtmlDecode (column.InnerText);
				}
			}
			return transformed;
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
	}
}

