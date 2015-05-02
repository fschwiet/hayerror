using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace monarquia
{
	public class LingueeGenerator : ExerciseGenerator
	{
		public LingueeGenerator (string dataDirectory) : base(dataDirectory)
		{
		}

		public List<Exercise> ForVerb(ChromeDriver webDriver, string infinitive) {

			List<Exercise> results = new List<Exercise> ();

			var verb = dataLoader.GetAllSavedSpanishVerbs().Single(v => v.Infinitive == infinitive);

			var pointOfViewGroupedByConjugationEffect = new [] {
				new [] { PointOfView.FirstPerson },
				new [] { PointOfView.FirstPersonPlural },
				new [] { PointOfView.SecondPerson },
				new [] { PointOfView.SecondPersonPluralFormal },
				new [] { PointOfView.ThirdPersonMasculine, PointOfView.ThirdPersonFeminine, PointOfView.SecondPersonFormal },
				new [] { PointOfView.ThirdPersonPluralMasculine, PointOfView.ThirdPersonPluralFeminine, PointOfView.SecondPersonPluralFormal }
			};

			foreach (var conjugation in Enum.GetValues(typeof(Conjugation)).Cast<Conjugation>()) {
				foreach (var pointsOfView in pointOfViewGroupedByConjugationEffect) {

					List<Exercise> pointOfViewExercises = new List<Exercise> ();

					var exerciseTemplate = new Exercise () {
						ExtraInfo = verb.Infinitive,
					};

					exerciseTemplate.Tags.Add ("drill:linguee");
					exerciseTemplate.Tags.Add ("conjugation:" + conjugation);

					foreach (var pointOfView in pointsOfView) {

						var exercise = exerciseTemplate.Clone ();

						var basePhrase = pointOfView.GetSubjectNoun ().AsSpanish(pointOfView) + " " +
							verb.ForSpanishConjugation (conjugation).AsSpanish(pointOfView);

						if (pointOfView.IsSecondPerson ())
							exercise.HintsForTranslated.Add (pointOfView.GetSubjectNoun().AsSpanish(pointOfView));

						var lingueeResults = DoLingueeLookup (webDriver, basePhrase);

						exercise.Original = lingueeResults.Keys.OrderBy (k => Exercise.WeighString(k)).FirstOrDefault();

						if (exercise.Original != null) {
							exercise.Translated = lingueeResults [exercise.Original];
							pointOfViewExercises.Add (exercise);
						}
					}

					if (!pointOfViewExercises.Any ()) {

						var basePhrase = verb.ForSpanishConjugation (conjugation).AsSpanish(pointsOfView.First ());
						var lingueeResults = DoLingueeLookup (webDriver, basePhrase);

						if (!lingueeResults.Any ())
							throw new Exception (String.Format("No linguee results found for {0} of {1} as {2}", verb.Infinitive, conjugation, basePhrase));

						var exercise = exerciseTemplate.Clone ();

						exercise.Original = lingueeResults.Keys.OrderBy (k => Exercise.WeighString(k)).FirstOrDefault();
						exercise.Translated = lingueeResults [exercise.Original];

						pointOfViewExercises.Add (exercise);
					}

					results.Add (pointOfViewExercises.OrderBy(e => e.GetWeight()).First());
				}
			}

			// since we sometimes regress to not including the object pronoun, its possible
			// we've done duplicated lookups.  Remove the dupes now.
			results = results.GroupBy (r => r.Original).Select (y => y.First ()).ToList ();

			return results;
		}

		public static Dictionary<string, string> DoLingueeLookup (ChromeDriver chromeDriver, string target)
		{
			var lingueeLookup = "http://www.linguee.com/english-spanish/search?source=auto&query=\"" + System.Net.WebUtility.UrlEncode (target) + "\"";

			try {
				Dictionary<string, string> results = new Dictionary<string, string> ();

				chromeDriver.Navigate ().GoToUrl (lingueeLookup);

				// This elements are difficult to remove for output selectively,
				// so I'm just removing them all from the page.
				chromeDriver.ExecuteScript ("$('.source_url_spacer').remove()");
				chromeDriver.ExecuteScript ("$('.source_url').remove()");
				chromeDriver.ExecuteScript ("$('.behindLinkDiv').remove()");

				foreach (var example in chromeDriver.FindElementsByCssSelector ("tbody.examples tr")) {
					var elements = example.FindElements (By.TagName ("td")).ToArray ();
					var sample = elements [0].Text;
					var translation = elements [1].Text;
					results [sample] = translation;
				}

				return results;
			}
			catch(Exception e) {
				throw new Exception ("Linguee failure for url: " + lingueeLookup, e);
			}
		}
	}
}

