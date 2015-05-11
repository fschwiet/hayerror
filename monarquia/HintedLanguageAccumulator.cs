using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace monarquia
{
	public class HintedLanguageAccumulator
	{
		class Record
		{
			public string Text;
			public IEnumerable<string> Hints;
		
			public string AsFinalText(bool capitolize) {

				var text = Text;
				if (capitolize) {
					text = text[0].ToString ().ToUpper () + text.Substring(1);
				}

				if (!Hints.Any ()) {
					return text;
				}

				var hintText = "(" + string.Join (", ", Hints) + ")";

				if (!Text.Any ()) {
					return hintText;
				}

				return hintText + " " + text;
			}
		}

		List<Record> records = new List<Record> ();

		public void Add(string value, IEnumerable<string> hints = null)
		{
			hints = hints ?? new string[0];

			if (value.Length == 0) {
				if (hints.Any ())
					throw new Exception ("Attempted to accumulate an empty language result with a tag.");

				return;
			}

			records.Add (new Record { Text = value, Hints = hints });
		}

		public void ApplyContraction(string start, string end, string substitution) {
			var modified = records.ToArray ();

			for (var i = 0; i < modified.Length - 2; i++) {
				if (modified [i].Text == start && modified [i + 1].Text == end) {
					modified [i].Text = substitution;
					modified [i].Hints = modified [i].Hints.Concat (modified [i + 1].Hints);
				
					var trimmed = modified.ToList ();
					trimmed.RemoveAt (i + 1);
					modified = trimmed.ToArray ();
				}
			}

			records = modified.ToList();
		}

		public void ApplyTransform(string target, Func<string, bool> nextTest, string substitution) {
			var modified = records.ToArray ();

			for (var i = 0; i < modified.Length - 1; i++) {
				if (modified [i].Text == target && nextTest(modified [i + 1].Text)) {
					modified [i].Text = substitution;
				}
			}

			records = modified.ToList();
		}

		public string GetResult()
		{
			StringBuilder result = new StringBuilder ();
			bool haveCapital = false;
			var separator = "";

			foreach (var record in records) {

				result.Append (separator);
				separator = " ";

				if (!haveCapital && record.Text.Any ()) {
					result.Append (record.AsFinalText (true));
					haveCapital = true;
				} else {
					result.Append (record.AsFinalText (false));
				}
			}

			result.Append (".");

			return result.ToString ();
		}
	}
	
}
