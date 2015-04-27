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
			var results = new List<EspanolGenerator.Exercise> ();
			var cannedData = new BetterCannedData ();

			var generator = new EspanolGenerator (cannedData, "./data");

			foreach (var verbString in Verbs) {
				var verb = generator.LookupVerb (verbString);
				results.AddRange (generator.GetForVerb(verb, true));
			}

			if (IncludeTranslations) {
				var translations = GetTranslation.DownloadTranslationsFromGoogle(results.Select(t => t.Original));

				foreach (var result in results) {
					result.Translated = translations [result.Original];
				}
			}

			ExerciseGenerator.Exercise.WriteAsCsv (Console.Out, results);

			return 0;
		} 
	}
}

