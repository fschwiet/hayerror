using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monarquia
{
    public class RoleSelections
    {
        Dictionary<Role, RoleSelection> SpanishRoles = new Dictionary<Role,RoleSelection>();
        Dictionary<Role, RoleSelection> EnglishRoles = new Dictionary<Role,RoleSelection>();

        public RoleSelections Clone()
        {
            return new RoleSelections()
            {
                SpanishRoles = new Dictionary<Role, RoleSelection>(this.SpanishRoles),
                EnglishRoles = new Dictionary<Role, RoleSelection>(this.EnglishRoles)
            };
        }

        public RoleSelections WithRole(Role role, RoleSelection value)
        {
            var result = this.Clone();

            result.SpanishRoles.Add(role, value);
            result.EnglishRoles.Add(role, value);

            return result;
        }

        public RoleSelection GetForRole(Role role)
        {
            if (!SpanishRoles.ContainsKey(role) && !EnglishRoles.ContainsKey(role))
                return null;

            if (SpanishRoles[role] != EnglishRoles[role])
                throw new Exception("GetForRole called when Spanish and English have different roles.");

            return SpanishRoles[role];
        }

        public ITranslateable GetForSpanishRole(Role role)
        {
            RoleSelection result;

            if (!this.SpanishRoles.TryGetValue(role, out result))
                return null;

            return result.Value;
        }

        public ITranslateable GetForEnglishRole(Role role)
        {
            RoleSelection result;

            if (!this.EnglishRoles.TryGetValue(role, out result))
                return null;

            return result.Value;
        }

        public RoleSelections MakeSpanishReflexive()
        {
            var result = this.Clone();

            var spanishSubject = result.SpanishRoles[Role.subject];

            result.SpanishRoles[Role.indirectObjectPronoun]
                = new RoleSelection(spanishSubject.UnderlyingObject.ReflexivePronoun(), spanishSubject.UnderlyingObject);

            return result;
        }

        public RoleSelections MakeIndirectObjectPronoun()
        {
            var spanishUnderlyingSubject = this.SpanishRoles[Role.subject].UnderlyingObject;
            var englishUnderlyingSubject = this.EnglishRoles[Role.subject].UnderlyingObject;
            var spanishUnderlyingIndirectObject = this.SpanishRoles[Role.indirectObject].UnderlyingObject;
            var englishUnderlyingIndirectObject = this.EnglishRoles[Role.indirectObject].UnderlyingObject;

            var spanishReflexive = spanishUnderlyingSubject != null
                && spanishUnderlyingSubject == spanishUnderlyingIndirectObject;

            var englishReflexive = englishUnderlyingSubject != null
                && englishUnderlyingSubject == englishUnderlyingIndirectObject;

            var result = this.Clone();

            result.SpanishRoles[Role.indirectObjectPronoun] =
                new RoleSelection(spanishReflexive ? spanishUnderlyingIndirectObject.ReflexivePronoun() : spanishUnderlyingIndirectObject.IndirectObjectPronoun(),
                                  spanishUnderlyingIndirectObject);

            result.SpanishRoles.Remove(Role.indirectObject);

            result.EnglishRoles[Role.indirectObject] =
                new RoleSelection(englishReflexive ? englishUnderlyingIndirectObject.ReflexivePronoun() : englishUnderlyingIndirectObject.IndirectObjectPronoun(),
                                  englishUnderlyingIndirectObject);
            return result;
        }
    }
}
