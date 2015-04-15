using System;
using System.Collections.Generic;

namespace monarquia
{
	public class CannedData
	{
		public enum Timeframe {
			Now,
			Frequency,
			NearFuture
		}

		public CannedData ()
		{
			AddVerbEnding ("beber", "leche");
			AddVerbEnding ("comer", "fajitas");
			AddVerbEnding ("dar", "un abrazo");
			AddVerbEnding ("dar", "gritos");
			//  AddVerbEnding ("dar", "la una");  ->  only clocks can strike one (to indicate a time)
			AddVerbEnding ("hablar", "a la reportera");
			AddVerbEnding ("hablar", "con él");
			AddVerbEnding ("preparar", "la cena");
			AddVerbEnding ("subir", "la escalera");
			AddVerbEnding ("sumar", "la cuenta");
			AddVerbEnding ("temer", "a los críticos");
			AddVerbEnding ("tener", "frío");   // to be cold
			AddVerbEnding ("tener", "hambre"); // to be hungry
			AddVerbEnding ("tener", "miedo");  // to have fear
			AddVerbEnding ("tener", "razón");  // to be right
			AddVerbEnding ("tener", "sed");  // to be thirsty
			AddVerbEnding ("tener", "prisa");  // to be in a hurry
			AddVerbEnding ("tener", "culpa");  // to be at fault

			AddTimeframeExpression (Timeframe.Now, "ahora");
			AddTimeframeExpression (Timeframe.Now, "ahora mismo");
			AddTimeframeExpression (Timeframe.Now, "hoy");

			AddTimeframeExpression (Timeframe.Frequency, "a menudo");
			AddTimeframeExpression (Timeframe.Frequency, "de vez en cuando");
			AddTimeframeExpression (Timeframe.Frequency, "de vez en cuando");
			AddTimeframeExpression (Timeframe.Frequency, "en general");
			AddTimeframeExpression (Timeframe.Frequency, "los lunes");
			AddTimeframeExpression (Timeframe.Frequency, "los martes");
			AddTimeframeExpression (Timeframe.Frequency, "los miercoles");
			AddTimeframeExpression (Timeframe.Frequency, "los jueves");
			AddTimeframeExpression (Timeframe.Frequency, "los viernes");
			AddTimeframeExpression (Timeframe.Frequency, "los sábados");
			AddTimeframeExpression (Timeframe.Frequency, "los domingos");
			AddTimeframeExpression (Timeframe.Frequency, "los fines de semanas");
			AddTimeframeExpression (Timeframe.Frequency, "normalmente");
			AddTimeframeExpression (Timeframe.Frequency, "nunca");
			AddTimeframeExpression (Timeframe.Frequency, "siempre");

			AddTimeframeExpression (Timeframe.NearFuture, "al mediodía");
			AddTimeframeExpression (Timeframe.NearFuture, "esta semana");
			AddTimeframeExpression (Timeframe.NearFuture, "esta mes");
		}

		Dictionary<string, List<string>> AllVerbEndings = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);
		Dictionary<Timeframe, List<string>> TimeExpressions = new Dictionary<Timeframe, List<string>>();

		void AddVerbEnding(string verbInfinitive, string ending) {
		
			if (!AllVerbEndings.ContainsKey (verbInfinitive)) {
				AllVerbEndings.Add (verbInfinitive, new List<string> ());
			}

			AllVerbEndings [verbInfinitive].Add (ending);
		}

		public IEnumerable<string> GetVerbEndings(string verbInfinitive) {

			if (!AllVerbEndings.ContainsKey (verbInfinitive)) {
				return new string[] { "" };
			}

			return AllVerbEndings [verbInfinitive];
		}

		void AddTimeframeExpression(Timeframe timeframe, params string[] expressions) {

			if (!TimeExpressions.ContainsKey (timeframe)) {
				TimeExpressions [timeframe] = new List<string> ();
			}

			foreach (var expression in expressions) {
				TimeExpressions[timeframe].Add (expression);
			}
		}
	}
}

