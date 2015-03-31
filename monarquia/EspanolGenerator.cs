using System;
using System.Collections.Generic;

namespace monarquia
{
	public class EspanolGenerator
	{
		public EspanolGenerator ()
		{
		}

		public IEnumerable<string> GetAll(){
			return new [] { 
				"yo voy",
				"tú vas", 
				"él va",
				"ella va",
				"usted va",
				"vosotros vais",
				"ellos van",
				"ellas van",
				"ustedes van"};
		}
	}
}

