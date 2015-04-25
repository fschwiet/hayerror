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
	public class ScrapeDictionary : ConsoleCommand
	{
		public string TargetDirectory;
		public string[] Words;
		public bool IsSpanish = true;

		public ScrapeDictionary ()
		{
			this.IsCommand ("scrape-dictionary");
			this.HasAdditionalArguments (2, " <outputDirectory> <filename>");
			this.HasOption ("e", "Flag indicating the term is English", v => IsSpanish = false);
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

					driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));

					driver.Navigate ().GoToUrl ("http://spanishdict.com/translate/" + word);

					var languageLink = IsSpanish ? "a[href='#translate-es']" : "a[href='#translate-en']";

					driver.FindElementByCssSelector (languageLink).Click ();

					var definitionFileTarget = Path.Combine (TargetDirectory, word + ".definition.txt");
					File.WriteAllText (definitionFileTarget, driver.PageSource);

					var conjugationLink = driver.FindElementsByLinkText ("Conjugation").FirstOrDefault ();

					if (conjugationLink != null) {
						driver.Navigate ().GoToUrl ("http://spanishdict.com/conjugate/" + word);

						//  use FindElement to force a wait for the page to load
						driver.FindElementByCssSelector (".vtable-label");

						var conjugationFileTarget = Path.Combine (TargetDirectory, word + ".conjugation.txt");
						File.WriteAllText (conjugationFileTarget, driver.PageSource);
					}
				}

			return -1;
		}
	}
}


