using System;
using System.Collections.Generic;
using System.Linq;
using ManyConsole;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace monarquia
{
	public class LingueeLookup : ConsoleCommand
	{
		public string Original;

		public LingueeLookup ()
		{
			this.IsCommand ("linguee", "Looks up a linguee example with translation");
			this.AllowsAnyAdditionalArguments (" <expression>");
		}

		public override int? OverrideAfterHandlingArgumentsBeforeRun (string[] remainingArguments)
		{
			if (remainingArguments.Length < 1) {
				throw new ConsoleHelpAsException ("No input given");
			}

			Original = string.Join (" ", remainingArguments);

			return base.OverrideAfterHandlingArgumentsBeforeRun (remainingArguments);
		}

		public override int Run (string[] remainingArguments)
		{
			var options = new ChromeOptions ();
			var service = ChromeDriverService.CreateDefaultService ();
			service.SuppressInitialDiagnosticInformation = true;

			Dictionary<string, string> results;

			using (var client = new ChromeDriver (service, options)) {
				results = LingueeGenerator.DoLingueeLookup (client, Original);
			}

			if (results.Count == 0) {
				throw new ConsoleHelpAsException ("Could not find linguee.com examples for '" + Original + "'");
			}

			foreach(var shortestKey in results.Keys.OrderBy(key => key.Length).Take(2)) {

				Console.WriteLine ();
				Console.WriteLine (shortestKey);
				Console.WriteLine (results[shortestKey]);
			}

			return 0;
		}
	}
}

