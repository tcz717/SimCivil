using Microsoft.CodeAnalysis.CSharp.Syntax;
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
        public static IReadOnlyDictionary<string, string> ProjectDirs { get; private set; }

        public void Prebuild(string solutionPath, string projName)
        {
            var projDirs = GetProjects(solutionPath);
            if (!projDirs.TryGetValue(projName, out string projPath))
            {
                throw new Exception($"No project found in solution - {projName}");
            }

            var builders =
                GetType().Assembly.GetTypes().Where(type => type.GetInterface(nameof(IPrebuilder)) != null);

            var builder = builders
                .Select(type => (IPrebuilder)Activator.CreateInstance(type))
                .Where(type => type.ProjectName == projName)
                .FirstOrDefault();

            if (builder == null)
            {
                throw new Exception($"No project found in prebuilders - {projName}");
            }

            Console.WriteLine($"Running prebuild method for {projName}.");
            builder.Build(projPath);
            Console.WriteLine($"Finished prebuild method for {projName}.");
        }

        private static IDictionary<string, string> GetProjects(string solutionPath)
        {
            string sol = File.ReadAllText(Path.Combine(solutionPath));
            var matches = Regex.Matches(sol, @"Project\([\w{}"" -]+\) = ""([\w.]+)"", ""([\w.\\]+)""");
            var projDirs = new Dictionary<string, string>();
            foreach (Match match in matches)
            {
                var proj = match.Groups[2].Value;
                if (!proj.EndsWith("csproj"))
                {
                    continue;
                }
                var dir = Path.Combine(Path.GetDirectoryName(solutionPath), Path.GetDirectoryName(proj));
                projDirs[match.Groups[1].Value] = dir;
            }

            ProjectDirs = projDirs;
            return projDirs;
        }
    }
}
