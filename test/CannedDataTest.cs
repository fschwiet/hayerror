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

			var dataDirectory = "../../../data";
			var dataLoader = new DataLoader (dataDirectory);
			var cannedData = new BigCannedData (dataLoader);
			new monarquia.EspanolGenerator (cannedData, dataDirectory).GetExercises ();
		}
	}
}

