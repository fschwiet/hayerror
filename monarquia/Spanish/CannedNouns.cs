using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{
	public static class CannedNouns
	{
        static Dictionary<string, IEnumerable<Noun>> CategorizedNounPhrases = new Dictionary<string, IEnumerable<Noun>>();

        static IEnumerable<Noun> CommonPeopleSubjectPronouns = new[] {
				new Noun ("","", role: Identity.Speaker).SubjectPronoun(),
				new Noun ("","", role: Identity.Listener).SubjectPronoun(),
                new Noun ("","", role: Identity.FormalListener).SubjectPronoun(),
                new Noun ("","").SubjectPronoun(),
				new Noun ("","", isFeminine: true).SubjectPronoun(),
				new Noun ("","", isPlural: true, role: Identity.Speaker).SubjectPronoun(), 
				new Noun ("","", isPlural: true, role: Identity.Listener).SubjectPronoun(),
				new Noun ("","", isPlural: true, role: Identity.FormalListener).SubjectPronoun(),
				new Noun ("","", isPlural: true).SubjectPronoun(),
				new Noun ("","", isPlural: true, isFeminine: true).SubjectPronoun()
            };

        static IEnumerable<Noun> CommonSubjects = new[] {
                new Noun ("amigo", "friend", tagWith: "amigo"),
                new Noun ("amiga", "friend", tagWith: "amigo", isFeminine: true),
                new Noun ("amigos", "friend", tagWith: "amigo"),
                new Noun ("amigas", "friend", tagWith: "amigo", isFeminine: true),
                new Noun ("maestro", "schoolteacher"),
                new Noun ("maestra", "schoolteacher", isFeminine: true),
                new Noun ("profesor", "teacher"),
                new Noun ("profesora", "teacher", isFeminine: true),
                new Noun ("jefe", "boss"),
                new Noun ("dueño", "dueño"),
                new Noun ("dueña", "dueño", isFeminine: true),
				new Noun ("conocidos", "acquaintances", isPlural: true)
            };

        static IEnumerable<Noun> Relatives =  new[] {
				new Noun ("padre", "father"),
				new Noun ("madre", "mother", isFeminine:true),
				new Noun ("padres", "parents", isPlural: true),
				new Noun ("hermano", "brother"),
				new Noun ("hermana", "sister", isFeminine:true),
				new Noun ("hermanos", "siblings"),
				new Noun ("esposo", "husband"),
				new Noun ("esposa", "wife", isFeminine:true),
				new Noun ("hijo", "son"),
				new Noun ("hija", "daughter", isFeminine:true),
				new Noun ("hijos", "children", isPlural: true),
				new Noun ("abuelo", "grandfather"),
				new Noun ("abuela", "grandmother", isFeminine:true),
				new Noun ("abuelos", "grandparents", isPlural: true),
				new Noun ("tío", "uncle"),
				new Noun ("tía", "aunt", isFeminine:true),
				new Noun ("primo", "cousin"),
				new Noun ("prima", "cousin", isFeminine:true),
				new Noun ("primos", "cousins", isPlural: true),
				new Noun ("sobrino", "nephew"),
				new Noun ("sobrina", "niece", isFeminine:true),
				new Noun ("bisabuelo", "great grandfather"),
				new Noun ("bisabuela", "great grandmother", isFeminine:true),
				new Noun ("bisabuelos", "great grandparents", isPlural: true),
				new Noun ("suegro", "father-in-law"),
				new Noun ("suegra", "mother-in-law", isFeminine:true),
				new Noun ("suegros", "in-law", isPlural:true),
				new Noun ("cuñado", "brother-in-law"),
				new Noun ("cuñada", "sister-in-law", isFeminine:true),
				new Noun ("nieto", "grandson"),
				new Noun ("nieta", "granddaughter", isFeminine:true),
				new Noun ("nietos", "grandchildren", isPlural: true),
				new Noun ("bisnieto", "great grandson"),
				new Noun ("bisnieta", "great granddaughter", isFeminine:true),
				new Noun ("bisnieto", "great grandchildren", isPlural: true),
				new Noun ("padrastro", "stepfather"),
				new Noun ("madrastra", "stepmother", isFeminine:true),
				new Noun ("hijastro", "stepson"),
				new Noun ("hijastra", "stepdaughter", isFeminine:true),
				new Noun ("hijastros", "stepchildren", isPlural: true),
				new Noun ("hermanastro", "stepbrother"),
				new Noun ("hermanastra", "stepsister", isFeminine:true),
				new Noun ("padrino", "godfather"),
				new Noun ("madrina", "godmother", isFeminine:true),
				new Noun ("ahijado", "godson"),
				new Noun ("ahijada", "goddaughter", isFeminine:true)
			};

        public static IEnumerable<Noun> GetCommonPeopleSubjectPronouns(bool includeThirdParty = true)
        {
            if (includeThirdParty)
                return CommonPeopleSubjectPronouns;
            else
                return CommonPeopleSubjectPronouns.Where(n => n.Role != Identity.Other);
        }

        public static IEnumerable<Noun> GetCommonSubjects()
        {
            return CommonSubjects;
        }

        public static IEnumerable<Noun> GetRelatives()
        {
            if (!CategorizedNounPhrases.ContainsKey("relatives"))
            {
                var result = new List<Noun>();

                foreach(var relative in Relatives)
                {
                    result.Add(BelongingNoun.CreateFor(Identity.Speaker, relative));
                }

                CategorizedNounPhrases["relatives"] = result;
            }

            return CategorizedNounPhrases["relatives"];
        }
	}
}

