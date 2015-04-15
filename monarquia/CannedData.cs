using System;
using System.Collections.Generic;

namespace monarquia
{
	public class CannedData
	{
		public CannedData ()
		{
			AddVerbEnding ("beber", "leche");
			AddVerbEnding ("comer", "fajitas");
			AddVerbEnding ("hablar", "a la reportera");
			AddVerbEnding ("hablar", "con él");
			AddVerbEnding ("preparar", "la cena");
			AddVerbEnding ("subir", "la escalera");
			AddVerbEnding ("sumar", "la cuenta");
			AddVerbEnding ("temer", "a los críticos");
		}

		void AddVerbEnding(string verbInfinitive, string ending) {
		
			if (!AllVerbEndings.ContainsKey (verbInfinitive)) {
				AllVerbEndings.Add (verbInfinitive, new List<string> ());
			}

			AllVerbEndings [verbInfinitive].Add (ending);
		}

		Dictionary<string, List<string>> AllVerbEndings = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);

		public IEnumerable<string> GetVerbEndings(string verbInfinitive) {

			if (!AllVerbEndings.ContainsKey (verbInfinitive)) {
				return new string[] { "" };
			}

			return AllVerbEndings [verbInfinitive];
		}
	}
}

