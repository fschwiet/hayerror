using System;
using System.IO;
using System.Linq;

using ManyConsole;
using System.Net;
using System.Collections.Generic;

namespace monarquia
{
	public class LoadAllExampleTexts : ConsoleCommand
	{
		public string DataDirectory;

		public LoadAllExampleTexts ()
		{
			this.IsCommand ("load-examples");
			this.HasAdditionalArguments (1, " <data-dir>");
		}

		public override int? OverrideAfterHandlingArgumentsBeforeRun (string[] remainingArguments)
		{
			DataDirectory = remainingArguments [0];

			if (!Directory.Exists (DataDirectory))
				throw new ConsoleHelpAsException ("Directory not found.");

			return base.OverrideAfterHandlingArgumentsBeforeRun (remainingArguments);
		}

		public override int Run (string[] remainingArguments)
		{
			List < Tuple < string,string >> results = new List<Tuple<string, string>> ();

			foreach(var file in Directory.GetFiles(remainingArguments[0])) {

				if (!file.EndsWith (".definition.txt")) {
					continue;
				}

				CsQuery.CQ document = File.ReadAllText (file);

				var examples = document[".dictionary-neodict-example"];

				foreach (CsQuery.IDomObject example in examples) {
					var separator = example.ChildNodes.Single(n => n.Classes.Contains("icon-dash"));

					var first = WebUtility.HtmlDecode(separator.PreviousSibling.InnerText);
					var second = WebUtility.HtmlDecode(separator.NextSibling.InnerText);

					results.Add (new Tuple<string, string> (first, second));
				}
			}

			results = results.OrderBy (t => -t.Item1.Length).ToList();

			foreach (var result in results) {
				Console.WriteLine (result.Item1 + " -- " + result.Item2);
			}


			return 0;
		}

	}
}

