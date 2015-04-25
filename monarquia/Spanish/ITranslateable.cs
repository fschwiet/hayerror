using System;
using System.Linq;

namespace monarquia
{
	public interface ITranslateable {
		string AsSpanish(PointOfView pointOfView);
		string AsEnglish(PointOfView pointOfView);
	}

	//  All ITransalateables either implement NotComposed or Composed
	//  so anything that is actually not Composed can be implicitly
	//  converted to a Composed (since we can't do certain operator
	//  overloads on interfaces like ITranslteable)

	public abstract class NotComposed : ITranslateable {

		public abstract string AsSpanish(PointOfView pointOfView);
		public abstract string AsEnglish(PointOfView pointOfView);
	}

	public class Composed : ITranslateable {

		public ITranslateable[] spanish;
		public ITranslateable[] english;

		public Composed() {
		}

		public string AsSpanish(PointOfView pointOfView) {
			return String.Join (" ", spanish.Select(e => e.AsSpanish(pointOfView)));
		}

		public string AsEnglish(PointOfView pointOfView) {
			return String.Join (" ", english.Select(e => e.AsEnglish(pointOfView)));
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

	public class TranslationNotImplemented : NotComposed {

		public class TranslatedNotImplementedException : Exception {
		}

		string value;

		public TranslationNotImplemented(string value) {
			this.value = value;
		}

		public override string AsSpanish(PointOfView pointOfView) {
			return value;
		}

		public override string AsEnglish(PointOfView pointOfView) {
			throw new TranslatedNotImplementedException();
		}
	}
	
}
