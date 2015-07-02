using System;
using System.Collections.Generic;
using System.Linq;


namespace monarquia
{
	public class EnglishVerbConjugator : VerbConjugator
	{
		VerbConjugator inner;

		public EnglishVerbConjugator (VerbConjugator inner)
		{
			this.inner = inner;
		}

		public override string ConjugatedForTense (Frame frame)
		{
			var innerResult = inner.ConjugatedForTense (frame);

			if (!frame.IsNegated)
				return innerResult;

            if (!English.Classification.IsAuxiliaryVerb(Infinitive))
            {
                if (frame.Conjugation == Conjugation.Present)
                {

                    var newFrame = frame.Clone();

                    var prefix = "do not ";

                    if (newFrame.PointOfView.IsThirdPerson())
                    {
                        prefix = "does not ";

                        newFrame = newFrame.Clone(pointOfView: PointOfView.FirstPerson);
                    }

                    return prefix + inner.ConjugatedForTense(newFrame);
                }
                else if (
                  frame.Conjugation == Conjugation.PastImperfect ||
                  frame.Conjugation == Conjugation.PastPreterite)
                {

                    var newFrame = frame.Clone(conjugation: Conjugation.Present, pointOfView: PointOfView.FirstPerson);

                    return "did not " + inner.ConjugatedForTense(newFrame);
                }

            }

			var innerResultParts = innerResult.Split (new [] { ' ' }).ToList ();
			innerResultParts.Insert (1, "not");
			return string.Join (" ", innerResultParts);
		}
		public override string Infinitive {
			get {
				return inner.Infinitive;
			}
		}
	}
}

