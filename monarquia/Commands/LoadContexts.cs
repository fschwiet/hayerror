using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ManyConsole;
using System.Net;

namespace monarquia
{
	public class LoadContexts : ConsoleCommand
	{
		public string DataDirectory;

		public LoadContexts ()
		{
			this.IsCommand ("load-contexts");
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
			List<string> allContexts = new List<string> ();

			foreach (var file in Directory.GetFiles(DataDirectory)) {

				if (!file.EndsWith (".definition.txt")) {
					continue;
				}

				CsQuery.CQ document = File.ReadAllText (file);

				var contexts = document[".dictionary-entry .context"].
					Select(c => WebUtility.HtmlDecode(c.InnerText.Trim().TrimStart('(').TrimEnd(')')));

				allContexts.AddRange (contexts);
			}

			Console.WriteLine (allContexts.Count ());
			var counted = from s in allContexts group s by s into g 
				select new {
					context = g.Key,
					count = g.Count()
				};

			counted = counted.OrderBy (g => g.count);

			foreach (var entry in counted) {
				Console.WriteLine ("{0} - {1}", entry.context, entry.count);
			}

			return 0;
		}
	}
}

