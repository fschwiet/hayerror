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

			AddTimeframeExpression (Verb.Conjugation.PastPreterite, new CannedTranslation("a esa hora", "at that hour"));
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, new CannedTranslation("ayer", "yesterday"));
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, new CannedTranslation("de pronto", "suddenly"));
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, new CannedTranslation("de repente", "suddenly"));

			AddTimeframeExpression (Verb.Conjugation.PastImperfect, new CannedTranslation("él me dijo que", "he told me that"));
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, new CannedTranslation("todas las mañanas", "every morning"));
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, new CannedTranslation("todos los años", "every year"));
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, new CannedTranslation("todos los días", "every day"));

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
		}
	}

	public class BigCannedData : BetterCannedData
	{
		public BigCannedData ()
		{
			AddVerbTranslation ("beber", "drink");
			AddVerbTranslation ("comer", "eat");
			AddVerbTranslation ("estar", "is");
			AddVerbTranslation ("gritar", "shout");
			AddVerbTranslation ("hablar", "talk");
			AddVerbTranslation ("ir", "go");
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

			AddVerbEnding ("estar", new CannedTranslation("en frente", "in front"));
			AddVerbEnding ("estar", new CannedTranslation("en el detrás", "in the back"));
			AddVerbEnding ("estar", new CannedTranslation("al otro lado de la calle", "on the other side of the street"));

			AddVerbEnding ("hablar", new CannedTranslation("a la reportera", "to the reporter"));
			AddVerbEnding ("hablar", new CannedTranslation("con él", "to him"));
			AddVerbEnding ("ir", new CannedTranslation("al cine", "to the movies"));
			AddVerbEnding ("ir", new CannedTranslation("a decir la verdad", "to tell the truth"));
			AddVerbEnding ("ir", new CannedTranslation("a leer", "to read"));
			AddVerbEnding ("ir", new CannedTranslation("allí", "threre"));
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
			AddTimeframeExpression (Verb.Conjugation.Present, new CannedTranslation("", ""));
			AddTimeframeExpression (Verb.Conjugation.Present, "ahora");
			AddTimeframeExpression (Verb.Conjugation.Present, "ahora mismo");
			AddTimeframeExpression (Verb.Conjugation.Present, "hoy");
			AddTimeframeExpression (Verb.Conjugation.Present, "a este minuto");

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

			// Near future
			AddTimeframeExpression (Verb.Conjugation.Present, "al mediodía");
			AddTimeframeExpression (Verb.Conjugation.Present, "esta semana");
			AddTimeframeExpression (Verb.Conjugation.Present, "esta mes");

			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "a las cuatro");
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "anoche");
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "anteanoche");
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "anteayer");
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


			/*
			 * 
			// excluding siempre since amibguous and google mistranslates 'Siempre usted está en frente'

			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "entonces");  // ambiguous
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "finalmente");  // ambiguous
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "por fin");  // ambiguous
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "por primera vez");  // ambiguous
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "un día");  // ambiguous
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "una noche");  // ambiguous
			AddTimeframeExpression (Verb.Conjugation.PastPreterite, "una vez");  // ambiguous

			AddTimeframeExpression (Timeframe.PastPreterite, "con frecuencia"); // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "constantemente"); // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "de costumbre"); // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "frecuentemente"); // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "generalmente"); // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "normalmente"); // ambiguous
			AddTimeframeExpression (Timeframe.PastPreterite, "por lo general"); // ambiguous

			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "mientras");  // also a conjugation

*/

			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "a veces");
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "algunas veces");
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "casi nunca");
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "casi siempre");
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "de vez en cuando");
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "muchas veces");
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "por lo general");
			AddTimeframeExpression (Verb.Conjugation.PastImperfect, "rara vez");

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
	}
}

