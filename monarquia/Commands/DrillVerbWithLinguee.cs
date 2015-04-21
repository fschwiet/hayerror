using System;
using System.Collections.Generic;
using System.Linq;
using ManyConsole;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;



namespace monarquia
{
	public class DrillVerbWithLinguee : ConsoleCommand
	{
		public string Original;

		public DrillVerbWithLinguee ()
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
			var options = new ChromeOptions();
			var service = ChromeDriverService.CreateDefaultService();
			service.SuppressInitialDiagnosticInformation = true;

			Dictionary<string,string> results = new Dictionary<string,string> ();

			using (var client = new ChromeDriver (service, options)) {

				var lingueeLookup = "http://www.linguee.com/english-spanish/search?source=auto&query=\"" 
					+ System.Net.WebUtility.UrlEncode (Original) + "\"";

				client.Navigate ().GoToUrl (lingueeLookup);

				// This elements are difficult to remove for output selectively,
				// so I'm just removing them all from the page.
				client.ExecuteScript ("$('.source_url_spacer').remove()");
				client.ExecuteScript ("$('.source_url').remove()");
				client.ExecuteScript ("$('.behindLinkDiv').remove()");

				foreach (var example in client.FindElementsByCssSelector("tbody.examples tr")) {

					var elements = example.FindElements (By.TagName ("td")).ToArray ();
					var sample = elements[0].Text;
					var translation = elements[1].Text;

					results [sample] = translation;
				}
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

