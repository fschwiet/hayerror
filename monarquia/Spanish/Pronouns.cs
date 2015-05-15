﻿using System;
using System.Collections.Generic;
using System.Linq;
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
				new CannedTranslation ("tú", "you", frameFilter: frame => {
					return frame.PointOfView == PointOfView.SecondPerson;
				}).WithEnglishHint(),
				new CannedTranslation ("usted", "you", frameFilter: frame => {
					return frame.PointOfView == PointOfView.SecondPersonFormal;
				}).WithEnglishHint(),
				new CannedTranslation ("él", "he", frameFilter: frame => {
					return frame.PointOfView == PointOfView.ThirdPersonMasculine;
				}),
				new CannedTranslation ("ella", "she", frameFilter: frame => {
					return frame.PointOfView == PointOfView.ThirdPersonFeminine;
				}),
				new CannedTranslation ("nosotros", "we", frameFilter: frame => {
					return frame.PointOfView == PointOfView.FirstPersonPlural;
				}),
				new CannedTranslation ("vosotros", "you all", frameFilter: frame => {
					return frame.PointOfView == PointOfView.SecondPersonPlural;
				}).WithEnglishHint(),
				new CannedTranslation ("ustedes", "you all", frameFilter: frame => {
					return frame.PointOfView == PointOfView.SecondPersonPluralFormal;
				}),
				new CannedTranslation ("ellos", "they", frameFilter: frame => {
					return frame.PointOfView == PointOfView.ThirdPersonPluralMasculine;
				}).WithEnglishHint(),
				new CannedTranslation ("ellas", "they", frameFilter: frame => {
					return frame.PointOfView == PointOfView.ThirdPersonPluralFeminine;
				}).WithEnglishHint()
			};
		}

		//  http://www.studyspanish.com/lessons/iodopro.htm
		// http://www.studyspanish.com/lessons/reflexive2.htm
		public static IEnumerable<ITranslateable> GetCommonPeopleIndirectObject() {

			List<ITranslateable> results = new List<ITranslateable> ();

			results.Add (CannedTranslation.WithoutPointOfView ("me", "me", PointOfView.FirstPerson));
			results.Add (CannedTranslation.WithPointOfView    ("me", "myself", PointOfView.FirstPerson));

			results.Add (CannedTranslation.WithoutPointOfView ("te", "you", PointOfView.SecondPerson));
			results.Add (CannedTranslation.WithPointOfView    ("te", "yourself", PointOfView.SecondPerson));

			results.Add (CannedTranslation.WithoutPointOfView ("le", "you", PointOfView.SecondPersonFormal)
				.WithSpanishHint("usted").WithEnglishHint("formal"));
			results.Add (CannedTranslation.WithPointOfView    ("se", "yourself", PointOfView.SecondPersonFormal)
				.WithSpanishHint("usted"));

			results.Add (CannedTranslation.WithoutPointOfView ("le", "him", PointOfView.ThirdPersonMasculine)
				.WithSpanishHint ("masculine"));
			results.Add (CannedTranslation.WithPointOfView ("se", "himself", PointOfView.ThirdPersonMasculine)
				.WithSpanishHint ("masculine"));

			results.Add (CannedTranslation.WithoutPointOfView ("le", "her", PointOfView.ThirdPersonMasculine)
				.WithSpanishHint ("feminine"));
			results.Add (CannedTranslation.WithPointOfView ("se", "herself", PointOfView.ThirdPersonMasculine)
				.WithSpanishHint ("feminine"));

			results.Add (CannedTranslation.WithoutPointOfView ("nos", "us", PointOfView.FirstPersonPlural));
			results.Add (CannedTranslation.WithPointOfView    ("nos", "ourselves", PointOfView.FirstPersonPlural));

			results.Add (CannedTranslation.WithoutPointOfView ("os", "you all", PointOfView.SecondPersonPlural));
			results.Add (CannedTranslation.WithPointOfView    ("os", "yourselves", PointOfView.SecondPersonPlural));

			results.Add (CannedTranslation.WithoutPointOfView ("les", "you all", PointOfView.SecondPersonPluralFormal)
				.WithSpanishHint("ustedes"));
			results.Add (CannedTranslation.WithPointOfView    ("se", "yourselves", PointOfView.SecondPersonPluralFormal)
				.WithSpanishHint("ustedes"));

			results.Add (CannedTranslation.WithoutPointOfView ("les", "them", PointOfView.ThirdPersonPluralMasculine));
			results.Add (CannedTranslation.WithPointOfView    ("se", "themselves", PointOfView.ThirdPersonPluralMasculine));

			results.Add (CannedTranslation.WithoutPointOfView ("les", "them", PointOfView.ThirdPersonPluralFeminine));
			results.Add (CannedTranslation.WithPointOfView    ("se", "themselves", PointOfView.ThirdPersonPluralFeminine));
			return results;
		}
	}
}

