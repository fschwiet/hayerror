using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monarquia
{
    public class BetterCannedData : CannedDataBuilder
    {
        DataLoader dataLoader;

        public BetterCannedData(DataLoader dataLoader)
        {

            this.dataLoader = dataLoader;

            var timeframeExpressions = new[] {

				AddTimeframeExpression(Conjugation.PastPreterite, new CannedTranslation("a esa hora", "at that hour")),
				AddTimeframeExpression(Conjugation.PastPreterite, new CannedTranslation("ayer", "yesterday")),
				AddTimeframeExpression(Conjugation.PastPreterite, new CannedTranslation("de pronto", "suddenly")),
				AddTimeframeExpression(Conjugation.PastPreterite, new CannedTranslation("de repente", "suddenly")),
				AddTimeframeExpression(Conjugation.PastImperfect, new CannedTranslation("él me dijo que", "he told me that")),
				AddTimeframeExpression(Conjugation.PastImperfect, new CannedTranslation("todas las mañanas", "every morning")),
				AddTimeframeExpression(Conjugation.PastImperfect, new CannedTranslation("todos los años", "every year")),
				AddTimeframeExpression(Conjugation.PastImperfect, new CannedTranslation("todos los días", "every day"))
			}.ToList();

            foreach (var conjugation in Enum.GetValues(typeof(Conjugation)).Cast<Conjugation>()
                .Where(c => c != Conjugation.PastPreterite && c != Conjugation.PastImperfect))
            {
                timeframeExpressions.Add(AddTimeframeExpression(conjugation, new CannedTranslation("", "")));
            }

            var peopleExpressions = Pronouns.GetCommonPeopleSubjectNouns();

            AddRoleSelector(StartScenarios().
                hasOneOf("timeframe", timeframeExpressions).
                hasOneOf("subject", peopleExpressions).
                hasOneOf("verbEnding", new[] { new CannedTranslation("leche", "milk"), new CannedTranslation("agua", "water") }).
                hasTranslation("beber", "drink"));

            var actor = new Noun("actor", "actriz", "actores", "actrizes").WithTranslation("actor", "actors");
            var cook = new Noun("cocinero", "cocinera", "cocineros", "cocineras").WithTranslation("cook", "cooks");
            var dentist = new Noun("dentista", "dentistas").WithTranslation("dentist", "dentists");
            var student = new Noun("estudiante", "estudiantes").WithTranslation("student", "students");

            var professions = new[] { actor, cook, dentist, student };

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf("verbEnding", professions.Select(p => p.WithEnglishAlternative(new Article() + p)))
                .hasTranslation("ser", "be"));

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", new[] {
					CannedTranslation.WithPointOfView ("tiempo", "time", PointOfView.ThirdPersonMasculine),
					CannedTranslation.WithPointOfView ("mucho tráfico", "a lot of traffic", PointOfView.ThirdPersonMasculine),
					CannedTranslation.WithPointOfView ("confusión", "confusion", PointOfView.ThirdPersonFeminine),
					CannedTranslation.WithPointOfView ("lluvia", "rain", PointOfView.ThirdPersonFeminine),
					CannedTranslation.WithPointOfView ("problemas", "problems", PointOfView.ThirdPersonPluralMasculine),
					CannedTranslation.WithPointOfView ("tres pasos", "three steps", PointOfView.ThirdPersonPluralMasculine),
					CannedTranslation.WithPointOfView ("opciones", "options", PointOfView.ThirdPersonPluralFeminine),
					CannedTranslation.WithPointOfView ("soluciónes", "solutions", PointOfView.ThirdPersonPluralFeminine)
				})
                .hasOneOf("fakeSubject", new[] {
					new EnglishOnly("there")
				})
                .hasTranslation("haber", "be"),
                new[] { "timeframe", "spanishonlyNoPreposition", "verbPhrase", "subject" },
                new[] { "timeframe", "fakeSubject", "verbPhrase", "subject" });

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", new[] {
					CannedTranslation.WithPointOfView ("calor", "hot", PointOfView.ThirdPersonMasculine),
					CannedTranslation.WithPointOfView ("fresco", "cool", PointOfView.ThirdPersonMasculine),
					CannedTranslation.WithPointOfView ("viento", "windy", PointOfView.ThirdPersonMasculine),
				})
                .hasOneOf("fakeSubject", new[] {
					new EnglishOnly("it")
				})
                .hasTranslation("hacer", "be"),
                    new[] { "timeframe", "spanishonlyNoPreposition", "verbPhrase", "subject" },
                    new[] { "timeframe", "fakeSubject", "verbPhrase", "subject" }
            );

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf("verbEnding", new[] {
					new CannedTranslation("una viaje", "a trip"),
				})
                .hasTranslation("hacer", "take")
            );

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf("verbEnding", new[] {
					new CannedTranslation("una visita", ""),
				})
                .hasTranslation("hacer", "visit")
            );

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf("object", new[] {
					new CannedTranslation ("daño", "")
				})
                .hasOneOf("indirectObject", Pronouns.GetCommonPeopleIndirectObject())
                .hasOneOf("verbEnding", new[] { new CannedTranslation("", "") })
                .hasTranslation("hacer", "hurt"),
                new[] { "timeframe", "subject", "spanishonlyNoPreposition", "indirectObject", "verbPhrase", "object" },
                new[] { "timeframe", "subject", "verbPhrase", "indirectObject" }
                );

            var verbEndingsForConocer = new[] {
				new CannedTranslation ("a ella", "her"),
				new CannedTranslation ("a mi padre", "my father"),
				new CannedTranslation ("a mi madre", "my mother"),
				new CannedTranslation ("a mi hermano", "my brother"),
				new CannedTranslation ("a mi hermana", "my sister"),
				new CannedTranslation ("a mi suegro", "my father-in-law"),
				new CannedTranslation ("a mi suegra", "my mother-in-law"),
				new CannedTranslation ("a mi cuñado", "my brother-in-law"),
				new CannedTranslation ("a mi cuñada", "my sister-in-law"),
				new CannedTranslation ("a mi esposo", "my husband"),
				new CannedTranslation ("a mi esposa", "my wife"),
				new CannedTranslation ("a mi abuelo", "my grandfather"),
				new CannedTranslation ("a mi abuela", "my grandmother"),
				new CannedTranslation ("a mi bisabuelo", "my great grandfather"),
				new CannedTranslation ("a mi bisabuela", "my great grandmother"),
				new CannedTranslation ("a mi hijo", "my son"),
				new CannedTranslation ("a mi hija", "my daughter"),
				new CannedTranslation ("a mi nieto", "my grandson"),
				new CannedTranslation ("a mi nieta", "my granddaughter"),
				new CannedTranslation ("a mi bisnieto", "my great grandson"),
				new CannedTranslation ("a mi bisnieta", "my great granddaughter"),
				new CannedTranslation ("a mi tío", "my uncle"),
				new CannedTranslation ("a mi tía", "my aunt"),
				new CannedTranslation ("a mi primo", "my cousin"),
				new CannedTranslation ("a mi prima", "my cousin"),
				new CannedTranslation ("a mi sobrino", "my nephew"),
				new CannedTranslation ("a mi sobrina", "my niece"),
				new CannedTranslation ("a mi padrastro", "my stepfather"),
				new CannedTranslation ("a mi madrastra", "my stepmother"),
				new CannedTranslation ("a mi hijastro", "my stepson"),
				new CannedTranslation ("a mi hijastra", "my stepdaughter"),
				new CannedTranslation ("a mi hermanastro", "my stepbrother"),
				new CannedTranslation ("a mi hermanastra", "my stepsister"),
				new CannedTranslation ("a mi padrino", "my godfather"),
				new CannedTranslation ("a mi madrina", "my godmother"),
				new CannedTranslation ("a mi ahijado", "my godson"),
				new CannedTranslation ("a mi ahijada", "my goddaughter"),
				new CannedTranslation ("a mi conocido", "my acquaintance")
			};

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf("verbEnding", verbEndingsForConocer)
                .hasTranslation("conocer", "meet", frame => frame.Conjugation == Conjugation.PastPreterite || frame.Conjugation == Conjugation.Future)
                .hasTranslation("conocer", "know", frame => !(frame.Conjugation == Conjugation.PastPreterite || frame.Conjugation == Conjugation.Future)));

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", new[] {
					CannedTranslation.WithPointOfView("el reloj", "the watch", PointOfView.ThirdPersonMasculine),
					CannedTranslation.WithPointOfView("el reloj", "the clock", PointOfView.ThirdPersonMasculine)
				})
                .hasOneOf("verbEnding", new[] {
					new CannedTranslation("la una", "one"),
					new CannedTranslation("las dos", "two"),
					new CannedTranslation("el mediodía", "noon"),
					new CannedTranslation("la medianoche", "midnight")
				})
                .hasTranslation("dar", "strike"));

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf("verbEnding", new[] { new SpanishOnly("un abrazo").WithEnglishAlternative("") })
                .hasTranslation("dar", "hug"));

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf("verbEnding", new[] { new SpanishOnly("gritos").WithEnglishAlternative("") })
                .hasTranslation("dar", "shout"));

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf("verbEnding", new[] { new SpanishOnly("gritos ahogados").WithEnglishAlternative("") })
                .hasTranslation("dar", "gasp"));

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf("verbEnding", new ITranslateable[] {
                    new CannedTranslation("en frente", "in front"),
                    new CannedTranslation("al otro lado de la calle", "on the other side of the street"),
                    new Noun ("listo", "lista", "listos", "listas").WithTranslation("ready", "ready"),
                    new CannedTranslation("afuera", "outside")
				})
                .hasTranslation("estar", "be", framing => true)
            );

            HasEnglishTranslation("ir", "go");
            //HasEnglishTranslation ("cortar", "cut");
            ReflexiveHasEnglishTranslation("cortar", "cut");
            ReflexiveHasEnglishTranslation("duchar", "shower");
            ReflexiveHasEnglishTranslation("mirar", "look");

            foreach (var timeframeExpression in timeframeExpressions)
            {
                AddTimeframeExpression(timeframeExpression);
            }

            //  English "good" is ambiguous
            //AddVerbEnding ("estar", new Noun ("bueno", "buena", "buenos", "buenas").WithTranslation("good", "good"));

            AddVerbEnding("ir", new CannedTranslation("al cine", "to the movies"));
            AddVerbEnding("ir", new CannedTranslation("a la playa", "to the beach"));
            //AddVerbEnding ("ir", new CannedTranslation("a leer", "to read"));
            AddVerbEnding("ir", new CannedTranslation("allí", "there"));

            AddVerbEnding("cortar", new CannedTranslation("la cadena", "the chain"));
            AddVerbEnding("cortar", new CannedTranslation("los árboles", "the trees"));

            var pelo = new CannedTranslation("pelo", "hair");

            AddVerbEnding("cortarse", (new SpanishOnly("el") + pelo).
                WithEnglishAlternative(new PossessiveAdjective() + pelo));

            AddVerbEnding("ducharse", new CannedTranslation("", ""));
            AddVerbEnding("ducharse", new CannedTranslation("con agua fría", "with cold water"));
            AddVerbEnding("ducharse", new CannedTranslation("con agua caliente", "with hot water"));

            AddVerbEnding("mirarse", new CannedTranslation("", ""));
            AddVerbEnding("mirarse", new SpanishOnly("en el espejo").
                WithEnglishAlternative(new EnglishOnly("at") + new ReflexivePronoun() + new EnglishOnly("in the mirror")));
            //  Plural only: AddVerbEnding ("mirarse", new CannedTranslation ("uno a otro", "at one another"));
        }

        public VerbRoleSelector StartScenarios()
        {
            var result = new VerbRoleSelector(this, this.dataLoader);
            result.hasOneOf("spanishonlyNoPreposition", PotentialNegationPreposition.Get());
            return result;
        }
    }
}
