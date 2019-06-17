using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using static SimCivil.Tool.PrebuildTasks.FileUtils;

namespace SimCivil.Tool.PrebuildTasks.Prebuilders
{
    public class GrainPrebuilder : IPrebuilder
    {
        public string ProjectName => "SimCivil.Orleans.Grains";

        public void Build(string projPath)
        {
            foreach (string fileFullName in Directory.GetFiles(Path.Combine(BuilderDispatcher.ProjectDirs["SimCivil.Orleans.Interfaces"], "Component", "Item", "State")).Where(s => s.EndsWith("State.cs")))
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileFullName);
                Console.WriteLine($"Updating {fileNameWithoutExtension}.");

                string file = File.ReadAllText(fileFullName);
                var tree = CSharpSyntaxTree.ParseText(file);
                var root = tree.GetRoot() as CompilationUnitSyntax;

                var cls = root.Members.OfType<NamespaceDeclarationSyntax>().First()
                    .Members.OfType<ClassDeclarationSyntax>().First();
                var props = cls.Members.OfType<PropertyDeclarationSyntax>();

                List<string> res = new List<string>();
                foreach (var prop in props)
                {
                    res.Add(string.Empty);
                    res.Add($"public Task<{prop.Type}> Get{prop.Identifier}()");
                    res.Add("{");
                    res.Add($"    return Task.FromResult(State.{prop.Identifier});");
                    res.Add("}");
                    res.Add(string.Empty);
                    res.Add($"public Task Set{prop.Identifier}({prop.Type} value)");
                    res.Add("{");
                    res.Add($"    State.{prop.Identifier} = value;");
                    res.Add($"    return WriteStateAsync();");
                    res.Add("}");
                }
                res.Add(string.Empty);

                string target = Path.Combine(projPath, "Component", "Item", fileNameWithoutExtension.Substring(0, fileNameWithoutExtension.Length - "State".Length) + "Grain.cs");
                if (!File.Exists(target))
                {
                    Console.WriteLine("No Grain file found, skipped.");
                    continue;
                }

                List<string> lines = File.ReadAllLines(target).ToList();

                if (!TryInsertLinesToRegion(lines, res, "StateProperty"))
                {
                    continue;
                }

                File.WriteAllLines(target, lines);
            }
        }
    }
}
