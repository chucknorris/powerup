using System;
using System.Configuration;
using System.Linq;
using Powerup.Commandline.Options;
using Powerup.Output;

namespace Powerup
{
    class Program
    {
        static void Main(string[] args)
        {
            if (ConfigurationManager.ConnectionStrings.Count == 0) return;

            var conn = new Configuration();
            var optionSet = ParseCommandLineOptions(conn);
            
            try
            {
                var app = new Application(conn);

                optionSet.Parse(args);
                if(!conn.IsValid)
                {
                    throw new OptionException("missing parameters","");
                }

                app.BuildEntities();
                app.AddCodeToEntities();

                foreach (var writer in app
                    .TheObjects()
                    .Select(item => new FileWriter(item.ThingToTemplate, conn.OutputFolder)))
                {
                    writer.DoWrite();
                }
            }
            catch (OptionException optionException)
            {
                Console.WriteLine(optionException.Message);
                ShowTheHelp(optionSet);
            }
        }

        private static void ShowTheHelp(OptionSet optionSet)
        {
            optionSet.WriteOptionDescriptions(Console.Out);
        }

        private static OptionSet ParseCommandLineOptions(Configuration configuration)
        {
            var optionSet = new OptionSet()
                .Add("?|h|help", ShowHelp)
                .Add("d=|database=",
                     "Specify the name of a database to use.",
                     o => configuration.DatabaseName = o)
                .Add("s=|server=",
                    "Specify the name of a server to use.",
                    o => configuration.ServerName = o)
                 .Add("o=|output=",
                    "The location to write the generated files.",
                    o => configuration.OutputFolder = o);
            
            return optionSet;
        }

        private static void ShowHelp(string option)
        {
            if (option != null) throw new OptionException("Help", "");
        }
    }
}
