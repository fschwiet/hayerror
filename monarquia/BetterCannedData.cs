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

				new CannedTranslation("a esa hora", "at that hour", f => f.Conjugation == Conjugation.PastPreterite),
				new CannedTranslation("ayer", "yesterday", f => f.Conjugation == Conjugation.PastPreterite),
				//AddTimeframeExpression(Conjugation.PastPreterite, new CannedTranslation("de pronto", "suddenly")),
				//AddTimeframeExpression(Conjugation.PastPreterite, new CannedTranslation("de repente", "suddenly")),
				new CannedTranslation("él me dijo que", "he told me that", f => f.Conjugation == Conjugation.PastImperfect),
				//AddTimeframeExpression(Conjugation.PastImperfect, new CannedTranslation("todas las mañanas", "every morning")),
				//AddTimeframeExpression(Conjugation.PastImperfect, new CannedTranslation("todos los años", "every year")),
				//AddTimeframeExpression(Conjugation.PastImperfect, new CannedTranslation("todos los días", "every day"))
			}.ToList<ITranslateable>();

            foreach (var conjugation in Enum.GetValues(typeof(Conjugation)).Cast<Conjugation>()
                .Where(c => c != Conjugation.PastPreterite && c != Conjugation.PastImperfect))
            {
                timeframeExpressions.Add(new CannedTranslation("", "", f => f.Conjugation == conjugation));
            }

            var peopleExpressions = CannedNouns.GetCommonPeopleSubjectPronouns();
            AddLearningPriority(peopleExpressions);

            AddRoleSelector(StartScenarios()
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, peopleExpressions)
                .hasOneOf(Role.verbEnding, new ITranslateable[] {
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
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, peopleExpressions)
                .hasOneOf<ITranslateable>(Role.verbEnding,
                        professions,
                        spanishDecorator: t => t,
                        englishDecorator: t => new Article() + t)
                .hasTranslation("ser", "be"));

            AddRoleSelector(StartScenarios()
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, peopleExpressions)
                .hasOneOf<ITranslateable>(Role.verbEnding, new [] {
                        new FollowsFrameMasculinityAndPlurality("grosero", "grosera", "groseros", "groseras").WithEnglishAlternative("rude"),
                        new FollowsFrameMasculinityAndPlurality("gracioso", "graciosa", "graciosos", "graciosas").WithEnglishAlternative("funny"),
                        new FollowsFrameMasculinityAndPlurality("amable", "amables").WithEnglishAlternative("kind")
                })
                .hasTranslation("ser", "be"));

            AddRoleSelector(StartScenarios()
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, peopleExpressions)
                .hasOneOf(Role.verbEnding, new[] {
                    new CannedTranslation("al cine", "to the movies"),
                    new CannedTranslation("a la playa", "to the beach"),
                    new CannedTranslation("allí", "there")
                })
                .hasTranslation("ir", "go"));

            AddRoleSelector(StartScenarios()
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, new[] {
					new Noun("", "there")
                })
                .hasOneOf(Role.directObject, new[] {
					new Noun ("tiempo", "time"),
					new Noun ("mucho tráfico", "a lot of traffic"),
				})
                .hasTranslation("haber", "be"));

            AddRoleSelector(StartScenarios()
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, new[] {
					new Noun("", "there", isFeminine: true)
                })
                .hasOneOf(Role.directObject, new[] {
					new Noun ("confusión", "confusion", isFeminine: true),
					new Noun ("lluvia", "rain", isFeminine: true),
				})
                .hasTranslation("haber", "be"));

            AddRoleSelector(StartScenarios()
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, new[] {
					new Noun("", "there", isPlural: true)
                })
                .hasOneOf(Role.directObject, new[] {
					new Noun ("problemas", "problems", isPlural: true),
					new Noun ("tres pasos", "three steps", isPlural: true),
				})
                .hasTranslation("haber", "be"));

            AddRoleSelector(StartScenarios()
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, new[] {
					new Noun("", "there", isFeminine: true, isPlural: true)
                })
                .hasOneOf(Role.directObject, new[] {
					new Noun ("opciones", "options", isFeminine: true, isPlural: true),
					new Noun ("soluciónes", "solutions", isFeminine: true, isPlural: true)
				})
                .hasTranslation("haber", "be"));

            AddRoleSelector(StartScenarios()
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, new[] {
					new Noun("", "it")
				})
                .hasOneOf(Role.directObject, new[] {
					new Noun ("calor", "hot"),
					new Noun ("fresco", "cool"),
					new Noun ("viento", "windy"),
				})
                .hasTranslation("hacer", "feel"));

            AddRoleSelector(StartScenarios()
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, peopleExpressions)
                .hasOneOf(Role.verbEnding, new[] {
					new CannedTranslation("un viaje", "a trip"),
				})
                .hasTranslation("hacer", "take")
            );

            AddRoleSelector(StartScenarios()
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, peopleExpressions)
                .hasOneOf(Role.verbEnding, new[] {
					new CannedTranslation("una visita", ""),
				})
                .hasTranslation("hacer", "visit")
            );

            AddRoleSelector(StartScenarios()
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, peopleExpressions)
                .hasOneOf(Role.directObject, new[] {
					new CannedTranslation ("daño", "")
				})
                .hasOneOf(Role.indirectObject, peopleExpressions)
                .hasTransform(roleSelections => roleSelections.MakeIndirectObjectPronoun())
                .hasTranslation("hacer", "hurt"));

            var relativeNouns = CannedNouns.GetRelatives();

            AddLearningPriority(relativeNouns);

            AddRoleSelector(StartScenarios()
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, peopleExpressions)
                .hasOneOf(Role.verbEnding, new[] { new CannedTranslation("a mis padres", "my parents") })
                .hasOneOf<ITranslateable>(Role.verbEnding, 
                    relativeNouns,
                    spanishDecorator: t => new SpanishOnly("a") + new SpanishOnly("mi") + t,
                    englishDecorator: t => new EnglishOnly("my") + t)
                .hasTranslation("conocer", "meet", frame => frame.Conjugation == Conjugation.PastPreterite || frame.Conjugation == Conjugation.Future)
                .hasTranslation("conocer", "know", frame => !(frame.Conjugation == Conjugation.PastPreterite || frame.Conjugation == Conjugation.Future)));

            AddRoleSelector(StartScenarios()
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf<Noun>(Role.subject,
                    new[] {
					    new Noun("reloj", "watch"),
					    new Noun("relojes", "watches", isPlural:true),
					    new Noun("reloj", "clock"),
					    new Noun("relojes", "clocks", isPlural:true)
                    },
                    noun => noun.DefiniteArticle() + noun)
                .hasOneOf<Noun>(Role.verbEnding,
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
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, peopleExpressions)
                .hasOneOf(Role.verbEnding, new[] { new SpanishOnly("un abrazo").WithEnglishAlternative("") })
                .hasTranslation("dar", "hug"));

            AddRoleSelector(StartScenarios()
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, peopleExpressions)
                .hasOneOf(Role.verbEnding, new[] { new SpanishOnly("gritos").WithEnglishAlternative("") })
                .hasTranslation("dar", "shout"));

            AddRoleSelector(StartScenarios()
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, peopleExpressions)
                .hasOneOf(Role.verbEnding, new[] { new SpanishOnly("gritos ahogados").WithEnglishAlternative("") })
                .hasTranslation("dar", "gasp"));

            AddRoleSelector(StartScenarios().
                hasOneOf(Role.timeframe, timeframeExpressions).
                hasOneOf(Role.subject, peopleExpressions).
                hasOneOf(Role.verbEnding, new[] { new CannedTranslation("leche", "milk"), new CannedTranslation("agua", "water") }).
                hasTranslation("beber", "drink"));

            AddRoleSelector(StartScenarios()
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, peopleExpressions)
                .hasOneOf(Role.verbEnding, new[] { new CannedTranslation("la cena", "dinner") })
                .hasTranslation("preparar", "prepare"));

            AddRoleSelector(StartScenarios()
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, peopleExpressions)
                .hasOneOf(Role.directObject, new[] { new PossessiveAdjective(false, true) + new CannedTranslation("voz", "voice") })
                .hasOneOf(Role.indirectObject, new[] { 
                        new Noun("otros", "others", isPlural: true),
                        new Noun("coro", "chorus"),
                    }, n => new CannedTranslation("a", "to") + n.DefiniteArticle() + n)
                .hasTranslation("sumar", "add"));

            AddRoleSelector(StartScenarios()
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, peopleExpressions)
                .hasOneOf(Role.directObject, new[] { new Noun("comida", "food", isFeminine: true) },
                    n => n.DefiniteArticle() + n)
                .hasOneOf(Role.indirectObject, new[] { 
                        new Noun("cuenta", "bill", isFeminine: true)
                    }, n => new CannedTranslation("a", "to") + n.DefiniteArticle() + n)
                .hasTranslation("sumar", "add"));

            AddRoleSelector(StartScenarios()
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, peopleExpressions)
                .hasOneOf(Role.directObject, new[] {
                    new Noun("cadena", "chain", isFeminine: true),
                    new Noun("manzana", "apple", isFeminine: true)
                }, n => n.DefiniteArticle() + n)
                .hasTranslation("cortar", "cut"));

            AddRoleSelector(StartScenarios()
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, peopleExpressions)
                .hasOneOf(Role.directObject, new[] {
                    new Noun("pelo", "hair")
                    },
                    spanishToTranslateable: n => n.DefiniteArticle() + n,
                    englishToTranslateable: n => n.PossessedBySubjectArticle() + n)
                .hasTransform(s => s.MakeSpanishReflexive())
                .hasTranslation("cortar", "cut"));

            AddRoleSelector(StartScenarios()
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, peopleExpressions)
                .hasOneOf(Role.verbEnding, new[] {
                    new Noun("agua fría", "cold water"),
                    new Noun("agua caliente", "hot water")
                    }, n => new CannedTranslation("con", "with") + n)
                .hasTransform(s => s.MakeSpanishReflexive())
                .hasTranslation("duchar", "shower"));

            AddRoleSelector(StartScenarios()
                .hasOneOf(Role.timeframe, timeframeExpressions)
                .hasOneOf(Role.subject, peopleExpressions)
                .hasOneOf(Role.verbEnding, new ITranslateable[] {
                    new SpanishOnly("en el espejo").WithEnglishAlternative(new EnglishOnly("at") + new ReflexivePronoun() + new EnglishOnly("in the mirror")),
                    new CannedTranslation ("uno a otro", "at one another", frame => frame.PointOfView.IsPlural())
                    })
                .hasTransform(s => s.MakeSpanishReflexive())
                .hasTranslation("mirar", "look"));

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
