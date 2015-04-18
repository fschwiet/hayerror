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
			this.SkipsCommandSummaryBeforeRunning ();
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

			var translated = GetTranslation.DownloadTranslationsFromGoogle(results);

			using (var client = new System.Net.WebClient ()) {

				var csv = new CsvWriter( Console.Out);

				foreach (var result in translated) {

					csv.WriteField (result.Key);
					csv.WriteField (result.Value);
					csv.WriteField (Verb);
					csv.WriteField ("hayerror:drill");
					csv.NextRecord ();
				}

				Console.Out.Flush ();
			}

	

			return 0;
		} 
	}
}

