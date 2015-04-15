using System;
using System.Collections.Generic;
using System.Linq;
using ManyConsole;

namespace monarquia
{
	public class DrillVerb : ConsoleCommand
	{
		public string Verb;

		public DrillVerb ()
		{
			this.IsCommand ("drill-verb", "Generate phrases for a particular verb");
			this.HasAdditionalArguments (1, " <verb infinitive>");
		}

		public override int? OverrideAfterHandlingArgumentsBeforeRun (string[] remainingArguments)
		{
			Verb = remainingArguments [0].ToLower ();

			return base.OverrideAfterHandlingArgumentsBeforeRun (remainingArguments);
		}

		public override int Run (string[] remainingArguments)
		{
			var generator = new EspanolGenerator ("./data");

			var results = generator.GetForVerb (Verb, true);

			foreach (var result in results) {
				Console.WriteLine (result);
			}

			return 0;
		} 
	}
}

