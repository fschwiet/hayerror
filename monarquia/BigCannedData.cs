using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{
	public class BetterCannedData : CannedData 
	{
		public static readonly IEnumerable<string> BetterVerbs = new string[] {
			"ser"
		};

		public BetterCannedData() {
			
			AddVerbTranslation ("ser", "is");
			AddVerbTranslation ("estar", "is");
			AddVerbTranslation ("ir", "go");

			AddTimeframeExpression (Conjugation.PastPreterite, new CannedTranslation("a esa hora", "at that hour"));
			AddTimeframeExpression (Conjugation.PastPreterite, new CannedTranslation("ayer", "yesterday"));
			AddTimeframeExpression (Conjugation.PastPreterite, new CannedTranslation("de pronto", "suddenly"));
			AddTimeframeExpression (Conjugation.PastPreterite, new CannedTranslation("de repente", "suddenly"));

			AddTimeframeExpression (Conjugation.PastImperfect, new CannedTranslation("él me dijo que", "he told me that"));
			AddTimeframeExpression (Conjugation.PastImperfect, new CannedTranslation("todas las mañanas", "every morning"));
			AddTimeframeExpression (Conjugation.PastImperfect, new CannedTranslation("todos los años", "every year"));
			AddTimeframeExpression (Conjugation.PastImperfect, new CannedTranslation("todos los días", "every day"));

			var actor = new Noun ("actor", "actriz", "actores", "actrizes").WithTranslation ("actor", "actors");
			var cook = new Noun ("cocinero", "cocinera", "cocineros", "cocineras").WithTranslation ("cook", "cooks");
			var dentist = new Noun ("dentista", "dentistas").WithTranslation ("dentist", "dentists");
			var student = new Noun ("estudiante", "estudiantes").WithTranslation ("student", "students");

			var professions = new [] { actor, cook, dentist, student };

			foreach (var profession in professions.Select(t => (Composed)t)) {
				AddVerbEnding ("ser", 
					profession.WithEnglishAlternative(
						new Article() + profession
					));
			}

			AddVerbEnding ("estar", new CannedTranslation("en frente", "in front"));
			AddVerbEnding ("estar", new CannedTranslation("al otro lado de la calle", "on the other side of the street"));
			AddVerbEnding ("estar", new Noun ("listo", "lista", "listos", "listas").WithTranslation("ready", "ready"));
			AddVerbEnding ("estar", new CannedTranslation("afuera", "outside"));

			//  English "good" is ambiguous
			//AddVerbEnding ("estar", new Noun ("bueno", "buena", "buenos", "buenas").WithTranslation("good", "good"));

			AddVerbEnding ("ir", new CannedTranslation("al cine", "to the movies"));
			AddVerbEnding ("ir", new CannedTranslation ("a la playa", "to the beach"));
			//AddVerbEnding ("ir", new CannedTranslation("a leer", "to read"));
			AddVerbEnding ("ir", new CannedTranslation("allí", "there"));
		}
	}

	public class BigCannedData : BetterCannedData
	{
		public BigCannedData ()
		{
			AddVerbTranslation ("beber", "drink");
			AddVerbTranslation ("comer", "eat");
			AddVerbTranslation ("gritar", "shout");
			AddVerbTranslation ("hablar", "talk");
			//AddVerbTranslation ("preparar", "prepare"); -> ugh, can't load spanishdictionary.com/conjugate/prepare
			AddVerbTranslation ("subir", "climb");
			AddVerbTranslation ("sumar", "sum");
			AddVerbTranslation ("temer", "fear");
			AddVerbTranslation ("tener", "have");


			AddVerbEnding ("beber", new CannedTranslation("leche", "milk"));
			AddVerbEnding ("beber", new CannedTranslation("agua", "water"));
			AddVerbEnding ("comer", new CannedTranslation("fajitas", "fajitas"));
			AddVerbEnding ("dar", new CannedTranslation("un abrazo", "a hug"));
			AddVerbEnding ("dar", new CannedTranslation("gritos", "shouts"));
			//  AddVerbEnding ("dar", "la una");  ->  only clocks can strike one (to indicate a time)

			AddVerbEnding ("hablar", new CannedTranslation("a la reportera", "to the reporter"));
			AddVerbEnding ("hablar", new CannedTranslation("con él", "to him"));
			AddVerbEnding ("preparar", new CannedTranslation("la cena", "the dinner"));
		
			AddVerbEnding ("subir", new CannedTranslation("la escalera", "the stairs"));
			AddVerbEnding ("sumar", new CannedTranslation("la cuenta", "the bill"));
			AddVerbEnding ("temer", new CannedTranslation("a los críticos", "the critics"));
			AddVerbEnding ("tener", "frío");   // to be cold
			AddVerbEnding ("tener", "hambre"); // to be hungry
			AddVerbEnding ("tener", "miedo");  // to have fear
			AddVerbEnding ("tener", "razón");  // to be right
			AddVerbEnding ("tener", "sed");  // to be thirsty
			AddVerbEnding ("tener", "prisa");  // to be in a hurry
			AddVerbEnding ("tener", "culpa");  // to be at fault


			// Now
			AddTimeframeExpression (Conjugation.Present, new CannedTranslation("", ""));
			AddTimeframeExpression (Conjugation.Present, "ahora");
			AddTimeframeExpression (Conjugation.Present, "ahora mismo");
			AddTimeframeExpression (Conjugation.Present, "hoy");
			AddTimeframeExpression (Conjugation.Present, "a este minuto");

			// Usually? Frequency?
			AddTimeframeExpression (Conjugation.Present, "a menudo");  // not sure
			AddTimeframeExpression (Conjugation.Present, "de vez en cuando");  // not sure
			AddTimeframeExpression (Conjugation.Present, "en general");
			AddTimeframeExpression (Conjugation.Present, "los lunes");
			AddTimeframeExpression (Conjugation.Present, "los martes");
			AddTimeframeExpression (Conjugation.Present, "los miercoles");
			AddTimeframeExpression (Conjugation.Present, "los jueves");
			AddTimeframeExpression (Conjugation.Present, "los viernes");
			AddTimeframeExpression (Conjugation.Present, "los sábados");
			AddTimeframeExpression (Conjugation.Present, "los domingos");
			AddTimeframeExpression (Conjugation.Present, "los fines de semanas");
			AddTimeframeExpression (Conjugation.Present, "normalmente");

			// Near future
			AddTimeframeExpression (Conjugation.Present, "al mediodía");
			AddTimeframeExpression (Conjugation.Present, "esta semana");
			AddTimeframeExpression (Conjugation.Present, "esta mes");

			AddTimeframeExpression (Conjugation.PastPreterite, "a las cuatro");
			AddTimeframeExpression (Conjugation.PastPreterite, "anoche");
			AddTimeframeExpression (Conjugation.PastPreterite, "anteanoche");
			AddTimeframeExpression (Conjugation.PastPreterite, "anteayer");
			AddTimeframeExpression (Conjugation.PastPreterite, "ayer por la mañana");
			AddTimeframeExpression (Conjugation.PastPreterite, "ayer por la noche");
			AddTimeframeExpression (Conjugation.PastPreterite, "ayer por la tarde");
			AddTimeframeExpression (Conjugation.PastPreterite, "el año pasado");
			AddTimeframeExpression (Conjugation.PastPreterite, "el lunes pasado");
			AddTimeframeExpression (Conjugation.PastPreterite, "el mes pasado");
			AddTimeframeExpression (Conjugation.PastPreterite, "en ese instante");
			AddTimeframeExpression (Conjugation.PastPreterite, "en ese momento");
			AddTimeframeExpression (Conjugation.PastPreterite, "hace diez años");
			AddTimeframeExpression (Conjugation.PastPreterite, "hoy por la mañana");
			AddTimeframeExpression (Conjugation.PastPreterite, "la semana pasada");


			/*
			 * 
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

			AddTimeframeExpression (Conjugation.PastImperfect, "a veces");
			AddTimeframeExpression (Conjugation.PastImperfect, "algunas veces");
			AddTimeframeExpression (Conjugation.PastImperfect, "casi nunca");
			AddTimeframeExpression (Conjugation.PastImperfect, "casi siempre");
			AddTimeframeExpression (Conjugation.PastImperfect, "de vez en cuando");
			AddTimeframeExpression (Conjugation.PastImperfect, "muchas veces");
			AddTimeframeExpression (Conjugation.PastImperfect, "por lo general");
			AddTimeframeExpression (Conjugation.PastImperfect, "rara vez");

			AddTimeframeExpression (Conjugation.PresentPerfect, new CannedTranslation("",""));

			AddTimeframeExpression (Conjugation.Future, "");
			AddTimeframeExpression (Conjugation.Future, "a la una");
			AddTimeframeExpression (Conjugation.Future, "de aquí a dos días");
			AddTimeframeExpression (Conjugation.Future, "el lunes que vien");
			AddTimeframeExpression (Conjugation.Future, "en una semana");
			AddTimeframeExpression (Conjugation.Future, "esta noche");
			AddTimeframeExpression (Conjugation.Future, "esta primavera");
			AddTimeframeExpression (Conjugation.Future, "luego");
			AddTimeframeExpression (Conjugation.Future, "mañana");
			AddTimeframeExpression (Conjugation.Future, "mañana por la mañana");
			AddTimeframeExpression (Conjugation.Future, "mañana por la tarde");
			AddTimeframeExpression (Conjugation.Future, "mañana por la noche");
			AddTimeframeExpression (Conjugation.Future, "pasado mañana");

			AddTimeframeExpression (Conjugation.Conditional, "");
			AddTimeframeExpression (Conjugation.Conditional, "ahora mismo");
			AddTimeframeExpression (Conjugation.Conditional, "por supuesto");
		}
	}
}

