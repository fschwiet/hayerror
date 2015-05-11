using System;
using NUnit.Framework;
using monarquia;

namespace test
{
	[TestFixture]
	public class ArticleAwareness
	{
		[Test]
		[TestCase("car", "A car.")]
		[TestCase("university", "A university.")]
		[TestCase("user", "A user.")]
		[TestCase("elephant", "An elephant.")]
		[TestCase("egg", "An egg.")]
		[TestCase("idiot", "An idiot.")]
		[TestCase("apple", "An apple.")]
		[TestCase("orphan", "An orphan.")]
		[TestCase("hour", "An hour.")]
		[TestCase("horse", "A horse.")]
		public void TransformsAtoAn (string input, string expected)
		{
			var phoneticData = new CachedPhoneticData("../../../data");

			var result = EspanolGenerator.MakeEnglishSentence(phoneticData, new [] {
					new ResultChunk() {
						EnglishTranslation = "a",
						EnglishHint = new string[0]
					}, 
					new ResultChunk() {
						EnglishTranslation = input,
						EnglishHint = new string[0]	
					}
				});

			Assert.AreEqual(expected, result);
		}
	}
}

