using System;
using System.Collections.Generic;
using System.Linq;
using ManyConsole;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace monarquia
{
	public class DrillWithLinguee : ConsoleCommand
	{
		public string[] Verbs;

		public DrillWithLinguee ()
		{
			this.IsCommand ("linguee-verb", "Use Linguee to find phrases for a particular verb");
			this.AllowsAnyAdditionalArguments (" <verb infinitive>+");
			this.SkipsCommandSummaryBeforeRunning ();
		}

		public override int? OverrideAfterHandlingArgumentsBeforeRun (string[] remainingArguments)
		{
			Verbs = remainingArguments.ToArray ();
			return base.OverrideAfterHandlingArgumentsBeforeRun (remainingArguments);
		} 

		public override int Run (string[] remainingArguments)
		{
			List<ExerciseGenerator.Exercise> results = new List<ExerciseGenerator.Exercise> ();
			LingueeGenerator generator = new LingueeGenerator ("./data");

			var options = new ChromeOptions ();
			var service = ChromeDriverService.CreateDefaultService ();
			service.SuppressInitialDiagnosticInformation = true;

			using (var client = new ChromeDriver (service, options)) {
				foreach (var verb in Verbs) {
					results.AddRange(generator.ForVerb(client, verb));
				}
			}

			ExerciseGenerator.Exercise.WriteAsCsv (Console.Out, results);

			return 0;
		} 
	}
}

