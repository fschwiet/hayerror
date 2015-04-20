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

			var results = new List<EspanolGenerator.Exercise> ();

			foreach (var verb in Verbs) {
				results.AddRange (generator.GetForVerb (verb, true));
			}

			if (IncludeTranslations) {
				var translations = GetTranslation.DownloadTranslationsFromGoogle(results.Select(t => t.Original));

				foreach (var result in results) {
					result.Translated = translations [result.Original];
				}
			}

			using (var client = new System.Net.WebClient ()) {

				var csv = new CsvWriter( Console.Out);

				foreach (var result in results) {

					csv.WriteField (result.Original);

					var translated = result.Translated;
					if (result.HintsForTranslated.Any()) {
						translated = "(" + string.Join(", ", result.HintsForTranslated) + ") " + result.Translated;
					}

					csv.WriteField (translated);
					csv.WriteField (result.ExtraInfo);
					csv.WriteField (string.Join(" ", result.Tags));
					csv.NextRecord ();
				}

				Console.Out.Flush ();
			}

	

			return 0;
		} 
	}
}

