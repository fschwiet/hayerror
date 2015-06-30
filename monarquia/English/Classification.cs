using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monarquia.English
{
    class Classification
    {
        static readonly string[] AuxiliaryVerbs = new [] {
            "be",
            "can",
            "could",
            "dare",
            "do",
            "have",
            "may",
            "might",
            "must",
            "need",
            "ought",
            "shall",
            "should",
            "will",
            "would"
        };
        
        public static bool IsAuxiliaryVerb(string englishVerb)
        {
            // https://en.wikipedia.org/wiki/Auxiliary_verb#Auxiliary_verbs_vs._light_verbs
            return AuxiliaryVerbs.Contains(englishVerb.ToLower());
        }
    }
}
