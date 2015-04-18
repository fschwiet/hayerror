using System;
using System.Linq;
using CsQuery;
using NUnit.Framework;


namespace test
{
	[TestFixture]
	public class CsQueryTest
	{
		[Test]
		public void TextIncludesSubElements ()
		{
			CsQuery.CQ document = "<td class=\"vtable-word\">est<span class=\"conj-irregular\">á</span></td>";

			var td = document ["td"].First ();

			Assert.AreEqual ("está", td.Text());
		}

		[Test]
		public void InnerTextIncludesSubElements ()
		{
			CsQuery.CQ document = "<td class=\"vtable-word\">est<span class=\"conj-irregular\">á</span></td>";

			//  https://github.com/jamietre/CsQuery/issues/186

			foreach (var element in document["td"]) {
				IDomObject e = element;
				Assert.AreEqual ("está", e.Cq().Text());
			}
		}

		//[Test]
		public void CanParseEstarFile() {
			CsQuery.CQ document = System.IO.File.ReadAllText ("../../../data/estar.conjugation.txt");

			var entries = document ["a.vtable-label:eq(0) + .vtable-wrapper tr td"];

			foreach (var entry in entries) {
				Console.WriteLine("contents: " + ((CsQuery.CQ)entry.InnerHTML).Text());
			}
		
			/*
			var rows = document ["a.vtable-label:eq(0) + .vtable-wrapper tr"];

			foreach (var row in rows) {
				foreach (var entry in row.ChildNodes) {
					Console.WriteLine("contents: " + entry.InnerText);
				}
			}
			*/

			throw new Exception ("hi");
		}
	}
}

