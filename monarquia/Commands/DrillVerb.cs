using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ManyConsole;
using CsvHelper;

namespace monarquia
{
	public class DrillVerb : ConsoleCommand
	{
		public string Verb;

		public DrillVerb ()
		{
			this.IsCommand ("drill-verb", "Generate phrases for a particular verb");
			this.HasAdditionalArguments (1, " <verb infinitive>");
		}

		public override int? OverrideAfterHandlingArgumentsBeforeRun (string[] remainingArguments)
		{
			Verb = remainingArguments [0].ToLower ();

			return base.OverrideAfterHandlingArgumentsBeforeRun (remainingArguments);
		}

		public override int Run (string[] remainingArguments)
		{
			var generator = new EspanolGenerator ("./data");

			var results = generator.GetForVerb (Verb, true);

			using (var client = new System.Net.WebClient ()) {

				var memoryStream = new MemoryStream ();
				var csv = new CsvWriter( new StreamWriter(memoryStream, System.Text.Encoding.UTF8));

				foreach (var original in results) {

					var translation = GetTranslation.DownloadTranslation (client, original);

					Console.WriteLine (original + ", " + translation);
					csv.WriteField(original);
					csv.WriteField (translation);
					csv.NextRecord ();
				}
			}

	

			return 0;
		} 
	}
}

