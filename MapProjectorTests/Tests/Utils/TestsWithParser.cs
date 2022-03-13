using System;
using CommandLine;

namespace MapProjectorCLI.Tests
{
    public class TestsWithParser
    {
        const string defaultArgs = "-f ..\\..\\Tests\\Input\\earth_equirect.png -o out.png";
        protected string[] ToArgs(string rawArgs, bool withDefaults = true)
        {
            if(withDefaults)
            {
                rawArgs += $" {defaultArgs}";
            }

            return rawArgs.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        protected static void Parse(string[] args, Action<CLIParams> success)
        {
            CLIParams.Parse(args)
                .WithParsed(success)
                .WithNotParsed(errs =>
            {
                foreach (var err in errs)
                {
                    throw new Exception(err.ToString());
                }
            });
        }

    }
}
