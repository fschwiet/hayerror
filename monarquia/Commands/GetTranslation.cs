using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ManyConsole;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium.Chrome;

namespace monarquia
{
	public class GetTranslation : ConsoleCommand
	{
		public string Original;

		public GetTranslation ()
		{
			this.IsCommand ("translate", "Translate a Spanish expression to English.");
			this.HasAdditionalArguments(1, " <expression>");
		}

		public override int? OverrideAfterHandlingArgumentsBeforeRun (string[] remainingArguments)
		{
			Original = remainingArguments [0];

			return base.OverrideAfterHandlingArgumentsBeforeRun (remainingArguments);
		}

		public override int Run (string[] remainingArguments)
		{
			var inputs = new [] { Original};

			var results = DownloadTranslationsFromGoogle (inputs);

			foreach (var result in results) {
				Console.WriteLine (result);
			}

			return 0;
		}

		public static Dictionary<string, string> DownloadTranslationsFromGoogle (string[] inputs)
		{
			var results = new Dictionary<string, string>();

			using (var client = new ChromeDriver ()) {

				foreach (var input in inputs) {

					client.Navigate ().GoToUrl ("https://translate.google.com/#es/en/" + WebUtility.UrlEncode(input));

					var result = client.FindElementByCssSelector("#result_box").Text;

					results.Add (input, result);
				}
			}
			return results;
		}

		static List<string> DownloadTranslationsFromMyMemory (string[] inputs)
		{
			var results = new List<string> ();
			using (var client = new WebClient ()) {
				foreach (var input in inputs) {
					results.Add (DownloadTranslation (client, input));
				}
			}
			return results;
		}

		public static string DownloadTranslation (WebClient client, string source)
		{
			var query = "http://mymemory.translated.net/api/get?langpair=es|en&q=" + WebUtility.UrlEncode (source);
			var resultText = client.DownloadString (query);
			var result = JObject.Parse (resultText);
			var responseData = result.GetValue ("responseData") as JObject;
			var translatedText = (responseData.GetValue ("translatedText") as JValue).ToString ();
			return translatedText;
		}
	}
}

