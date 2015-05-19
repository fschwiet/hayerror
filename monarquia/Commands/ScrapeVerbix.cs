using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using ManyConsole;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace monarquia
{
	public class ScrapeVerbix : ConsoleCommand
	{
		public string TargetDirectory;
		public string[] Words;
		public bool SkipExisting = false;

		public ScrapeVerbix ()
		{
			this.IsCommand ("scrape-verbix");
			this.HasAdditionalArguments (2, " <outputDirectory> <filename>");
			this.HasOption ("s", "Skip verbs that already verb a .conjugation.txt file.", v => SkipExisting = true);
		}

		public override int? OverrideAfterHandlingArgumentsBeforeRun (string[] remainingArguments)
		{
			TargetDirectory = Path.GetFullPath(remainingArguments [0]);

			if (!Directory.Exists (TargetDirectory)) {
				Console.WriteLine ("Creating directory: " + TargetDirectory);

				Directory.CreateDirectory (TargetDirectory);
			}

			Words = File.ReadAllLines (remainingArguments [1]).Select (w => w.Trim ()).ToArray();

			return base.OverrideAfterHandlingArgumentsBeforeRun (remainingArguments);
		}

		public override int Run (string[] remainingArguments)
		{
			var options = new ChromeOptions();
			var service = ChromeDriverService.CreateDefaultService();
			service.SuppressInitialDiagnosticInformation = true;

			using (var driver = new ChromeDriver (service, options))
				foreach (var word in Words) {

					var conjugationFileTarget = Path.Combine (TargetDirectory, word + ".verbix.txt");

					if (SkipExisting && File.Exists (conjugationFileTarget)) {
						continue;
					}

					driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));

					driver.Navigate ().GoToUrl ("http://conjugator.reverso.net/conjugation-english-verb-" + word + ".html");

					var infiniteElements = driver.FindElementsByCssSelector ("[title='Existing infinitive']");

					if (!infiniteElements.Any ())
						throw new Exception ("Verb not recognized: " + word);

					if (infiniteElements.Single ().Text != word)
						throw new Exception ("Verb not infinitive: " + word);

					driver.Navigate ().GoToUrl ("http://www.verbix.com/webverbix/English/" + word + ".html");

					File.WriteAllText (conjugationFileTarget, driver.PageSource);
				}

			return -1;
		}
	}
}

