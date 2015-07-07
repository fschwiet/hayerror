using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{
	public static class Pronouns
	{
		public static IEnumerable<ITranslateable> GetCommonPeopleSubjectNouns() {
			return new [] {
				new Noun ("yo", "I", isSubject: true, role: Noun.Identity.Speaker),
				new Noun ("tú", "you", isSubject: true, role: Noun.Identity.Listener).WithEnglishHint(),
                new Noun ("usted", "you", isSubject: true, role: Noun.Identity.FormalListener).WithEnglishHint(),
                new Noun ("él", "he", isSubject: true),
				new Noun ("ella", "she", isSubject: true, isFeminine: true),
				new Noun ("nosotros", "we", isSubject: true, isPlural: true, role: Noun.Identity.Speaker), 
				new Noun ("vosotros", "you all", isSubject: true, isPlural: true, role: Noun.Identity.Listener).WithEnglishHint().WithTag("vosotros"),
				new Noun ("ustedes", "you all", isSubject: true, isPlural: true, role: Noun.Identity.FormalListener),
				new Noun ("ellos", "they", isSubject:true, isPlural: true).WithEnglishHint(),
				new Noun ("ellas", "they", isSubject:true, isPlural: true, isFeminine: true).WithEnglishHint()
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

		//  http://www.studyspanish.com/lessons/iodopro.htm
		// http://www.studyspanish.com/lessons/reflexive2.htm
		public static IEnumerable<ITranslateable> GetCommonPeopleIndirectObject() {

			List<ITranslateable> results = new List<ITranslateable> ();

			results.Add (CannedTranslation.WithoutPointOfView ("me", "me", PointOfView.FirstPerson));
			results.Add (CannedTranslation.WithPointOfView    ("me", "myself", PointOfView.FirstPerson));

			results.Add (CannedTranslation.WithoutPointOfView ("te", "you", PointOfView.SecondPerson));
			results.Add (CannedTranslation.WithPointOfView    ("te", "yourself", PointOfView.SecondPerson));

			results.Add (CannedTranslation.WithoutPointOfView ("le", "you", PointOfView.SecondPersonFormal)
				.WithSpanishHint("usted").WithEnglishHint("formal"));
			results.Add (CannedTranslation.WithPointOfView    ("se", "yourself", PointOfView.SecondPersonFormal));

			results.Add (CannedTranslation.WithoutPointOfView ("le", "him", PointOfView.ThirdPersonMasculine)
				.WithSpanishHint ("masculine"));
			results.Add (CannedTranslation.WithPointOfView ("se", "himself", PointOfView.ThirdPersonMasculine));

			results.Add (CannedTranslation.WithoutPointOfView ("le", "her", PointOfView.ThirdPersonMasculine)
				.WithSpanishHint ("feminine"));
			results.Add (CannedTranslation.WithPointOfView ("se", "herself", PointOfView.ThirdPersonMasculine));

			results.Add (CannedTranslation.WithoutPointOfView ("nos", "us", PointOfView.FirstPersonPlural));
			results.Add (CannedTranslation.WithPointOfView    ("nos", "ourselves", PointOfView.FirstPersonPlural));

			results.Add (CannedTranslation.WithoutPointOfView ("os", "you all", PointOfView.SecondPersonPlural).WithTag("vosotros"));
			results.Add (CannedTranslation.WithPointOfView    ("os", "yourselves", PointOfView.SecondPersonPlural).WithTag("vosostros"));

			results.Add (CannedTranslation.WithoutPointOfView ("les", "you all", PointOfView.SecondPersonPluralFormal)
                .WithSpanishHint("ustedes"));
            results.Add(CannedTranslation.WithPointOfView("se", "yourselves", PointOfView.SecondPersonPluralFormal));

			results.Add (CannedTranslation.WithoutPointOfView ("les", "them", PointOfView.ThirdPersonPluralMasculine));
			results.Add (CannedTranslation.WithPointOfView    ("se", "themselves", PointOfView.ThirdPersonPluralMasculine));

			results.Add (CannedTranslation.WithoutPointOfView ("les", "them", PointOfView.ThirdPersonPluralFeminine));
			results.Add (CannedTranslation.WithPointOfView    ("se", "themselves", PointOfView.ThirdPersonPluralFeminine));
			return results;
		}
	}
}

