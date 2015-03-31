using NUnit.Framework;
using System;
using System.Linq;

namespace test
{
	[TestFixture ()]
	public class CanGenerateSpanish
	{
		[Test ()]
		public void TestCase ()
		{
			var results = new monarquia.EspanolGenerator().GetAll();

			Assert.Contains ("yo voy", results.ToArray());
		}
	}
}

