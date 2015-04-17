using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ManyConsole;
using Newtonsoft.Json.Linq;

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

			using (var client = new WebClient ()) {

				var query = "http://mymemory.translated.net/api/get?langpair=es|en&q=" + WebUtility.UrlEncode (Original);

				var translatedText = DownloadTranslation (client, query);

				Console.WriteLine (translatedText);
			}

			return 0;
		}

		public static string DownloadTranslation (WebClient client, string query)
		{
			var resultText = client.DownloadString (query);
			var result = JObject.Parse (resultText);
			var responseData = result.GetValue ("responseData") as JObject;
			var translatedText = (responseData.GetValue ("translatedText") as JValue).ToString ();
			return translatedText;
		}
	}
}

