using System;

namespace monarquia
{
	public class Article : EnglishOnly
	{
		public Article ()
		{
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

