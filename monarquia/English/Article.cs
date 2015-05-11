using System;

namespace monarquia
{
	public class Article : EnglishOnly
	{
		public Article ()
		{
		}

		public override System.Collections.Generic.IEnumerable<ResultChunk> GetResult (Frame frame)
		{
			return new [] {
				new ResultChunk () {
					EnglishTranslation = frame.PointOfView.IsPlural () ? "" : "a"
				}
			};
		}
	}
}

