using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SimCivil.Tool.PrebuildTasks
{
    public class BuilderDispatcher
    {
        public void Prebuild(string solutionPath, string projName)
        {
            var projName = Path.GetFileName(Path.GetDirectoryName(projPath));

            var builders =
                GetType().Assembly.GetTypes().Where(type => type.GetInterface(nameof(IPrebuilder)) != null);

            var builder = builders
                .Select(type => (IPrebuilder)Activator.CreateInstance(type))
                .Where(type => type.ProjectName == projName)
                .FirstOrDefault();

            if (builder == null)
            {
                Console.WriteLine("No prebuild method found and executed.");
                return;
            }

            Console.WriteLine($"Running prebuild method for {projName}.");
            builder.Build(projPath);
            Console.WriteLine($"Finished prebuild method for {projName}.");
        }

        private static IDictionary<string, string> GetProjects(string solutionPath)
        {
            string sol = File.ReadAllText(Path.Combine(solutionPath, "SimCivil.sln"));
            var match = Regex.Match(sol, "Project\\([\\w{}\" -]+\\) = \"([\\w.]+)\", \"([\\w.\\\\]+)\"");
        }
    }
}
