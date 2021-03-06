﻿using System;

namespace monarquia
{
	public class FollowsFrameMasculinityAndPlurality : ITranslateable
	{
		public string Masculine;
		public string Feminine;
		public string PluralMasculine;
		public string PluralFeminine;

		public string EnglishSingular;
		public string EnglishPlural;

		public FollowsFrameMasculinityAndPlurality (string masculine, string feminine, string pluralMasculine, string pluralFeminine)
		{
			Masculine = masculine;
			Feminine = feminine;
			PluralMasculine = pluralMasculine;
			PluralFeminine = pluralFeminine;
		}

		public FollowsFrameMasculinityAndPlurality (string singular, string plural) {
			Masculine = singular;
			Feminine = singular;
			PluralMasculine = plural;
			PluralFeminine = plural;
		}

		public FollowsFrameMasculinityAndPlurality WithTranslation(string singular, string plural) {
			EnglishSingular = singular;
			EnglishPlural = plural;
			return this;
		}

		public override System.Collections.Generic.IEnumerable<ResultChunk> GetResult (Frame frame)
		{
			return new [] {
				new ResultChunk() {
					SpanishTranslation = AsSpanish(frame),
					EnglishTranslation = AsEnglish(frame)
				}
			};
		}

		public string AsSpanish(Frame frame) {
			if (frame.PointOfView.IsFeminine () ?? false) {
				if (frame.PointOfView.IsPlural ()) {
					return PluralFeminine;
				} else {
					return Feminine;
				}
			} else {
				if (frame.PointOfView.IsPlural ()) {
					return PluralMasculine;
				} else {
					return Masculine;
				}
			}
		}

		public string AsEnglish(Frame frame) {
			if (frame.PointOfView.IsPlural())
				return EnglishPlural;
			else
				return EnglishSingular;
		}
	}
}

