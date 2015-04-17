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
			var query = "http://mymemory.translated.net/api/get?langpair=es|en&q=" + WebUtility.UrlEncode (Original);

			using (var client = new WebClient ()) {
				var resultText = client.DownloadString (query);

				var result = JObject.Parse (resultText);
				var responseData = result.GetValue ("responseData") as JObject;
				var translatedText = responseData.GetValue ("translatedText") as JValue;
				Console.WriteLine (translatedText.ToString ());
			}

			return 0;
		}
	}
}

