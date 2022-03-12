using System;
using NUnit.Framework;
using CommandLine;
using MapProjectorLib;
using System.Reflection;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Linq;

namespace MapProjectorCLI.Tests
{
    //This test harnass generates an example file from the tests
    public class ExampleGeneratingTests : TestsWithParser
    {
        protected static SortedDictionary<string, Example> globalExamples = new SortedDictionary<string, Example>();

        protected const string repoPath = @"https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output";

        protected void AddExample(string[] args, string notes = "", [CallerMemberName] string callerName = "")
        {
            var callerClass = this.GetType().FullName;
            var titleIdx = callerClass.IndexOf("+");
            var key = $"{callerClass.Substring(titleIdx + 1)}.{callerName}";
            AddExample(key, callerName, args, notes);

        }

        protected void AddExample(string title, string[] args, string notes = "")
        {
            var callerClass = this.GetType().FullName;
            var titleIdx = callerClass.IndexOf("+");
            var key = $"{callerClass.Substring(titleIdx + 1)}.{title}";
            AddExample(key, title, args, notes);

        }
        protected void AddExample(string key, string title, string[] args, string notes = "")
        {

            var images = new List<string>();
            var loopCnt = LoopCount(args);

            if (loopCnt == 0)
            {
                images.Add($"{repoPath}/{title}.png");
            }
            else
            {
                for (int i = 0; i < loopCnt; i++)
                {
                    var sequenceTitle = string.Format("{0}{1,4:0000}", title, i);
                    images.Add($"{repoPath}/{sequenceTitle}.png");
                }
            }

            globalExamples.Add(
                key,
                new Example(title, args, images, notes)
           );
        }

        int LoopCount(string[] args)
        {
            var loopIdx = Array.FindIndex(args, arg => arg == "--loop");
            if (loopIdx == -1) return 0;
            return int.Parse(args[loopIdx + 1]);
        }

        [OneTimeTearDown]
        public void OutputExampleFile()
        {
            var assm = typeof(Program).GetTypeInfo().Assembly;
            var currentDir = Path.GetDirectoryName(assm.Location);
            string exampleFile = Path.Combine(currentDir, "..\\..\\..\\examples.md");

            File.WriteAllText(exampleFile, CompiledExamples());
            Console.WriteLine($"Examples written to {exampleFile}");
        }

        readonly Regex camelCaseMatcher = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

        string CompiledExamples()
        {
            var exampleHierarchy = SplitList(globalExamples);

            var e = new StringBuilder("# Examples");

            foreach (var iEx in exampleHierarchy)
            {
                e.Append(iEx.ToString(2));
            }

            return e.ToString();
        }

        //qualifiedTitleDict is alpha ordered by key
        List<IExample> SplitList(SortedDictionary<string, Example> qualifiedTitleDict)
        {
            string ToTitle(string key)
            {
                var titleIdx = key.LastIndexOf('.');
                var camelCaseTitle = key.Substring(titleIdx + 1);
                return Regex.Replace(camelCaseMatcher.Replace(camelCaseTitle, " "), @"\s+", " ");
            }

            var examples = new List<IExample>();
            var qualifiedTitlesList = qualifiedTitleDict.Keys.ToList();
            var listIdx = 0;

            while (listIdx < qualifiedTitlesList.Count)
            {
                var currentQualifiedTitle = qualifiedTitlesList[listIdx];
                var headingCutoffIdx = currentQualifiedTitle.IndexOf('+');

                //some splits are by ".", so check for that too
                if (headingCutoffIdx == -1)
                {
                    headingCutoffIdx = currentQualifiedTitle.IndexOf('.');
                }

                //top level
                if (headingCutoffIdx == -1)
                {
                    var singleExample = qualifiedTitleDict[currentQualifiedTitle];
                    singleExample.Title = ToTitle(singleExample.Title);
                    examples.Add(singleExample);
                    listIdx++;
                }
                else
                {
                    var currentHeading = currentQualifiedTitle.Substring(0, headingCutoffIdx);

                    var matchingList = qualifiedTitlesList
                            //this might create a bug if the currentHeading is a shorter version of a different heading.
                            //Checking complicates the logic and this scenario doesn't seem likely. Fix if it turns
                            //into a problem
                            .Where(title => title.StartsWith(currentHeading))
                            .ToList();

                    var subList = matchingList
                        .Select(title => title.Substring(currentHeading.Length + 1))
                        .ToList();

                    var subDict = new SortedDictionary<string, Example>();

                    for (int i = 0; i < matchingList.Count; i++)
                    {
                        subDict.Add(subList[i], qualifiedTitleDict[matchingList[i]]);
                    }

                    var subGroup = new ExampleGroup()
                    {
                        Title = ToTitle(currentHeading)
                    };

                    subGroup.Examples = SplitList(subDict);

                    examples.Add(subGroup);

                    listIdx += matchingList.Count;
                }


            }

            return examples;

        }
    }
}
