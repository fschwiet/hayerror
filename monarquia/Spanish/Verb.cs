using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{
	public abstract class Verb {
		
		protected Verb() {
		}

		public abstract string Infinitive {
			get;
		}

		public abstract string ConjugatedForTense (Conjugation conjugation, PointOfView pointOfView);

		public ITranslateable Conjugation(Conjugation conjugation, Verb englishVerb) 
		{
			return new VerbInstance (this, englishVerb, conjugation);
		}
	}

	public class CannedVerb : Verb
	{
		readonly string infinitive;
		Dictionary<Conjugation, Dictionary<PointOfView, string>> tenses;

		public CannedVerb(string infinitiv) {
			this.infinitive = infinitiv;
			this.tenses = new Dictionary<Conjugation, Dictionary<PointOfView, string>> ();
		}

		public Verb WithTenses (Conjugation conjugation, Dictionary<PointOfView, string> value) {

			if (value.Values.Any(v => v.Contains(","))) {
				throw new Exception(String.Format("WithTenses passed invalid value for verb {0} for conjugation {1}.", infinitive, conjugation));
			}

			tenses [conjugation] = value;
			return this;
		}

		public override string Infinitive {
			get {
				return infinitive;
			}
		}

		public override string ConjugatedForTense(Conjugation conjugation, PointOfView pointOfView) {
			return tenses [conjugation][pointOfView];
		}

		public void MakeThirdPersonPluralMatchSingular() {
			//  http://spanish.about.com/cs/verbs/a/haber_as_there.htm

			foreach (var conjugation in tenses.Keys) {

				tenses [conjugation] [PointOfView.ThirdPersonPluralMasculine] = 
					tenses [conjugation] [PointOfView.ThirdPersonMasculine];

				tenses [conjugation] [PointOfView.ThirdPersonPluralFeminine] = 
					tenses [conjugation] [PointOfView.ThirdPersonFeminine];
			}
		}
	}

	public class ReflexiveVerb : Verb
	{
		Verb underlyingVerb;

		public ReflexiveVerb (string infinitive, DataLoader dataLoader)
		{
			this.underlyingVerb = dataLoader.GetAllSavedSpanishVerbs ().Single (v => v.Infinitive == infinitive);
		}

		public override string Infinitive {
			get {
				return this.underlyingVerb.Infinitive + "se";
			}
		}

		public override string ConjugatedForTense (Conjugation conjugation, PointOfView pointOfView)
		{
			var original = this.underlyingVerb.ConjugatedForTense (conjugation, pointOfView);

			return GetReflexiveArticle (pointOfView) + " " + original;
		}

		string GetReflexiveArticle(PointOfView pointOfView) 
		{
			switch (pointOfView) {
			case PointOfView.FirstPerson:
				return "me";
			case PointOfView.FirstPersonPlural:
				return "nos";
			case PointOfView.SecondPerson:
				return "te";
			case PointOfView.SecondPersonFormal:
				return "se";
			case PointOfView.SecondPersonPlural:
				return "os";
			case PointOfView.SecondPersonPluralFormal:
				return "se";

			case PointOfView.ThirdPersonFeminine:	
			case PointOfView.ThirdPersonMasculine:
			case PointOfView.ThirdPersonPluralFeminine:
			case PointOfView.ThirdPersonPluralMasculine:
				return "se";
			default:
				throw new InvalidOperationException ();
			}
		}
	}

	public class VerbInstance : ITranslateable
	{
		Verb spanishVerb;
		Verb englishVerb;
		Conjugation conjugation;

		public VerbInstance(Verb spanishVerb, Verb englishVerb, Conjugation conjugation)
		{
			this.spanishVerb = spanishVerb;
			this.englishVerb = englishVerb;
			this.conjugation = conjugation;
		}

		public override IEnumerable<ResultChunk> GetResult (Frame frame)
		{
			return new [] { new ResultChunk () {
				SpanishTranslation = spanishVerb.ConjugatedForTense (frame.Conjugation, frame.PointOfView),
				EnglishTranslation = englishVerb == null ? null : englishVerb.ConjugatedForTense (frame.Conjugation, frame.PointOfView),
				SpanishHint = new string[0],
				EnglishHint = new string[0],
				Tags = new [] { "verb:" + spanishVerb.Infinitive, "conjugation:" + conjugation, frame.PointOfView.AsTagString () },
				ExtraInfo = new [] { "verb " + spanishVerb.Infinitive, conjugation.AsFriendlyString () }
				}
			};
		}
	}
}

