using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{
	public class BigCannedData : BetterCannedData
	{
		public BigCannedData (DataLoader dataLoader) : base(dataLoader)
		{
			HasEnglishTranslation ("comer", "eat");
			HasEnglishTranslation ("gritar", "shout");
			HasEnglishTranslation ("hablar", "talk");
			HasEnglishTranslation ("preparar", "prepare");
			HasEnglishTranslation ("subir", "climb");
			HasEnglishTranslation ("sumar", "sum");
			HasEnglishTranslation ("temer", "fear");
			HasEnglishTranslation ("tener", "have");

			AddVerbEnding ("comer", new CannedTranslation("fajitas", "fajitas"));

			AddVerbEnding ("hablar", new CannedTranslation("a la reportera", "to the reporter"));
			AddVerbEnding ("hablar", new CannedTranslation("con él", "to him"));
			AddVerbEnding ("preparar", new CannedTranslation("la cena", "the dinner"));
		
			AddVerbEnding ("subir", new CannedTranslation("la escalera", "the stairs"));
			AddVerbEnding ("sumar", new CannedTranslation("la cuenta", "the bill"));
			AddVerbEnding ("temer", new CannedTranslation("a los críticos", "the critics"));
			//AddVerbEnding ("tener", "frío");   // to be cold
			//AddVerbEnding ("tener", "hambre"); // to be hungry
			//AddVerbEnding ("tener", "miedo");  // to have fear
			//AddVerbEnding ("tener", "razón");  // to be right
			//AddVerbEnding ("tener", "sed");  // to be thirsty
			//AddVerbEnding ("tener", "prisa");  // to be in a hurry
			//AddVerbEnding ("tener", "culpa");  // to be at fault


			var timeframeExpressions = new [] {
				// Now
				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("ahora", "now")),
				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("ahora mismo", "right now")),
				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("hoy", "today")),
				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("a este minuto", "at this minute")),

				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("ahora", "now")),
				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("ahora mismo", "right now")),
				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("hoy", "today")),
				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("a este minuto", "at this minute")),

				// Usually? Frequency?
				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("a menudo", "often")),  // not sure
				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("de vez en cuando", "from time to time")),  // not sure
				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("en general", "in general")),
				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("los lunes", "the Mondays")),
				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("los martes", "the Tuesdays")),
				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("los miercoles", "the Wednesdays")),
				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("los jueves", "the Thursdays")),
				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("los viernes", "the Fridays")),
				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("los sábados", "the Saturdays")),
				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("los domingos", "the Sundays")),
				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("los fines de semanas", "the weekends")),
				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("normalmente", "normally")),

				// Near future
				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("al mediodía","at noon")),
				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("esta semana","this week")),
				AddTimeframeExpression (Conjugation.Present, new CannedTranslation("esta mes", "this month")),

				AddTimeframeExpression (Conjugation.PastPreterite, new CannedTranslation("a las cuatro", "at four o'clock")),
				AddTimeframeExpression (Conjugation.PastPreterite, new CannedTranslation("anoche", "last night")),
				AddTimeframeExpression (Conjugation.PastPreterite, new CannedTranslation("anteanoche", "the night before last")),
				AddTimeframeExpression (Conjugation.PastPreterite, new CannedTranslation("anteayer", "the day before yesterday")),
				AddTimeframeExpression (Conjugation.PastPreterite, new CannedTranslation("ayer por la mañana", "yesterday morning")),
				AddTimeframeExpression (Conjugation.PastPreterite, new CannedTranslation("ayer por la noche", "yesterday night")),
				AddTimeframeExpression (Conjugation.PastPreterite, new CannedTranslation("ayer por la tarde", "yesterday afternoon")),
				AddTimeframeExpression (Conjugation.PastPreterite, new CannedTranslation("el año pasado", "last year")),
				AddTimeframeExpression (Conjugation.PastPreterite, new CannedTranslation("el lunes pasado","last Monday")),
				AddTimeframeExpression (Conjugation.PastPreterite, new CannedTranslation("el mes pasado","last month")),
				AddTimeframeExpression (Conjugation.PastPreterite, new CannedTranslation("en ese instante","at that instant")),
				AddTimeframeExpression (Conjugation.PastPreterite, new CannedTranslation("en ese momento","at that moment")),
				AddTimeframeExpression (Conjugation.PastPreterite, new CannedTranslation("la semana pasada", "last week")),


				AddTimeframeExpression (Conjugation.PastImperfect, new CannedTranslation("a veces", "at times")),
				AddTimeframeExpression (Conjugation.PastImperfect, new CannedTranslation("algunas veces", "sometimes")),
				// AddTimeframeExpression (Conjugation.PastImperfect, new CannedTranslation("casi nunca", )),
				AddTimeframeExpression (Conjugation.PastImperfect, new CannedTranslation("casi siempre", "almost always")),
				AddTimeframeExpression (Conjugation.PastImperfect, new CannedTranslation("de vez en cuando", "from time to time")),
				AddTimeframeExpression (Conjugation.PastImperfect, new CannedTranslation("muchas veces", "many times")),
				//AddTimeframeExpression (Conjugation.PastImperfect, new CannedTranslation("por lo general", )),
				//AddTimeframeExpression (Conjugation.PastImperfect, new CannedTranslation("rara vez", )),

				AddTimeframeExpression (Conjugation.Future, new CannedTranslation("a la una", "at one o'clock")),
				AddTimeframeExpression (Conjugation.Future, new CannedTranslation("de aquí a dos días", "two days from now")),
				AddTimeframeExpression (Conjugation.Future, new CannedTranslation("el lunes que viene", "next monday")),
				AddTimeframeExpression (Conjugation.Future, new CannedTranslation("en una semana", "in one week")),
				AddTimeframeExpression (Conjugation.Future, new CannedTranslation("esta noche", "tonight")),
				AddTimeframeExpression (Conjugation.Future, new CannedTranslation("esta primavera", "this spring")),
				AddTimeframeExpression (Conjugation.Future, new CannedTranslation("luego", "later")),
				AddTimeframeExpression (Conjugation.Future, new CannedTranslation("mañana", "tomorrow")),
				AddTimeframeExpression (Conjugation.Future, new CannedTranslation("mañana por la mañana", "tomorrow morning")),
				AddTimeframeExpression (Conjugation.Future, new CannedTranslation("mañana por la tarde", "tomorrow evening")),
				AddTimeframeExpression (Conjugation.Future, new CannedTranslation("mañana por la noche", "tomorrow night")),
				AddTimeframeExpression (Conjugation.Future, new CannedTranslation("pasado mañana", "the day after tomorrow")),

				AddTimeframeExpression (Conjugation.Conditional, new CannedTranslation("ahora mismo", "right now")),
				AddTimeframeExpression (Conjugation.Conditional, new CannedTranslation("por supuesto", "of course")),
			};

			foreach(var timeframeExpression in timeframeExpressions) {
				AddTimeframeExpression (timeframeExpression);
			}




			/*
			 
			// excluding siempre since amibguous and google mistranslates 'Siempre usted está en frente'

			AddTimeframeExpression (Conjugation.PastPreterite, "entonces");  // ambiguous
			AddTimeframeExpression (Conjugation.PastPreterite, "finalmente");  // ambiguous
			AddTimeframeExpression (Conjugation.PastPreterite, "por fin");  // ambiguous
			AddTimeframeExpression (Conjugation.PastPreterite, "por primera vez");  // ambiguous
			AddTimeframeExpression (Conjugation.PastPreterite, "un día");  // ambiguous
			AddTimeframeExpression (Conjugation.PastPreterite, "una noche");  // ambiguous
			AddTimeframeExpression (Conjugation.PastPreterite, "una vez");  // ambiguous

			AddTimeframeExpression (Timeframe.PastPreterite, "con frecuencia"); // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "constantemente"); // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "de costumbre"); // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "frecuentemente"); // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "generalmente"); // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "normalmente"); // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "por lo general"); // ambiguous

			AddTimeframeExpression (Conjugation.PastImperfect, "mientras");  // also a conjugation

			*/
		}
	}
}

