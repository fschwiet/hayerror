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
        protected List<ITranslateable> timeframeExpressions;

        public BetterCannedData(DataLoader dataLoader)
        {
            this.dataLoader = dataLoader;

            timeframeExpressions = new[] {

				AddTimeframeExpression(Conjugation.PastPreterite, new CannedTranslation("a esa hora", "at that hour")),
				AddTimeframeExpression(Conjugation.PastPreterite, new CannedTranslation("ayer", "yesterday")),
				//AddTimeframeExpression(Conjugation.PastPreterite, new CannedTranslation("de pronto", "suddenly")),
				//AddTimeframeExpression(Conjugation.PastPreterite, new CannedTranslation("de repente", "suddenly")),
				AddTimeframeExpression(Conjugation.PastImperfect, new CannedTranslation("él me dijo que", "he told me that")),
				//AddTimeframeExpression(Conjugation.PastImperfect, new CannedTranslation("todas las mañanas", "every morning")),
				//AddTimeframeExpression(Conjugation.PastImperfect, new CannedTranslation("todos los años", "every year")),
				//AddTimeframeExpression(Conjugation.PastImperfect, new CannedTranslation("todos los días", "every day"))
			}.ToList();

            foreach (var conjugation in Enum.GetValues(typeof(Conjugation)).Cast<Conjugation>()
                .Where(c => c != Conjugation.PastPreterite && c != Conjugation.PastImperfect))
            {
                timeframeExpressions.Add(AddTimeframeExpression(conjugation, new CannedTranslation("", "")));
            }

            var peopleExpressions = Pronouns.GetCommonPeopleSubjectNouns();

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

            var actor = new FollowsFrameMasculinityAndPlurality("actor", "actriz", "actores", "actrizes").WithTranslation("actor", "actors");
            var cook = new FollowsFrameMasculinityAndPlurality("cocinero", "cocinera", "cocineros", "cocineras").WithTranslation("cook", "cooks");
            var dentist = new FollowsFrameMasculinityAndPlurality("dentista", "dentistas").WithTranslation("dentist", "dentists");
            var student = new FollowsFrameMasculinityAndPlurality("estudiante", "estudiantes").WithTranslation("student", "students");

            var professions = new[] { actor, cook, dentist, student };

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf<ITranslateable>("verbEnding",
                        professions,
                        spanishDecorator: t => t,
                        englishDecorator: t => new Article() + t)
                .hasTranslation("ser", "be"));

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf<ITranslateable>("verbEnding", new [] {
                        new FollowsFrameMasculinityAndPlurality("grosero", "grosera", "groseros", "groseras").WithEnglishAlternative("rude"),
                        new FollowsFrameMasculinityAndPlurality("gracioso", "graciosa", "graciosos", "graciosas").WithEnglishAlternative("funny"),
                        new FollowsFrameMasculinityAndPlurality("amable", "amables").WithEnglishAlternative("kind")
                })
                .hasTranslation("ser", "be"));

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf("verbEnding", new [] {
                    new CannedTranslation("al cine", "to the movies"),
                    new CannedTranslation("a la playa", "to the beach"),
                    new CannedTranslation("allí", "there")
                })
                .hasTranslation("ir", "go"));

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
                .hasTranslation("hacer", "feel"),
                    new[] { "timeframe", "spanishonlyNoPreposition", "verbPhrase", "subject" },
                    new[] { "timeframe", "fakeSubject", "verbPhrase", "subject" }
            );

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf("verbEnding", new[] {
					new CannedTranslation("un viaje", "a trip"),
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
				new Noun ("padre", "father"),
				new Noun ("madre", "mother", isFeminine:true),
				new Noun ("hermano", "brother"),
				new Noun ("hermana", "sister", isFeminine:true),
				new Noun ("suegro", "father-in-law"),
				new Noun ("suegra", "mother-in-law", isFeminine:true),
				new Noun ("cuñado", "brother-in-law"),
				new Noun ("cuñada", "sister-in-law", isFeminine:true),
				new Noun ("esposo", "husband"),
				new Noun ("esposa", "wife", isFeminine:true),
				new Noun ("abuelo", "grandfather"),
				new Noun ("abuela", "grandmother", isFeminine:true),
				new Noun ("bisabuelo", "great grandfather"),
				new Noun ("bisabuela", "great grandmother", isFeminine:true),
				new Noun ("hijo", "son"),
				new Noun ("hija", "daughter", isFeminine:true),
				new Noun ("nieto", "grandson"),
				new Noun ("nieta", "granddaughter", isFeminine:true),
				new Noun ("bisnieto", "great grandson"),
				new Noun ("bisnieta", "great granddaughter", isFeminine:true),
				new Noun ("tío", "uncle"),
				new Noun ("tía", "aunt", isFeminine:true),
				new Noun ("primo", "cousin"),
				new Noun ("prima", "cousin", isFeminine:true),
				new Noun ("sobrino", "nephew"),
				new Noun ("sobrina", "niece", isFeminine:true),
				new Noun ("padrastro", "stepfather"),
				new Noun ("madrastra", "stepmother", isFeminine:true),
				new Noun ("hijastro", "stepson"),
				new Noun ("hijastra", "stepdaughter", isFeminine:true),
				new Noun ("hermanastro", "stepbrother"),
				new Noun ("hermanastra", "stepsister", isFeminine:true),
				new Noun ("padrino", "godfather"),
				new Noun ("madrina", "godmother", isFeminine:true),
				new Noun ("ahijado", "godson"),
				new Noun ("ahijada", "goddaughter", isFeminine:true),
				new Noun ("conocido", "acquaintance")
			};

            AddLearningPriority(relativeNouns);

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf("verbEnding", new[] { new CannedTranslation("a mis padres", "my parents") })
                .hasOneOf<ITranslateable>("verbEnding", 
                    relativeNouns,
                    spanishDecorator: t => new SpanishOnly("a") + new SpanishOnly("mi") + t,
                    englishDecorator: t => new EnglishOnly("my") + t)
                .hasTranslation("conocer", "meet", frame => frame.Conjugation == Conjugation.PastPreterite || frame.Conjugation == Conjugation.Future)
                .hasTranslation("conocer", "know", frame => !(frame.Conjugation == Conjugation.PastPreterite || frame.Conjugation == Conjugation.Future)));

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf<Noun>("subject",
                    new[] {
					    new Noun("reloj", "watch", isSubject: true),
					    new Noun("relojes", "watches", isSubject: true, isPlural:true),
					    new Noun("reloj", "clock", isSubject: true),
					    new Noun("relojes", "clocks", isSubject: true, isPlural:true)
                    },
                    noun => noun.DefiniteArticle() + noun)
                .hasOneOf<Noun>("verbEnding",
                    new[] {
					    new Noun("una", "one"),
					    new Noun("dos", "two", isPlural:true),
					    new Noun("seis", "six", isPlural:true),
					    new Noun("mediodía", "noon", isPlural:false),
					    new Noun("medianoche", "midnight", isPlural:true)
				    },
                    spanishToTranslateable: n => n.DefiniteArticle() + n,
                    englishToTranslateable: n => n)
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

            AddRoleSelector(StartScenarios().
                hasOneOf("timeframe", timeframeExpressions).
                hasOneOf("subject", peopleExpressions).
                hasOneOf("verbEnding", new[] { new CannedTranslation("leche", "milk"), new CannedTranslation("agua", "water") }).
                hasTranslation("beber", "drink"));

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf("verbEnding", new[] { new CannedTranslation("la cena", "dinner") })
                .hasTranslation("preparar", "prepare"));

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf("directObject", new[] { new PossessiveAdjective(false, true) + new CannedTranslation("voz", "voice") })
                .hasOneOf("indirectObject", new[] { 
                        new Noun("otros", "others", isPlural: true),
                        new Noun("coro", "chorus"),
                    }, n => new CannedTranslation("a", "to") + n.DefiniteArticle() + n)
                .hasTranslation("sumar", "add"));

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf("directObject", new[] { new Noun("comida", "food", isFeminine:true) },
                    n => n.DefiniteArticle() + n)
                .hasOneOf("indirectObject", new[] { 
                        new Noun("cuenta", "bill", isFeminine: true)
                    }, n => new CannedTranslation("a", "to") + n.DefiniteArticle() + n)
                .hasTranslation("sumar", "add"));

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf("directObject", new[] {
                    new Noun("cadena", "chain", isFeminine: true),
                    new Noun("manzana", "apple", isFeminine: true)
                }, n => n.DefiniteArticle() + n)
                .hasTranslation("cortar", "cut"));

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf("directObject", new[] {
                    new Noun("pelo", "hair")
                    },
                    spanishToTranslateable: n => n.DefiniteArticle() + n,
                    englishToTranslateable: n => n.PossessedBySubjectArticle() + n)
                .hasReflexiveTranslation("cortar", "cut"));

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf("verbEnding", new[] {
                    new Noun("agua fría", "cold water"),
                    new Noun("agua caliente", "hot water")
                    }, n => new CannedTranslation("con", "with") + n)
                .hasReflexiveTranslation("duchar", "shower"));

            AddRoleSelector(StartScenarios()
                .hasOneOf("timeframe", timeframeExpressions)
                .hasOneOf("subject", peopleExpressions)
                .hasOneOf("verbEnding", new ITranslateable[] {
                    new SpanishOnly("en el espejo").WithEnglishAlternative(new EnglishOnly("at") + new ReflexivePronoun() + new EnglishOnly("in the mirror")),
                    new CannedTranslation ("uno a otro", "at one another", frame => frame.PointOfView.IsPlural())
                    })
                .hasReflexiveTranslation("mirar", "look"));

            foreach (var timeframeExpression in timeframeExpressions)
            {
                AddTimeframeExpression(timeframeExpression);
            }

            //  English "good" is ambiguous
            //AddVerbEnding ("estar", new Noun ("bueno", "buena", "buenos", "buenas").WithTranslation("good", "good"));
        }

        public VerbRoleSelector StartScenarios()
        {
            var result = new VerbRoleSelector(this, this.dataLoader);
            return result;
        }
    }
}
