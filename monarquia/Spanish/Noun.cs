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
                bool isFeminine = false, bool isPlural = false, bool isSubject = false) 
            : base(spanishVersion, englishVersion)
        {
            this.WithTag("noun:" + spanishVersion + "-" + englishVersion);

            IsFeminine = isFeminine;
            IsPlural = isPlural;
            IsSubject = isSubject;
        }   

        public bool IsFeminine { get; private set; }
        public bool IsPlural { get; private set; }
        public bool IsSubject { get; private set; }

        public override bool AllowsFraming(Frame frame)
        {
            if (!IsSubject)
                return true;

            if (IsPlural != frame.PointOfView.IsPlural())
                return false;

            if (IsFeminine != frame.PointOfView.IsFeminine())
                return false;

            return true;
        }
        
        public ITranslateable DefiniteArticle()
        {
            if (IsPlural)
            {
                return IsFeminine ? new CannedTranslation("las", "the") : new CannedTranslation("la", "the");
            }
            else
            {
                return IsFeminine ? new CannedTranslation("los", "the") : new CannedTranslation("el", "the");
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
    }
}
