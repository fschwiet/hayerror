using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monarquia
{
    public class BelongingNoun : Noun
    {
        public Identity Owner;
        public Noun Owned;

        public static BelongingNoun CreateFor(Identity owner, Noun owned) 
        {
            BelongingNoun result = null;
            var spanishPostfix = owned.IsPlural ? "s" : "";

            switch (owner)
            {
                case Identity.Speaker:
                    result = new BelongingNoun(owned, "mi" + spanishPostfix, "my");
                    break;
                default:
                    throw new NotImplementedException();
            }

            result.Owned = owned;
            result.Owner = owner;

            return result;
        }

        BelongingNoun(Noun original, string spanishPrefix, string englishPrefix)
            : base(original)
        {
            this.spanishText = spanishPrefix + " " + this.spanishText;
            this.englishText = englishPrefix + " " + this.englishText;
        }
    }
}
