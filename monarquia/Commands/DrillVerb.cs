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
		public bool UseBigCannedData = false;

		public DrillVerb ()
		{
			this.IsCommand ("drill-verb", "Generate phrases for a particular verb");
			this.AllowsAnyAdditionalArguments (" <verb infinitive>+");
			this.HasOption ("t", "Include translations.", v => IncludeTranslations = true);
			this.HasOption ("b", "Use BiggCannedData.", v => UseBigCannedData = true);
			this.SkipsCommandSummaryBeforeRunning ();
		}

		public override int? OverrideAfterHandlingArgumentsBeforeRun (string[] remainingArguments)
		{
			Verbs = remainingArguments.ToArray ();

			if (!Verbs.Any ())
				throw new ConsoleHelpAsException ("No verbs specified");
			
			return base.OverrideAfterHandlingArgumentsBeforeRun (remainingArguments);
		} 

		public override int Run (string[] remainingArguments)
		{
			var results = new List<EspanolGenerator.Exercise> ();
			var cannedData = new BetterCannedData ();

			if (UseBigCannedData)
				cannedData = new BigCannedData ();

			var generator = new EspanolGenerator (cannedData, "./data");

			foreach (var verbString in Verbs) {
				results.AddRange (generator.GetExercises(verbString, true));
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

