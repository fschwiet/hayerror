using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManyConsole;

namespace monarquia.Commands
{
    public class ViewAllPriorityGroups : ConsoleCommand
    {
        public ViewAllPriorityGroups()
        {
            this.IsCommand("view-all-priority-groups", "List all tag priority groups.");
            this.SkipsCommandSummaryBeforeRunning();
        }

        public override int Run(string[] remainingArguments)
        {
            DataLoader dataLoader = new DataLoader("./data");
            var cannedData = new BetterCannedData(dataLoader);

            var exerciseGenerator = new EspanolGenerator(cannedData, "./data");
            var exercises = exerciseGenerator.GetExercises(null, false).Where(e => !e.Tags.Contains("vosotros"));

            List<string> usagePrefixes = new List<string>();
            Dictionary<string, List<string>> groupedByUsage = new Dictionary<string, List<string>>();

            foreach(var tag in exercises.SelectMany(e => e.Tags).Where(t => t.StartsWith("usage:")))
            {
                var prefix = tag.Substring(0, tag.IndexOf('-'));

                if (!usagePrefixes.Contains(prefix)) {
                    usagePrefixes.Add(prefix);
                    groupedByUsage[prefix] = new List<string>();
                }

                if (!groupedByUsage[prefix].Contains(tag))
                    groupedByUsage[prefix].Add(tag);
            }

            List<string> verbTags = new List<string>();
            foreach(var tag in exercises.SelectMany(e => e.Tags).Where(t => t.StartsWith("verb:")))
            {
                if (!verbTags.Contains(tag))
                    verbTags.Add(tag);
            }

            List<IEnumerable<string>> results = new List<IEnumerable<string>>();

            results.AddRange(cannedData.GetPriorityGroups());
            results.Add(verbTags);

            foreach(var prefix in usagePrefixes)
            {
                results.Add(groupedByUsage[prefix]);
            }

            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(results, Newtonsoft.Json.Formatting.Indented));

            return 0;
        }
    }

}
