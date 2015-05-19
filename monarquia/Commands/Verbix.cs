using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ManyConsole;
using Newtonsoft.Json.Linq;

namespace monarquia
{
	public class PropertyBag {

		JObject root;

		public PropertyBag(JObject root) {
			this.root = root;
		}

		public string GetValue(params string[] propertyNames)
		{
			JToken current = root;

			foreach (var property in propertyNames) {
				if (!(current is JObject)) {
					throw new Exception ("Could not load property " + property + " of non-JObject: " + string.Join (",", propertyNames));
				}

				if (!(current as JObject).TryGetValue (property, out current)) {
					throw new Exception ("Could not find property " + property + " querying for: " + string.Join (",", propertyNames));
				}
			}

			if (!(current is JValue))
				throw new Exception ("Property requested is not a JValue: " + string.Join(",",propertyNames));

			if ((current as JValue).Type != JTokenType.String)
				throw new Exception ("Property requested is not a string JValue: " + string.Join(",",propertyNames));

			return current.ToString ();
		}
	}

	public class Verbix : ConsoleCommand
	{
		public string Verb;

		public Verbix ()
		{
			this.IsCommand ("verbix", "Dumps stored conjugation information for a verb.");
			this.HasAdditionalArguments (1, " <verb>");
		}

		public override int? OverrideAfterHandlingArgumentsBeforeRun (string[] remainingArguments)
		{
			Verb = remainingArguments [0];

			return base.OverrideAfterHandlingArgumentsBeforeRun (remainingArguments);
		}

		public override int Run (string[] remainingArguments)
		{
			JObject result = new JObject();
			JObject topLoopPosition = result;

			var filepath = "./data/english-verbs/" + Verb + ".verbix.txt";

			if (!File.Exists (filepath))
				throw new ConsoleHelpAsException ("Could not find Verbix data at " + filepath);

			CsQuery.CQ document = File.ReadAllText (filepath);

			var headers = document [".column > h3, .column + h2"];

			foreach (var header in headers) {
				if (header.NodeName == "H2") {
					var nested = new JObject ();
					topLoopPosition.Add (header.Cq().Text(), nested);
					topLoopPosition = nested;
					continue;
				}

				var node = new JObject ();
				var containerNode = node;
				topLoopPosition.Add (header.Cq().Text(), node);

				var children = header.ParentNode.Cq ().Find ("td > p > *");
				string value1 = null;
				string value2 = null;

				foreach (var child in children) {
					
					if (child.NodeName == "BR") {
						if (value2 != null) {
							JToken temp;
							if (!node.TryGetValue (value1, out temp)) {
								containerNode.Add (value1, new JValue (value2));
							}
						}
						if (value1 != null) {
						
							JToken temp;
							if (!node.TryGetValue (value1, out temp)) {
								var subcontainer = new JObject ();
								node.Add (value1, subcontainer);
								containerNode = subcontainer;
							}
						}

						value1 = null;
						value2 = null;
					} else {
					
						if (value1 == null) {
							value1 = child.Cq().Text().Trim();
						} else if (value2 == null) {
							value2 = child.Cq ().Text ().Trim();
						}
					}
				}
			}

			Console.WriteLine (result.ToString ());

			var readable = new PropertyBag (result);
			Console.WriteLine (readable.GetValue ("Indicative", "Present", "I"));
			//((readable.GetValue("Indicative") as JObject).GetValue("Present") as JObject).GetValue("I")

			//Console.WriteLine (((readable.GetValue("Indicative") as JObject).GetValue("Present") as JObject).GetValue("I"));

			return 0;
		}
	}
}

