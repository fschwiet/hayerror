using System;
using System.Collections.Generic;

namespace monarquia
{
	public class CannedData
	{
		public enum Timeframe {
			Now,
			Frequency,
			NearFuture,
			PastPreterite, //  Preterite
			PastImperfect,
			NotImplemented
		}

		public CannedData ()
		{
			AddVerbEnding ("beber", "leche");
			AddVerbEnding ("beber", "agua");
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

			AddTimeframeExpression (Timeframe.NotImplemented, "");

			AddTimeframeExpression (Timeframe.Now, "");
			AddTimeframeExpression (Timeframe.Now, "ahora");
			AddTimeframeExpression (Timeframe.Now, "ahora mismo");
			AddTimeframeExpression (Timeframe.Now, "hoy");

			AddTimeframeExpression (Timeframe.Frequency, "a menudo");  // not sure
			AddTimeframeExpression (Timeframe.Frequency, "de vez en cuando");  // not sure
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
			AddTimeframeExpression (Timeframe.Frequency, "siempre");

			AddTimeframeExpression (Timeframe.NearFuture, "al mediodía");
			AddTimeframeExpression (Timeframe.NearFuture, "esta semana");
			AddTimeframeExpression (Timeframe.NearFuture, "esta mes");

			AddTimeframeExpression (Timeframe.PastPreterite, "a esa hora");
			AddTimeframeExpression (Timeframe.PastPreterite, "a las cuatro");
			AddTimeframeExpression (Timeframe.PastPreterite, "anoche");
			AddTimeframeExpression (Timeframe.PastPreterite, "anteanoche");
			AddTimeframeExpression (Timeframe.PastPreterite, "anteayer");
			AddTimeframeExpression (Timeframe.PastPreterite, "ayer");
			AddTimeframeExpression (Timeframe.PastPreterite, "de pronto");
			AddTimeframeExpression (Timeframe.PastPreterite, "de repente");
			AddTimeframeExpression (Timeframe.PastPreterite, "ayer por la mañana");
			AddTimeframeExpression (Timeframe.PastPreterite, "ayer por la noche");
			AddTimeframeExpression (Timeframe.PastPreterite, "ayer por la tarde");
			AddTimeframeExpression (Timeframe.PastPreterite, "el año pasado");
			AddTimeframeExpression (Timeframe.PastPreterite, "el lunes pasado");
			AddTimeframeExpression (Timeframe.PastPreterite, "el mes pasado");
			AddTimeframeExpression (Timeframe.PastPreterite, "en ese instante");
			AddTimeframeExpression (Timeframe.PastPreterite, "en ese momento");
			AddTimeframeExpression (Timeframe.PastPreterite, "hace diez años");
			AddTimeframeExpression (Timeframe.PastPreterite, "hoy por la mañana");
			AddTimeframeExpression (Timeframe.PastPreterite, "la semana pasada");

			AddTimeframeExpression (Timeframe.PastPreterite, "entonces");  // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "finalmente");  // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "por fin");  // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "por primera vez");  // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "un día");  // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "una noche");  // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "una vez");  // ambiguous

			/*
			AddTimeframeExpression (Timeframe.PastPreterite, "con frecuencia"); // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "constantemente"); // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "de costumbre"); // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "frecuentemente"); // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "generalmente"); // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "normalmente"); // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "por lo general"); // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "siempre"); // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "con frecuencia"); // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "con frecuencia"); // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "con frecuencia"); // ambiguous

*/

			AddTimeframeExpression (Timeframe.PastImperfect, "a veces");
			AddTimeframeExpression (Timeframe.PastImperfect, "algunas veces");
			AddTimeframeExpression (Timeframe.PastImperfect, "casi nunca");
			AddTimeframeExpression (Timeframe.PastImperfect, "casi siempre");
			AddTimeframeExpression (Timeframe.PastImperfect, "de vez en cuando");
			AddTimeframeExpression (Timeframe.PastImperfect, "generalmente");
			AddTimeframeExpression (Timeframe.PastImperfect, "mientras");
			AddTimeframeExpression (Timeframe.PastImperfect, "muchas veces");
			AddTimeframeExpression (Timeframe.PastImperfect, "por lo general");
			AddTimeframeExpression (Timeframe.PastImperfect, "rara vez");
			AddTimeframeExpression (Timeframe.PastImperfect, "siempre");
			AddTimeframeExpression (Timeframe.PastImperfect, "todas las mañanas");
			AddTimeframeExpression (Timeframe.PastImperfect, "todos los años");
			AddTimeframeExpression (Timeframe.PastImperfect, "todos los días");
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

		public IEnumerable<string> GetTimeframeExpressions(Timeframe timeframe) {

			return TimeExpressions [timeframe];
		}
	}
}

