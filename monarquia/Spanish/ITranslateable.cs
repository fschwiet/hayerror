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
	}

	//  All ITransalateables either implement NotComposed or Composed
	//  so anything that is actually not Composed can be implicitly
	//  converted to a Composed (since we can't do certain operator
	//  overloads on interfaces like ITranslteable)

	public abstract class NotComposed : ITranslateable {
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
