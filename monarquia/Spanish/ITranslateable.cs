using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{
	public abstract class ITranslateable {
		public abstract string AsSpanish(PointOfView pointOfView);
		public abstract string AsEnglish(PointOfView pointOfView);

		//  When ITranslateables are composed, GetEnglishHints() is
		//  only called on ITranslateables whose AsSpanish() is also
		//  called.
		public virtual IEnumerable<string> GetEnglishHints()
		{
			return new string[0];
		}

		public virtual IEnumerable<string> GetTags(Frame frame)
		{
			return new string[0];
		}

		public virtual IEnumerable<string> GetExtraHints()
		{
			return new string[0];
		}

		public virtual bool AllowsFraming(Frame frame) {
			return true;
		}

		public ITranslateable WithFrameRequirements(Func<Frame,bool> requirement) {
			return new TranslateableWithFrameRestriction(this, requirement);
		}
	}

	public class TranslateableWithFrameRestriction : NotComposed {

		ITranslateable translateable;
		Func<Frame,bool> restriction;

		public TranslateableWithFrameRestriction(ITranslateable translateable, Func<Frame,bool> restriction)
		{
			this.translateable = translateable;
			this.restriction = restriction;
		}

		public override bool AllowsFraming (Frame frame)
		{
			return this.restriction(frame) && base.AllowsFraming (frame);
		}

		public override string AsEnglish (PointOfView pointOfView)
		{
			return translateable.AsEnglish (pointOfView);
		}

		public override string AsSpanish (PointOfView pointOfView)
		{
			return translateable.AsSpanish (pointOfView);
		}

		public override IEnumerable<string> GetEnglishHints ()
		{
			return translateable.GetEnglishHints ();
		}

		public override IEnumerable<string> GetExtraHints ()
		{
			return translateable.GetExtraHints ();
		}

		public override IEnumerable<string> GetTags (Frame frame)
		{
			return translateable.GetTags (frame);
		}
	}

	//  All ITransalateables either implement NotComposed or Composed
	//  so anything that is actually not Composed can be implicitly
	//  converted to a Composed (since we can't do certain operator
	//  overloads on interfaces like ITranslteable)

	public abstract class NotComposed : ITranslateable {
	}

	public class SpanishOnly : NotComposed {

		string value;

		public SpanishOnly(string value) {
			this.value = value;
		}

		public SpanishOnly() {
		}

		public override string AsEnglish (PointOfView pointOfView)
		{
			throw new Exception ("AsEnglish() called on SpanishOnly instance");
		} 

		public override string AsSpanish (PointOfView pointOfView)
		{
			return value;
		}
	}

	public class EnglishOnly : NotComposed {

		string value;

		public EnglishOnly(string value) {
			this.value = value;
		}

		public EnglishOnly() {
		}

		public override string AsSpanish (PointOfView pointOfView)
		{
			throw new Exception ("AsSpanish() called on EnglishOnly instance");
		} 

		public override string AsEnglish (PointOfView pointOfView)
		{
			return value;
		}
	}

	public class Composed : ITranslateable {

		ITranslateable[] spanish;
		ITranslateable[] english;

		public Composed() {
		}

		public override string AsSpanish(PointOfView pointOfView) {
			return String.Join (" ", 
				spanish.Select(e => e.AsSpanish(pointOfView))
				.Where(s => !String.IsNullOrEmpty(s)));
		}

		public override string AsEnglish(PointOfView pointOfView) {
			return String.Join (" ", 
				english.Select(e => e.AsEnglish(pointOfView))
				.Where(s => !String.IsNullOrEmpty(s)));
		}

		public override IEnumerable<string> GetEnglishHints ()
		{
			return spanish.SelectMany (s => s.GetEnglishHints ());
		}

		public override IEnumerable<string> GetTags (Frame frame)
		{
			return spanish.SelectMany (s => s.GetTags (frame)).Concat (english.SelectMany (e => e.GetTags (frame))).Distinct ();
		}

		public override IEnumerable<string> GetExtraHints ()
		{
			return spanish.SelectMany (s => s.GetExtraHints ()).Concat (english.SelectMany (e => e.GetExtraHints ()));
		}

		public override bool AllowsFraming (Frame frame)
		{
			return spanish.All (s => s.AllowsFraming (frame))
			&& english.All (e => e.AllowsFraming (frame));
		}

		public Composed WithEnglishAlternative(ITranslateable english) {
			var result = new Composed ();
			result.spanish = (ITranslateable[])this.spanish.Clone ();
			result.english = new [] { english };
			return result;
		}

		public static implicit operator Composed(NotComposed a) {

			var result = new Composed();
			result.spanish = new [] {a};
			result.english = new [] {a};
			return result;
		}

		public static Composed operator +(Composed a, ITranslateable b) {
			var result = new Composed ();
			result.spanish = new [] { a, b };
			result.english = new [] { a, b };
			return result;
		}
	}
}
