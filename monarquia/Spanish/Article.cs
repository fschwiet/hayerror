using System;

namespace monarquia
{
	public class Article : NotComposed
	{
		public Article ()
		{
		}

		public override string AsSpanish (PointOfView pointOfView)
		{
			throw new NotImplementedException ();
		}

		public override string AsEnglish (PointOfView pointOfView)
		{
			if (pointOfView.IsPlural()) {
				return "";
			} else {
				return "a";
			}
		}
	}
}

