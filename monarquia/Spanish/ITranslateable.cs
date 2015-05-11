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

			return new [] {
				new ResultChunk () {
					SpanishTranslation = string.Join(" ", spanishChunks.Where(r => r.SpanishTranslation != null).Select(r => r.SpanishTranslation)),
					EnglishTranslation = string.Join(" ", englishChunks.Where(r => r.EnglishTranslation != null).Select(r => r.EnglishTranslation)),
					SpanishHint = spanishChunks.SelectMany (r => r.SpanishHint),
					EnglishHint = englishChunks.SelectMany (r => r.EnglishHint),
					Tags = spanishChunks.SelectMany(s => s.Tags).Concat(englishChunks.SelectMany(e => e.Tags)).Distinct(),
					ExtraInfo = spanishChunks.SelectMany(s => s.ExtraInfo).Concat(englishChunks.SelectMany(e => e.ExtraInfo))
				}
			};
		}

		public override bool AllowsFraming (Frame frame)
		{
			return spanish.All (s => s.AllowsFraming (frame))
			&& english.All (e => e.AllowsFraming (frame));
		}
	}
}
