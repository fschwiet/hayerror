using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace monarquia
{
	public class LingueeGenerator
	{
		public static Dictionary<string, string> DoLingueeLookup (ChromeDriver chromeDriver, string target)
		{
			var lingueeLookup = "http://www.linguee.com/english-spanish/search?source=auto&query=\"" + System.Net.WebUtility.UrlEncode (target) + "\"";

			try {
				Dictionary<string, string> results = new Dictionary<string, string> ();

				chromeDriver.Navigate ().GoToUrl (lingueeLookup);

				// This elements are difficult to remove for output selectively,
				// so I'm just removing them all from the page.
				chromeDriver.ExecuteScript ("$('.source_url_spacer').remove()");
				chromeDriver.ExecuteScript ("$('.source_url').remove()");
				chromeDriver.ExecuteScript ("$('.behindLinkDiv').remove()");

				foreach (var example in chromeDriver.FindElementsByCssSelector ("tbody.examples tr")) {
					var elements = example.FindElements (By.TagName ("td")).ToArray ();
					var sample = elements [0].Text;
					var translation = elements [1].Text;
					results [sample] = translation;
				}

				return results;
			}
			catch(Exception e) {
				throw new Exception ("Linguee failure for url: " + lingueeLookup, e);
			}
		}
	}
}

