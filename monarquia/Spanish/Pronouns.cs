using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{
	public static class Pronouns
	{
        public static IEnumerable<ITranslateable> GetCommonPeopleSubjectPronouns()
        {
            return new[] {
				new Noun ("","", role: Noun.Identity.Speaker).SubjectPronoun(),
				new Noun ("","", role: Noun.Identity.Listener).SubjectPronoun(),
                new Noun ("","", role: Noun.Identity.FormalListener).SubjectPronoun(),
                new Noun ("","").SubjectPronoun(),
				new Noun ("","", isFeminine: true).SubjectPronoun(),
				new Noun ("","", isPlural: true, role: Noun.Identity.Speaker).SubjectPronoun(), 
				new Noun ("","", isPlural: true, role: Noun.Identity.Listener).SubjectPronoun(),
				new Noun ("","", isPlural: true, role: Noun.Identity.FormalListener).SubjectPronoun(),
				new Noun ("","", isPlural: true).SubjectPronoun(),
				new Noun ("","", isPlural: true, isFeminine: true).SubjectPronoun()
            };
        }
	}
}

