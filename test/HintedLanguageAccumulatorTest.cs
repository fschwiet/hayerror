using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Text;
using monarquia;


namespace test
{
	[TestFixture]
	public class HintedLanguageAccumulatorTest
	{
		[Test]
		public void CanGenerateSentence() {
		
			var accumulator = new HintedLanguageAccumulator ();
			accumulator.Add ("hello");
			accumulator.Add ("world");
			accumulator.Add ("");

			var sentence = accumulator.GetResult ();
			Assert.AreEqual ("Hello world.", sentence);
		}

		[Test]
		public void CanInsertHints() {

			var accumulator = new HintedLanguageAccumulator ();
			accumulator.Add ("you", new [] { "usted", "singular" } );
			accumulator.Add ("are");
			accumulator.Add ("here");

			var sentence = accumulator.GetResult ();
			Assert.AreEqual ("(usted, singular) You are here.", sentence);
		}

		[Test]
		public void CanApplyContractions()
		{
			var accumulated = new HintedLanguageAccumulator ();
			accumulated.Add ("we");
			accumulated.Add ("are");
			accumulated.Add ("who");
			accumulated.Add ("we");
			accumulated.Add ("are");

			accumulated.ApplyContraction ("we", "are", "we're");

			Assert.AreEqual ("We're who we are.", accumulated.GetResult ());
		}

		[Test]
		public void CanApplyContextualTransforms()
		{
			var accumulated = new HintedLanguageAccumulator ();
			accumulated.Add ("a");
			accumulated.Add ("apple");
			accumulated.Add ("is");
			accumulated.Add ("a");
			accumulated.Add ("fruit");

			accumulated.ApplyTransform ("a", next => next.StartsWith ("a"), "an");

			Assert.AreEqual ("An apple is a fruit.", accumulated.GetResult ());
		}
	}
}

