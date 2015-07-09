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

        public static ITranslateable GetReflexivePronoun(PointOfView pointOfView)
        {
            switch (pointOfView)
            {
                case PointOfView.FirstPerson:
                    return new SpanishOnly("me");
                case PointOfView.FirstPersonPlural:
                    return new SpanishOnly("nos");
                case PointOfView.SecondPerson:
                    return new SpanishOnly("te");
                case PointOfView.SecondPersonFormal:
                    return new SpanishOnly("se");
                case PointOfView.SecondPersonPlural:
                    return new SpanishOnly("os");
                case PointOfView.SecondPersonPluralFormal:
                    return new SpanishOnly("se");

                case PointOfView.ThirdPersonFeminine:
                case PointOfView.ThirdPersonMasculine:
                case PointOfView.ThirdPersonPluralFeminine:
                case PointOfView.ThirdPersonPluralMasculine:
                    return new SpanishOnly("se");
                default:
                    throw new InvalidOperationException();
            }

        }
	}
}

