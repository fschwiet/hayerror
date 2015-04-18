using System;
using CsQuery;
using NUnit.Framework;


namespace test
{
	[TestFixture]
	public class CsQueryTest
	{
		[Test]
		public void Misc ()
		{
			CsQuery.CQ document = "<td class=\"vtable-word\">est<span class=\"conj-irregular\">á</span></td>";

			var td = document ["td"].First ();

			//  https://github.com/jamietre/CsQuery/issues/186
			//Assert.AreEqual ("está", td.Text());


		}
	}
}

