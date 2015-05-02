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

		public ITranslateable ForSpanishConjugation(Conjugation conjugation) 
		{
			return new SpanishVerbInstance (this, conjugation);
		}

		public ITranslateable ForEnglishConjugation(Conjugation conjugation)
		{
			return new EnglishVerbInstance (this, conjugation);
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

	public class SpanishVerbInstance : ITranslateable
	{
		Verb verb;
		Conjugation conjugation;

		public SpanishVerbInstance(Verb verb, Conjugation conjugation)
		{
			this.verb = verb;
			this.conjugation = conjugation;
		}

		public override string AsSpanish (PointOfView pointOfView)
		{
			return verb.ConjugatedForTense (conjugation, pointOfView);
		}

		public override string AsEnglish (PointOfView pointOfView)
		{
			throw new InvalidOperationException ();
		}
	}


	public class EnglishVerbInstance : ITranslateable
	{
		Verb verb;
		Conjugation conjugation;

		public EnglishVerbInstance(Verb verb, Conjugation conjugation)
		{
			this.verb = verb;
			this.conjugation = conjugation;
		}

		public override string AsSpanish (PointOfView pointOfView)
		{
			throw new InvalidOperationException ();
		}

		public override string AsEnglish (PointOfView pointOfView)
		{
			return verb.ConjugatedForTense (conjugation, pointOfView);
		}
	}
}

