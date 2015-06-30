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

            var actor = new FollowsFrameMasculinityAndPlurality("actor", "actriz", "actores", "actrizes").WithTranslation("actor", "actors");
            var cook = new FollowsFrameMasculinityAndPlurality("cocinero", "cocinera", "cocineros", "cocineras").WithTranslation("cook", "cooks");
            var dentist = new FollowsFrameMasculinityAndPlurality("dentista", "dentistas").WithTranslation("dentist", "dentists");
            var student = new FollowsFrameMasculinityAndPlurality("estudiante", "estudiantes").WithTranslation("student", "students");

            var professions = new[] { actor, cook, dentist, student };

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf<ITranslateable>("verbEnding", 
                        spanishNoun => new [] {spanishNoun}, 
                        englishNoun => new [] {new Article(), englishNoun}, professions)
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

            var relativeNouns = new[] {
				new CannedTranslation ("padre", "father"),
				new CannedTranslation ("madre", "mother"),
				new CannedTranslation ("hermano", "brother"),
				new CannedTranslation ("hermana", "sister"),
				new CannedTranslation ("suegro", "father-in-law"),
				new CannedTranslation ("suegra", "mother-in-law"),
				new CannedTranslation ("cuñado", "brother-in-law"),
				new CannedTranslation ("cuñada", "sister-in-law"),
				new CannedTranslation ("esposo", "husband"),
				new CannedTranslation ("esposa", "wife"),
				new CannedTranslation ("abuelo", "grandfather"),
				new CannedTranslation ("abuela", "grandmother"),
				new CannedTranslation ("bisabuelo", "great grandfather"),
				new CannedTranslation ("bisabuela", "great grandmother"),
				new CannedTranslation ("hijo", "son"),
				new CannedTranslation ("hija", "daughter"),
				new CannedTranslation ("nieto", "grandson"),
				new CannedTranslation ("nieta", "granddaughter"),
				new CannedTranslation ("bisnieto", "great grandson"),
				new CannedTranslation ("bisnieta", "great granddaughter"),
				new CannedTranslation ("tío", "uncle"),
				new CannedTranslation ("tía", "aunt"),
				new CannedTranslation ("primo", "cousin"),
				new CannedTranslation ("prima", "cousin"),
				new CannedTranslation ("sobrino", "nephew"),
				new CannedTranslation ("sobrina", "niece"),
				new CannedTranslation ("padrastro", "stepfather"),
				new CannedTranslation ("madrastra", "stepmother"),
				new CannedTranslation ("hijastro", "stepson"),
				new CannedTranslation ("hijastra", "stepdaughter"),
				new CannedTranslation ("hermanastro", "stepbrother"),
				new CannedTranslation ("hermanastra", "stepsister"),
				new CannedTranslation ("padrino", "godfather"),
				new CannedTranslation ("madrina", "godmother"),
				new CannedTranslation ("ahijado", "godson"),
				new CannedTranslation ("ahijada", "goddaughter"),
				new CannedTranslation ("conocido", "acquaintance")
			};

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf("verbEnding", new[] { new CannedTranslation("a ella", "her") })
                .hasOneOf("verbEnding", new[] { new CannedTranslation("a mis padres", "my parents") })
                .hasOneOf<ITranslateable>("verbEnding", 
                    spanishNoun => new [] { new SpanishOnly("a"), new SpanishOnly("mi"), spanishNoun},
                    englishNoun => new [] { new EnglishOnly("my"), englishNoun},
                    relativeNouns)
                .hasTranslation("conocer", "meet", frame => frame.Conjugation == Conjugation.PastPreterite || frame.Conjugation == Conjugation.Future)
                .hasTranslation("conocer", "know", frame => !(frame.Conjugation == Conjugation.PastPreterite || frame.Conjugation == Conjugation.Future)));

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf<Noun>("subject",
                    noun => new[] { noun.DefiniteArticle(), noun },
                    new[] {
					    new Noun("reloj", "watch", isSubject: true),
					    new Noun("relojes", "watches", isSubject: true, isPlural:true),
					    new Noun("reloj", "clock", isSubject: true),
					    new Noun("relojes", "clocks", isSubject: true, isPlural:true)
                    })
                .hasOneOf<Noun>("verbEnding",
                    spanishNoun => new[] { spanishNoun.DefiniteArticle(), spanishNoun },
                    englishNoun => new[] { englishNoun },
                    new[] {
					    new Noun("una", "one"),
					    new Noun("dos", "two", isPlural:true),
					    new Noun("seis", "six", isPlural:true),
					    new Noun("mediodía", "noon", isPlural:false),
					    new Noun("medianoche", "midnight", isPlural:true)
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
                    new FollowsFrameMasculinityAndPlurality ("listo", "lista", "listos", "listas").WithTranslation("ready", "ready"),
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
            return result;
        }
    }
}
