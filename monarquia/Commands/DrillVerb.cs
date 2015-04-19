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
		public string[] Verbs;
		public bool IncludeTranslations = false;

		public DrillVerb ()
		{
			this.IsCommand ("drill-verb", "Generate phrases for a particular verb");
			this.AllowsAnyAdditionalArguments (" <verb infinitive>+");
			this.HasOption ("t", "include translations", v => IncludeTranslations = true);
			this.SkipsCommandSummaryBeforeRunning ();
		}

		public override int? OverrideAfterHandlingArgumentsBeforeRun (string[] remainingArguments)
		{
			Verbs = remainingArguments.ToArray ();
			return base.OverrideAfterHandlingArgumentsBeforeRun (remainingArguments);
		} 

		public override int Run (string[] remainingArguments)
		{
			var generator = new EspanolGenerator ("./data");

			var results = new List<Tuple<string,string>> ();

			foreach (var verb in Verbs) {
				results.AddRange (generator.GetForVerb (verb, true).Select(t => new Tuple<string,string>(verb, t)));
			}

			if (!IncludeTranslations) {
				foreach (var result in results) {
					Console.WriteLine (result);
				}
				return 0;
			}

			var translated = GetTranslation.DownloadTranslationsFromGoogle(results.Select(t => t.Item2));

			using (var client = new System.Net.WebClient ()) {

				var csv = new CsvWriter( Console.Out);

				foreach (var result in results) {

					csv.WriteField (result.Item2);
					csv.WriteField (translated[result.Item2]);
					csv.WriteField (result.Item1);
					csv.WriteField ("hayerror:drill");
					csv.NextRecord ();
				}

				Console.Out.Flush ();
			}

	

			return 0;
		} 
	}
}

