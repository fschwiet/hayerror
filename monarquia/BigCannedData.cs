using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{
	public class BetterCannedData : CannedDataBuilder 
	{
		public static readonly IEnumerable<string> BetterVerbs = new string[] {
			"ser"
		};

		public BetterCannedData(DataLoader dataLoader) {

			var timeframeExpressions = new [] {

				AddTimeframeExpression(Conjugation.PastPreterite, new CannedTranslation("a esa hora", "at that hour")),
				AddTimeframeExpression(Conjugation.PastPreterite, new CannedTranslation("ayer", "yesterday")),
				AddTimeframeExpression(Conjugation.PastPreterite, new CannedTranslation("de pronto", "suddenly")),
				AddTimeframeExpression(Conjugation.PastPreterite, new CannedTranslation("de repente", "suddenly")),
				AddTimeframeExpression(Conjugation.PastImperfect, new CannedTranslation("él me dijo que", "he told me that")),
				AddTimeframeExpression(Conjugation.PastImperfect, new CannedTranslation("todas las mañanas", "every morning")),
				AddTimeframeExpression(Conjugation.PastImperfect, new CannedTranslation("todos los años", "every year")),
				AddTimeframeExpression(Conjugation.PastImperfect, new CannedTranslation("todos los días", "every day"))
			}.ToList();

			foreach(var conjugation in Enum.GetValues(typeof(Conjugation)).Cast<Conjugation>()
				.Where(c => c != Conjugation.PastPreterite && c != Conjugation.PastImperfect))
			{
					timeframeExpressions.Add(AddTimeframeExpression(conjugation, new CannedTranslation("","")));
			}

			var peopleExpressions = Pronouns.GetSubjectNouns ();

			AddRoleSelector(new VerbRoleSelector ("beber").
				hasOneOf ("timeframe", timeframeExpressions).
				hasOneOf ("subject", peopleExpressions).
				hasOneOf ("verbEnding", new [] { new CannedTranslation("leche", "milk"), new CannedTranslation("agua", "water")}).
				hasTranslation ("drink", this, dataLoader));

			var actor = new Noun ("actor", "actriz", "actores", "actrizes").WithTranslation ("actor", "actors");
			var cook = new Noun ("cocinero", "cocinera", "cocineros", "cocineras").WithTranslation ("cook", "cooks");
			var dentist = new Noun ("dentista", "dentistas").WithTranslation ("dentist", "dentists");
			var student = new Noun ("estudiante", "estudiantes").WithTranslation ("student", "students");

			var professions = new [] { actor, cook, dentist, student };

			AddRoleSelector (new VerbRoleSelector ("ser")
				.hasOneOf ("timeframe", timeframeExpressions)
				.hasOneOf ("subject", peopleExpressions)
				.hasOneOf ("verbEnding", professions.Select (p => ((Composed)p).WithEnglishAlternative ((Composed)new Article () + p)))
				.hasTranslation ("is", this, dataLoader));

			AddRoleSelector (new VerbRoleSelector ("haber")
				.hasOneOf ("timeframe", timeframeExpressions)
				.hasOneOf ("subject", new [] {
					new CannedTranslation ("mucho tráfico", "a lot of traffic", frameFilter: frame => frame.PointOfView == PointOfView.ThirdPersonPluralMasculine),
					new CannedTranslation ("tiempo", "time", frameFilter: frame => frame.PointOfView == PointOfView.ThirdPersonPluralMasculine),
					new CannedTranslation ("confusión", "confusion", frameFilter: frame => frame.PointOfView == PointOfView.ThirdPersonFeminine),
					new CannedTranslation ("problemas", "problems", frameFilter: frame => frame.PointOfView == PointOfView.ThirdPersonPluralMasculine),
					new CannedTranslation ("tres pasos", "three steps", frameFilter: frame => frame.PointOfView == PointOfView.ThirdPersonPluralMasculine)
				})
				.hasOneOf ("fakeSubject", new [] {
					new EnglishOnly("there")
				})
				.hasTranslation ("is", this, dataLoader),
				new [] {"timeframe", "verbPhrase", "subject" },
				new [] {"timeframe", "fakeSubject", "verbPhrase", "subject"});

			HasEnglishTranslation ("estar", "is");
			HasEnglishTranslation ("ir", "go");
			ReflexiveHasEnglishTranslation ("cortar", "cut");
			ReflexiveHasEnglishTranslation ("duchar", "shower");
			ReflexiveHasEnglishTranslation ("mirar", "look");
		
			HasEnglishTranslation ("conocer", p => {
				if (p == Conjugation.PastPreterite || p == Conjugation.Future)
					return "meet";

				return "know";
			});
				
			foreach(var timeframeExpression in timeframeExpressions) {
				AddTimeframeExpression (timeframeExpression);
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

			AddVerbEnding ("conocer", new CannedTranslation ("a ella", "her"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi padre", "my father"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi madre", "my mother"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi hermano", "my brother"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi hermana", "my sister"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi suegro", "my father-in-law"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi suegra", "my mother-in-law"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi cuñado", "my brother-in-law"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi cuñada", "my sister-in-law"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi esposo", "my husband"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi esposa", "my wife"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi abuelo", "my grandfather"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi abuela", "my grandmother"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi bisabuelo", "my great grandfather"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi bisabuela", "my great grandmother"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi hijo", "my son"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi hija", "my daughter"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi nieto", "my grandson"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi nieta", "my granddaughter"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi bisnieto", "my great grandson"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi bisnieta", "my great granddaughter"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi tío", "my uncle"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi tía", "my aunt"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi primo", "my cousin"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi prima", "my cousin"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi sobrino", "my nephew"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi sobrina", "my niece"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi padrastro", "my stepfather"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi madrastra", "my stepmother"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi hijastro", "my stepson"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi hijastra", "my stepdaughter"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi hermanastro", "my stepbrother"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi hermanastra", "my stepsister"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi padrino", "my godfather"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi madrina", "my godmother"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi ahijado", "my godson"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi ahijada", "my goddaughter"));
			AddVerbEnding ("conocer", new CannedTranslation ("a mi conocido", "my acquaintance"));

			AddVerbEnding ("cortar", new CannedTranslation ("la cadena", "the chain"));
			AddVerbEnding ("cortar", new CannedTranslation ("los árboles", "the trees"));

			var pelo = new CannedTranslation ("pelo", "hair");

			AddVerbEnding ("cortarse", ((Composed)new SpanishOnly("el") + pelo).
				WithEnglishAlternative ((Composed)new PossessiveAdjective() +  pelo));

			AddVerbEnding ("ducharse", new CannedTranslation ("", ""));
			AddVerbEnding ("ducharse", new CannedTranslation ("con agua fría", "with cold water"));
			AddVerbEnding ("ducharse", new CannedTranslation ("con agua caliente", "with hot water"));

			AddVerbEnding ("mirarse", new CannedTranslation ("", ""));
			AddVerbEnding ("mirarse", ((Composed)new SpanishOnly("en el espejo")).
				WithEnglishAlternative((Composed)new EnglishOnly("at") + new ReflexivePronoun() + new EnglishOnly("in the mirror")));
			//  Plural only: AddVerbEnding ("mirarse", new CannedTranslation ("uno a otro", "at one another"));
		}
	}

	public class BigCannedData : BetterCannedData
	{
		public BigCannedData (DataLoader dataLoader) : base(dataLoader)
		{
			HasEnglishTranslation ("comer", "eat");
			HasEnglishTranslation ("gritar", "shout");
			HasEnglishTranslation ("hablar", "talk");
			//AddVerbTranslation ("preparar", "prepare"); -> ugh, can't load spanishdictionary.com/conjugate/prepare
			HasEnglishTranslation ("subir", "climb");
			HasEnglishTranslation ("sumar", "sum");
			HasEnglishTranslation ("temer", "fear");
			HasEnglishTranslation ("tener", "have");


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

