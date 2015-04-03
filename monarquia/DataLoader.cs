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

				var indicativeRows = document ["a.vtable-label + .vtable-wrapper tr"].ToArray();
				
				var transformed = new string[5][];

				for (var initIndex = 0; initIndex < transformed.Length; initIndex++) {
					transformed [initIndex] = new string[6];
				}

				for (var rowIndex = 1; rowIndex < indicativeRows.Length; rowIndex++) {

					var row = indicativeRows [rowIndex];
					var columns = row.ChildNodes.ToArray ();

					for (var columnIndex = 1; columnIndex < columns.Length; columnIndex++) {
						var column = columns [columnIndex];

						transformed[columnIndex-1][rowIndex-1] = WebUtility.HtmlDecode(column.InnerText);
					}
				}

				var verb = new Verb (infinitive);

				var presentTenses = new Dictionary<PointOfView, string> ();
				presentTenses [PointOfView.FirstPerson] = transformed[0][0];
				presentTenses [PointOfView.SecondPerson] = transformed[0][1];
				presentTenses [PointOfView.SecondPersonFormal] = transformed[0][2];
				presentTenses [PointOfView.ThirdPersonMasculine] = transformed[0][2];
				presentTenses [PointOfView.ThirdPersonFeminine] = transformed[0][2];
				presentTenses [PointOfView.FirstPersonPlural] = transformed[0][3];
				presentTenses [PointOfView.SecondPersonPlural] = transformed[0][4];
				presentTenses [PointOfView.SecondPersonPluralFormal] = transformed[0][5];
				presentTenses [PointOfView.ThirdPersonPluralMasculine] = transformed[0][5];
				presentTenses [PointOfView.ThirdPersonPluralFeminine] = transformed[0][5];
				verb.WithPresentTenses (presentTenses);

				results.Add (verb);
			}

			return results;
		}
	}
}

