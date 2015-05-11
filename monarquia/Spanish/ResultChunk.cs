using System;
using System.Collections.Generic;
using System.Linq;
namespace monarquia
{
	public class ResultChunk
	{
		public string EnglishTranslation;
		public string SpanishTranslation;
		public IEnumerable<string>  EnglishHint = new string[0];
		public IEnumerable<string>  SpanishHint = new string[0];
		public IEnumerable<string> Tags = new string[0];
		public IEnumerable<string>  ExtraInfo = new string[0];
	}
}

