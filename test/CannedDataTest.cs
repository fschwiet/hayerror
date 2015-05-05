using System;
using NUnit.Framework;

using monarquia;

namespace test
{
	[TestFixture]
	public class CannedDataTest
	{
		[Test]
		public void DoesntBlowUp ()
		{
			//  This code is common to a lot of TestFixtureSetup methods, but is hard to debug
			//  there when it starts throwing exceptions.  So this test just runs to give more
			//  information about those TestFixtureSetup methods.

			new DataLoader ("../../../data");
			new BigCannedData ();
		}
	}
}

