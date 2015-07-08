using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{
    public enum Role
    {
        timeframe, 
        subject, 
        spanishonlyNoPreposition, 
        indirectObjectPronoun,
        directObjectProunoun,
        reflexivePronoun, 
        verbPhrase, 
        directObject, 
        indirectObject,
        verbEnding
    }

	public class CannedDataBuilder : ICannedData {

		List<ExpressableVerbRoleSelection> roleSelectors = new List<ExpressableVerbRoleSelection> ();
        List<IEnumerable<string>> TagPrioritizations = new List<IEnumerable<string>>();

		public void AddRoleSelector(
			VerbRoleSelector selector,
            IEnumerable<Role> spanishRolePattern = null,
            IEnumerable<Role> englishRolePattern = null)
        {

			roleSelectors.Add (new ExpressableVerbRoleSelection() {
				VerbRoleSelector = selector,
                SpanishRolePattern = spanishRolePattern ?? new[] { Role.timeframe, Role.subject, Role.spanishonlyNoPreposition, Role.indirectObjectPronoun, Role.directObjectProunoun, Role.reflexivePronoun, Role.verbPhrase, Role.directObject, Role.indirectObject, Role.verbEnding },
                EnglishRolePattern = englishRolePattern ?? new[] { Role.timeframe, Role.subject, Role.spanishonlyNoPreposition, Role.verbPhrase, Role.directObject, Role.indirectObject, Role.verbEnding }
			});
		}

		public IEnumerable<ExpressableVerbRoleSelection> GetAllVerbRoleSelectors()
		{
			return roleSelectors;
		}
        protected void AddLearningPriority(IEnumerable<ITranslateable> expressions)
        {
            List<string> tags = new List<string>();

            foreach(var expression in expressions)
            {
                foreach(var framing in Frame.SelectAllFrames())
                {
                    var result = expression.GetResult(framing);
                    var generatedTags = result.SelectMany(r => r.Tags).Distinct();

                    foreach(var generatedTag in generatedTags)
                    {
                        if (!tags.Contains(generatedTag))
                        {
                            tags.Add(generatedTag);
                        }
                    }
                }
            }

            TagPrioritizations.Add(tags);
        }

        public IEnumerable<IEnumerable<string>> GetPriorityGroups()
        {
            return TagPrioritizations;
        }
	}
	
}
