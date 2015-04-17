using System;
using System.Collections.Generic;

namespace monarquia
{
	public class CannedData
	{
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


			// Now
			AddTimeframeExpression (Verb.Conjugation.Present, "");
			AddTimeframeExpression (Verb.Conjugation.Present, "ahora");
			AddTimeframeExpression (Verb.Conjugation.Present, "ahora mismo");
			AddTimeframeExpression (Verb.Conjugation.Present, "hoy");

			// Usually? Frequency?
			AddTimeframeExpression (Verb.Conjugation.Present, "a menudo");  // not sure
			AddTimeframeExpression (Verb.Conjugation.Present, "de vez en cuando");  // not sure
			AddTimeframeExpression (Verb.Conjugation.Present, "en general");
			AddTimeframeExpression (Verb.Conjugation.Present, "los lunes");
			AddTimeframeExpression (Verb.Conjugation.Present, "los martes");
			AddTimeframeExpression (Verb.Conjugation.Present, "los miercoles");
			AddTimeframeExpression (Verb.Conjugation.Present, "los jueves");
			AddTimeframeExpression (Verb.Conjugation.Present, "los viernes");
			AddTimeframeExpression (Verb.Conjugation.Present, "los sábados");
			AddTimeframeExpression (Verb.Conjugation.Present, "los domingos");
			AddTimeframeExpression (Verb.Conjugation.Present, "los fines de semanas");
			AddTimeframeExpression (Verb.Conjugation.Present, "normalmente");
			AddTimeframeExpression (Verb.Conjugation.Present, "siempre");

			// Near future
			AddTimeframeExpression (Verb.Conjugation.Present, "al mediodía");
			AddTimeframeExpression (Verb.Conjugation.Present, "esta semana");
			AddTimeframeExpression (Verb.Conjugation.Present, "esta mes");

			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "a esa hora");
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "a las cuatro");
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "anoche");
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "anteanoche");
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "anteayer");
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "ayer");
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "de pronto");
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "de repente");
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "ayer por la mañana");
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "ayer por la noche");
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "ayer por la tarde");
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "el año pasado");
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "el lunes pasado");
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "el mes pasado");
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "en ese instante");
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "en ese momento");
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "hace diez años");
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "hoy por la mañana");
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "la semana pasada");

			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "entonces");  // ambiguous
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "finalmente");  // ambiguous
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "por fin");  // ambiguous
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "por primera vez");  // ambiguous
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "un día");  // ambiguous
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "una noche");  // ambiguous
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "una vez");  // ambiguous

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

			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "a veces");
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "algunas veces");
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "casi nunca");
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "casi siempre");
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "de vez en cuando");
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "generalmente");
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "mientras");
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "muchas veces");
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "por lo general");
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "rara vez");
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "siempre");
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "todas las mañanas");
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "todos los años");
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "todos los días");

			AddTimeframeExpression (Verb.Conjugation.PresentPerfect, "");

			AddTimeframeExpression (Verb.Conjugation.Future, "");
			AddTimeframeExpression (Verb.Conjugation.Future, "a la una");
			AddTimeframeExpression (Verb.Conjugation.Future, "de aquí a dos días");
			AddTimeframeExpression (Verb.Conjugation.Future, "el lunes que vien");
			AddTimeframeExpression (Verb.Conjugation.Future, "en una semana");
			AddTimeframeExpression (Verb.Conjugation.Future, "esta noche");
			AddTimeframeExpression (Verb.Conjugation.Future, "esta primavera");
			AddTimeframeExpression (Verb.Conjugation.Future, "luego");
			AddTimeframeExpression (Verb.Conjugation.Future, "mañana");
			AddTimeframeExpression (Verb.Conjugation.Future, "mañana por la mañana");
			AddTimeframeExpression (Verb.Conjugation.Future, "mañana por la tarde");
			AddTimeframeExpression (Verb.Conjugation.Future, "mañana por la noche");
			AddTimeframeExpression (Verb.Conjugation.Future, "pasado mañana");

			AddTimeframeExpression (Verb.Conjugation.Conditional, "");
			AddTimeframeExpression (Verb.Conjugation.Conditional, "ahora mismo");
			AddTimeframeExpression (Verb.Conjugation.Conditional, "por supuesto");
		}

		Dictionary<string, List<string>> AllVerbEndings = new Dictionary<string, List<string>>(StringComparer.InvariantCultureIgnoreCase);
		Dictionary<Verb.Conjugation, List<string>> TimeExpressions = new Dictionary<Verb.Conjugation, List<string>>();

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

		void AddTimeframeExpression(Verb.Conjugation conjugation, params string[] expressions) {

			if (!TimeExpressions.ContainsKey (conjugation)) {
				TimeExpressions [conjugation] = new List<string> ();
			}

			foreach (var expression in expressions) {
				TimeExpressions[conjugation].Add (expression);
			}
		}

		public IEnumerable<string> GetTimeframeExpressions(Verb.Conjugation conjugation) {
			
			return TimeExpressions [conjugation];
		}
	}
}

