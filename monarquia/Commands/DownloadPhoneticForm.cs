using System;
using System.Collections.Specialized;
using System.Linq;
using ManyConsole;


namespace monarquia
{
	public class DownloadPhoneticForm : ConsoleCommand
	{
		public string Expression;

		public DownloadPhoneticForm ()
		{
			this.IsCommand ("download-phonetic-english");
			this.AllowsAnyAdditionalArguments (" <expression*>");
		}

		public override int? OverrideAfterHandlingArgumentsBeforeRun (string[] remainingArguments)
		{
			Expression = String.Join (" ", remainingArguments);
			return null;
		}

		public override int Run (string[] remainingArguments)
		{
			var phoneticData = new CachedPhoneticData ("./data");

			var resultText = phoneticData.GetEnglishPhonetics (Expression);

			Console.WriteLine ("{0} - {1}", Expression, resultText);

			return 0;
		}
	}
}

