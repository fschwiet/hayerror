using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsQuery;
using System.Net;
using Newtonsoft.Json.Linq;

namespace monarquia
{
	public class DataLoader
	{
		string dataDirectory;
		List<VerbConjugator> spanishVerbs;
		List<VerbConjugator> englishVerbs;

		public DataLoader (string dataDirectory)
		{
			this.dataDirectory = dataDirectory;
		}

		public List<VerbConjugator> GetAllSavedSpanishVerbs() {

			if (spanishVerbs != null)
				return spanishVerbs;

			List<VerbConjugator> results = new List<VerbConjugator> ();

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

				var verb = new CannedVerbConjugator (infinitive);

				var presentPovLookup = GetPovLookupFromTableColumn (indicativeTable, 0);

				if (infinitive == "haber") {
					presentPovLookup [PointOfView.ThirdPersonMasculine] =
						presentPovLookup [PointOfView.ThirdPersonFeminine] =
							presentPovLookup [PointOfView.SecondPersonFormal] =
								"hay";
				}

				verb.WithTenses (Conjugation.Present, presentPovLookup);
				verb.WithTenses (Conjugation.PastPreterite, GetPovLookupFromTableColumn (indicativeTable, 1));
				verb.WithTenses (Conjugation.PastImperfect, GetPovLookupFromTableColumn (indicativeTable, 2));
				verb.WithTenses (Conjugation.Conditional, GetPovLookupFromTableColumn (indicativeTable, 3));
				verb.WithTenses (Conjugation.Future, GetPovLookupFromTableColumn (indicativeTable, 4));
				verb.WithTenses (Conjugation.PresentPerfect, GetPovLookupFromTableColumn (perfectTable, 0));

				if (infinitive == "haber") {
					verb.MakeThirdPersonPluralMatchSingular ();
				}

				results.Add (verb);
			}

			return spanishVerbs = results;
		}

		public List<VerbConjugator> GetAllSavedEnglishVerbs() {

			if (englishVerbs != null)
				return englishVerbs;

			List<VerbConjugator> results = new List<VerbConjugator> ();

			foreach(var file in Directory.GetFiles(Path.Combine(dataDirectory, "english-verbs"))) {

				var expectedFileEnding = ".verbix.txt";

				if (!file.EndsWith (expectedFileEnding)) {
					continue;
				}

				var filename = new FileInfo (file).Name;
				string infinitive = filename.Substring (0, filename.Length - expectedFileEnding.Length);

				var verb = new CannedVerbConjugator (infinitive);
				var verbixData = Verbix.ScrapeVerbixVeb (file);

				verb.WithTenses (Conjugation.Present, GetEnglishPovLookup (verbixData.Get("Indicative", "Present")));
				verb.WithTenses (Conjugation.PastPreterite, GetEnglishPovLookup (verbixData.Get("Indicative", "Past")));
				verb.WithTenses (Conjugation.PastImperfect, GetEnglishPovLookup (verbixData.Get("Indicative", "Past")));
				verb.WithTenses (Conjugation.Conditional, GetEnglishPovLookup (verbixData.Get("Conditional", "Present")));
				verb.WithTenses (Conjugation.Future, GetEnglishPovLookup (verbixData.Get("Indicative", "Future")));
				verb.WithTenses (Conjugation.PresentPerfect, GetEnglishPovLookup (verbixData.Get("Indicative", "Perfect")));

				results.Add (verb);
			}

			return englishVerbs = results;
		}

		Dictionary<PointOfView,string> GetEnglishPovLookup(JToken token)
		{
			var source = token as JObject;
			var results = new Dictionary<PointOfView, string> ();

			results [PointOfView.FirstPerson] = source.GetValue ("I").ToString();
			results [PointOfView.SecondPerson] = source.GetValue ("you").ToString();
			results [PointOfView.SecondPersonFormal] = source.GetValue ("you").ToString();
			results [PointOfView.ThirdPersonMasculine] = source.GetValue ("he").ToString();
			results [PointOfView.ThirdPersonFeminine] = source.GetValue ("he").ToString();
			results [PointOfView.FirstPersonPlural] = source.GetValue ("we").ToString();
			results [PointOfView.SecondPersonPlural] = source.GetValue ("you2").ToString();
			results [PointOfView.SecondPersonPluralFormal] = source.GetValue ("you2").ToString();
			results [PointOfView.ThirdPersonPluralMasculine] = source.GetValue ("they").ToString();
			results [PointOfView.ThirdPersonPluralFeminine] = source.GetValue ("they").ToString();

			return results;
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
			var results = new Dictionary<PointOfView, string> ();

			results [PointOfView.FirstPerson] = indicativeTable [columnIndex] [0];
			results [PointOfView.SecondPerson] = indicativeTable [columnIndex] [1];
			results [PointOfView.SecondPersonFormal] = indicativeTable [columnIndex] [2];
			results [PointOfView.ThirdPersonMasculine] = indicativeTable [columnIndex] [2];
			results [PointOfView.ThirdPersonFeminine] = indicativeTable [columnIndex] [2];
			results [PointOfView.FirstPersonPlural] = indicativeTable [columnIndex] [3];
			results [PointOfView.SecondPersonPlural] = indicativeTable [columnIndex] [4];
			results [PointOfView.SecondPersonPluralFormal] = indicativeTable [columnIndex] [5];
			results [PointOfView.ThirdPersonPluralMasculine] = indicativeTable [columnIndex] [5];
			results [PointOfView.ThirdPersonPluralFeminine] = indicativeTable [columnIndex] [5];
			return results;
		}

	}
}

