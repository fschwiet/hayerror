using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{
	public abstract class ITranslateable {

		public abstract IEnumerable<ResultChunk> GetResult (Frame frame);

		public virtual bool AllowsFraming(Frame frame) {
			return true;
		}

		public ITranslateable WithFrameRequirements(Func<Frame,bool> requirement) {
			return new TranslateableWithFrameRestriction(this, requirement);
		}

		public static ITranslateable operator +(ITranslateable a, ITranslateable b) {
			var result = new Composed (new [] { a, b }, new [] { a, b });
			return result;
		}

		public Composed WithEnglishAlternative(ITranslateable english) {
			var result = new Composed (new [] { this }, new [] { english });

			return result;
		}

		public Composed WithEnglishAlternative(string englsh) {
			return WithEnglishAlternative (new EnglishOnly (englsh));
		}
	}

	public class TranslateableWithFrameRestriction : ITranslateable {

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

		public override IEnumerable<ResultChunk> GetResult (Frame frame)
		{
			return translateable.GetResult (frame);
		}
	}

	public class SpanishOnly : ITranslateable {

		string value;

		public SpanishOnly(string value) {
			this.value = value;
		}

		public SpanishOnly() {
		}

		public override IEnumerable<ResultChunk> GetResult (Frame frame)
		{
			return new [] {
				new ResultChunk() {
					SpanishTranslation = value
				}
			};
		}
	}

	public class EnglishOnly : ITranslateable {

		string value;

		public EnglishOnly(string value) {
			this.value = value;
		}

		public EnglishOnly() {
		}

		public override IEnumerable<ResultChunk> GetResult (Frame frame)
		{
			return new [] {
				new ResultChunk() {
					EnglishTranslation = value
				}
			};
		}
	}

	public class Composed : ITranslateable {

		ITranslateable[] spanish;
		ITranslateable[] english;

		public Composed(IEnumerable<ITranslateable> spanish, IEnumerable<ITranslateable> english) {
			this.spanish = spanish.ToArray();
			this.english = english.ToArray();
		}

		public override IEnumerable<ResultChunk> GetResult (Frame frame)
		{
			var spanishChunks = spanish.SelectMany (s => s.GetResult (frame)).ToArray ();
			var englishChunks = english.SelectMany (e => e.GetResult (frame)).ToArray ();

			List<ResultChunk> results = new List<ResultChunk> ();

			foreach (var spanishChunk in spanish.SelectMany(s => s.GetResult(frame))) {
				results.Add (new ResultChunk () {
					SpanishTranslation = spanishChunk.SpanishTranslation,
					SpanishHint = spanishChunk.SpanishHint,
					EnglishTranslation = "",
					EnglishHint = new string[0],
					ExtraInfo = spanishChunk.ExtraInfo,
					Tags = spanishChunk.Tags
				});
			}

			foreach (var englishChunk in english.SelectMany(s => s.GetResult(frame))) {
				results.Add (new ResultChunk () {
					SpanishTranslation = "",
					SpanishHint = new string[0],
					EnglishTranslation = englishChunk.EnglishTranslation,
					EnglishHint = englishChunk.EnglishHint,
					ExtraInfo = englishChunk.ExtraInfo,
					Tags = englishChunk.Tags
				});
			}

			return results;
		}

		public override bool AllowsFraming (Frame frame)
		{
			return spanish.All (s => s.AllowsFraming (frame))
			&& english.All (e => e.AllowsFraming (frame));
		}
	}
}
