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

		public ITranslateable GetTranslateable(Conjugation conjugation, ICannedData cannedData, DataLoader loader) 
		{
			return new VerbInstance (this, conjugation, cannedData, loader);
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

	public class VerbInstance : ITranslateable
	{
		Verb spanishVerb;
		Verb englishVerb;
		Conjugation conjugation;

		public VerbInstance(Verb spanishVerb, Conjugation conjugation, ICannedData cannedData, DataLoader dataLoader)
		{
			this.spanishVerb = spanishVerb;

			if (cannedData != null)
				this.englishVerb = cannedData.TranslateVerbFromSpanishToEnglish (dataLoader, spanishVerb, conjugation);
			
			this.conjugation = conjugation;
		}

		public override string AsSpanish (PointOfView pointOfView)
		{
			return spanishVerb.ConjugatedForTense (conjugation, pointOfView);
		}

		public override string AsEnglish (PointOfView pointOfView)
		{
			if (englishVerb == null)
				throw new Exception ("Verb missing translation");

			return englishVerb.ConjugatedForTense (conjugation, pointOfView);
		}
	}
}

