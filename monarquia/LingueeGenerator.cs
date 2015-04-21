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

			var verb = LookupVerb (infinitive);

			foreach (var conjugation in Enum.GetValues(typeof(Verb.Conjugation)).Cast<Verb.Conjugation>()) {
				foreach (var pointOfView in ChoosePointOfViewsForDrill()) {

					var basePhrase = pointOfView.AsSubjectPronoun () + " " +
					                 verb.ConjugatedForTense (conjugation, pointOfView);

					var exercise = new Exercise () {
						ExtraInfo = verb.Infinitive,
					};

					exercise.Tags.Add ("drill:linguee");
					exercise.Tags.Add ("conjugation:" + conjugation);

					if (pointOfView.IsSecondPerson ())
						exercise.HintsForTranslated.Add (pointOfView.AsSubjectPronoun());
					
					var lingueeResults = DoLingueeLookup (webDriver, basePhrase);

					//  We'll use the shortest (hopefully the easiest) result.
					exercise.Original = lingueeResults.Keys.OrderBy (k => k.Length).FirstOrDefault();

					if (exercise.Original == null) {
						lingueeResults = DoLingueeLookup (webDriver, verb.ConjugatedForTense (conjugation, pointOfView));

						exercise.Original = lingueeResults.Keys.OrderBy (k => k.Length).FirstOrDefault();

						if (exercise.Original == null) {
							throw new Exception (String.Format ("Unable to find linguee phrases for {0} of {1}, '{2}'", conjugation, verb.Infinitive, basePhrase));
						}
					}

					exercise.Translated = lingueeResults [exercise.Original];

					results.Add (exercise);
				}
			}

			// since we sometimes regress to not including the object pronoun, its possible
			// we've done duplicated lookups.  Remove the dupes now.
			results = results.GroupBy (r => r.Original).Select (y => y.First ()).ToList ();

			return results;
		}

		public static Dictionary<string, string> DoLingueeLookup (ChromeDriver chromeDriver, string target)
		{
			Dictionary<string, string> results = new Dictionary<string, string> ();

			var lingueeLookup = "http://www.linguee.com/english-spanish/search?source=auto&query=\"" + System.Net.WebUtility.UrlEncode (target) + "\"";

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
	}
}

