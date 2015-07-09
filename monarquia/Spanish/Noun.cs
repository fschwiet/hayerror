using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monarquia
{
    public class Noun : CannedTranslation
    {
        public Noun(string spanishVersion, string englishVersion, 
                bool isFeminine = false, bool isPlural = false,
                Identity role = Identity.Other) 
            : base(spanishVersion, englishVersion)
        {
            if (!string.IsNullOrEmpty(spanishVersion) && !string.IsNullOrEmpty(englishVersion))
            {
                this.WithTag("noun:" + spanishVersion + "-" + englishVersion);
            }

            IsFeminine = isFeminine;
            IsPlural = isPlural;
            Role = role;
        }   

        public enum Identity {
            Speaker,
            Listener,
            FormalListener,
            Other
        }

        public bool IsFeminine { get; private set; }
        public bool IsPlural { get; private set; }
        public Identity Role { get; private set; } 
        public ITranslateable SubjectPronoun()
        {
            string spanishVersion, englishVersion, englishHint = null;
            List<string> tags = new List<string>();

            switch (Role)
            {
                case Identity.Speaker:
                    if (IsPlural)
                    {
                        spanishVersion = "nosotros";
                        englishVersion = "we";
                    }
                    else
                    {
                        spanishVersion = "yo";
                        englishVersion = "I";
                    }
                    break;

                case Identity.Listener:
                    if (IsPlural)
                    {
                        spanishVersion = "vosotros";
                        englishVersion = "all of you";
                        englishHint = spanishVersion;
                        tags.Add("vosotros");
                    }
                    else
                    {
                        spanishVersion = "tú";
                        englishVersion = "you";
                    }
                    break;  
                              
                case Identity.FormalListener:
                    if (IsPlural)
                    {
                        spanishVersion = "ustedes";
                        englishVersion = "all of you";
                    }
                    else
                    {
                        spanishVersion = "usted";
                        englishVersion = "you";
                        englishHint = spanishVersion;
                    }
                    break;
                case Identity.Other:

                    if (IsPlural)
                    {
                        if (IsFeminine)
                        {
                            spanishVersion = "ellas";
                            englishVersion = "they";
                            englishHint = spanishVersion;
                        }
                        else
                        {
                            spanishVersion = "ellos";
                            englishVersion = "they";
                            englishHint = spanishVersion;
                        }
                    }
                    else
                    {
                        if (IsFeminine)
                        {
                            spanishVersion = "ella";
                            englishVersion = "she";
                        }
                        else
                        {
                            spanishVersion = "él";
                            englishVersion = "he";
                        }
                    }
                    break;

                default:
                    throw new Exception("Unexpected role value: " + Role.ToString());
            }

            var result = new Noun(spanishVersion, englishVersion, IsFeminine, IsPlural, Role);

            if (englishHint != null)
                result.WithEnglishHint(englishHint);

            foreach (var tag in tags)
                result.WithTag(tag);

            return result;
        }

        public ITranslateable IndirectObjectPronoun()
        {
            string spanishVersion, englishVersion, spanishHint = null, englishHint = null;
            List<string> tags = new List<string>();

            switch (Role)
            {
                case Identity.Speaker:
                    if (IsPlural)
                    {
                        spanishVersion = "nos";
                        englishVersion = "us";
                    }
                    else
                    {
                        spanishVersion = "me";
                        englishVersion = "me";
                    }
                    break;

                case Identity.Listener:
                    if (IsPlural)
                    {
                        spanishVersion = "os";
                        englishVersion = "all of you";
                        tags.Add("vosotros");
                    }
                    else
                    {
                        spanishVersion = "te";
                        englishVersion = "you";
                    }
                    break;
                case Identity.FormalListener:
                    if (IsPlural)
                    {
                        spanishVersion = "les";
                        englishVersion = "all of you";
                        spanishHint = "ustedes";
                    }
                    else
                    {
                        spanishVersion = "le";
                        englishVersion = "you";
                        spanishHint = "usted";
                        englishHint = "formal";
                    }
                    break;
                case Identity.Other:
                    if (IsPlural)
                    {
                        if (IsFeminine)
                        {
                            spanishVersion = "les";
                            englishVersion = "them";
                        }
                        else
                        {
                            spanishVersion = "les";
                            englishVersion = "them";
                        }
                    }
                    else
                    {
                        if (IsFeminine)
                        {
                            spanishVersion = "le";
                            englishVersion = "her";
                            spanishHint = "feminine";
                        }
                        else
                        {
                            spanishVersion = "le";
                            englishVersion = "him";
                            spanishHint = "masculine";
                        }
                    }
                    break;
                default:
                    throw new Exception("Unexpected role: " + Role);

            }

            var result = new Noun(spanishVersion, englishVersion, IsFeminine, IsPlural, Role);

            if (englishHint != null)
                result.WithEnglishHint(englishHint);

            if (spanishHint != null)
                result.WithSpanishHint(spanishHint);

            foreach (var tag in tags)
                result.WithTag(tag);

            return result;
        }


        public ITranslateable ReflexivePronoun()
        {
            string spanishVersion, englishVersion;
            List<string> tags = new List<string>();

            switch (Role)
            {
                case Identity.Speaker:
                    if (IsPlural)
                    {
                        spanishVersion = "nos";
                        englishVersion = "ourselves";
                    }
                    else
                    {
                        spanishVersion = "me";
                        englishVersion = "myself";
                    }
                    break;

                case Identity.Listener:
                    if (IsPlural)
                    {
                        spanishVersion = "os";
                        englishVersion = "yourselves";
                        tags.Add("vosotros");
                    }
                    else
                    {
                        spanishVersion = "te";
                        englishVersion = "yourself";
                    }
                    break;
                case Identity.FormalListener:
                    if (IsPlural)
                    {
                        spanishVersion = "se";
                        englishVersion = "yourselves";
                    }
                    else
                    {
                        spanishVersion = "se";
                        englishVersion = "yourself";
                    }
                    break;
                case Identity.Other:
                    if (IsPlural)
                    {
                        spanishVersion = "se";
                        englishVersion = "themselves";
                    }
                    else
                    {
                        if (IsFeminine)
                        {
                            spanishVersion = "se";
                            englishVersion = "herself";
                        }
                        else
                        {
                            spanishVersion = "se";
                            englishVersion = "himself";
                        }
                    }
                    break;
                default:
                    throw new Exception("Unexpected role: " + Role);

            }

            var result = new Noun(spanishVersion, englishVersion, IsFeminine, IsPlural, Role);

            foreach (var tag in tags)
                result.WithTag(tag);

            return result;
        }

        public ITranslateable DefiniteArticle()
        {
            if (IsPlural)
            {
                return IsFeminine ? new CannedTranslation("las", "the") : new CannedTranslation("los", "the");
            }
            else
            {
                return IsFeminine ? new CannedTranslation("la", "the") : new CannedTranslation("el", "the");
            }
        }

        public ITranslateable IndefiniteArticle()
        {
            if (IsPlural)
            {
                return IsFeminine ? new CannedTranslation("unas", "some") : new CannedTranslation("una", "a");
            }
            else
            {
                return IsFeminine ? new CannedTranslation("unos", "some") : new CannedTranslation("uno", "a");
            }
        }

        public ITranslateable PossessedBySubjectArticle()
        {
            return new PossessiveAdjective(IsPlural, IsFeminine);
        }

        public PointOfView GetPointOfView()
        {
            switch(Role)
            {
                case Identity.Speaker:
                    return IsPlural ? PointOfView.FirstPersonPlural : PointOfView.FirstPerson;
                case Identity.Listener:
                    return IsPlural ? PointOfView.SecondPersonPlural : PointOfView.SecondPerson;
                case Identity.FormalListener:
                    return IsPlural ? PointOfView.SecondPersonPluralFormal : PointOfView.SecondPersonFormal;
                case Identity.Other:
                    return IsPlural ? IsFeminine ? PointOfView.ThirdPersonPluralFeminine : PointOfView.ThirdPersonPluralMasculine
                        : IsFeminine ? PointOfView.ThirdPersonFeminine : PointOfView.ThirdPersonMasculine;
                default:
                    throw new Exception("Unexpected Identity: " + Role);
            }
        }
    }
}
