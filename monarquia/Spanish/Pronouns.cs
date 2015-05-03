using System;
using System.Collections.Generic;

namespace monarquia
{
	public static class Pronouns
	{
		public static IEnumerable<ITranslateable> GetSubjectNouns() {
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

	}
}

