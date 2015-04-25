using System;

namespace monarquia
{
	public interface ITranslateable {
		string AsSpanish(PointOfView pointOfView);
		string AsEnglish(PointOfView pointOfView);
	}

	public class TranslationNotImplemented : ITranslateable {

		string value;

		public TranslationNotImplemented(string value) {
			this.value = value;
		}

		public string AsSpanish(PointOfView pointOfView) {
			return value;
		}

		public string AsEnglish(PointOfView pointOfView) {
			throw new Exception("not implemented");
		}
	}
	
}
