using System;
using System.Collections.Generic;

namespace monarquia
{
	public static class Pronouns
	{
		public static IEnumerable<ITranslateable> GetCommonPeopleSubjectNouns() {
			return new [] {
				new CannedTranslation ("yo", "I", frameFilter: frame => {
					return frame.PointOfView == PointOfView.FirstPerson;
				}),
				new CannedTranslation ("tú", "you", true, frameFilter: frame => {
					return frame.PointOfView == PointOfView.SecondPerson;
				}),
				new CannedTranslation ("usted", "you", true, frameFilter: frame => {
					return frame.PointOfView == PointOfView.SecondPersonFormal;
				}),
				new CannedTranslation ("él", "he", frameFilter: frame => {
					return frame.PointOfView == PointOfView.ThirdPersonMasculine;
				}),
				new CannedTranslation ("ella", "she", frameFilter: frame => {
					return frame.PointOfView == PointOfView.ThirdPersonFeminine;
				}),
				new CannedTranslation ("nosotros", "we", frameFilter: frame => {
					return frame.PointOfView == PointOfView.FirstPersonPlural;
				}),
				new CannedTranslation ("vosotros", "you all", true, frameFilter: frame => {
					return frame.PointOfView == PointOfView.SecondPersonPlural;
				}),
				new CannedTranslation ("ustedes", "you all", frameFilter: frame => {
					return frame.PointOfView == PointOfView.SecondPersonPluralFormal;
				}),
				new CannedTranslation ("ellos", "they", true, frameFilter: frame => {
					return frame.PointOfView == PointOfView.ThirdPersonPluralMasculine;
				}),
				new CannedTranslation ("ellas", "they", true, frameFilter: frame => {
					return frame.PointOfView == PointOfView.ThirdPersonPluralFeminine;
				})
			};
		}

		//  We're assuming third person direct objects are never the same
		//  http://www.studyspanish.com/lessons/iodopro.htm
		// http://www.studyspanish.com/lessons/reflexive2.htm
		public static IEnumerable<ITranslateable> GetCommonPeopleIndirectObject() {
			List<ITranslateable> results = new List<ITranslateable> ();

			Action<string,string> addObjectAsTransitiveThoughItCouldBeReflexive = 
				delegate (string spanish, string english) {
					results.Add(new CannedTranslation(spanish, english));
				};

			Action<PointOfView, string, string, string, string> addNecessarilyReflexivelyObject = 
				delegate(PointOfView pointOfView, string spanish, string spanishReflexive, string english, string englishReflexive) {
					results.Add(new CannedTranslation(spanishReflexive, englishReflexive, 
						frameFilter: frame => frame.PointOfView == pointOfView));
					results.Add(new CannedTranslation(spanish, english, 
						frameFilter: frame => frame.PointOfView != pointOfView));
				};

			addObjectAsTransitiveThoughItCouldBeReflexive ("le", "him");
			addObjectAsTransitiveThoughItCouldBeReflexive ("le", "her");
			addObjectAsTransitiveThoughItCouldBeReflexive ("les", "them");
			addObjectAsTransitiveThoughItCouldBeReflexive ("les", "them");

			addNecessarilyReflexivelyObject (PointOfView.FirstPerson, "me", "me", "me", "myself");
			addNecessarilyReflexivelyObject (PointOfView.FirstPersonPlural, "nos", "nos", "us", "ourselves");
			addNecessarilyReflexivelyObject (PointOfView.SecondPerson, "te", "te", "you", "yourself");
			//addNecessarilyReflexivelyObject (PointOfView.SecondPersonPlural, "os", "os", "you", "yourselves");

			return results;
		}
	}
}

