using System;
using System.Linq;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace SimCivil.Tool.PrebuildTasks
{
    public partial class Prebuilder
    {
        public string ProjectPath { get; set; }

        public void Prebuild(string projPath, string projName)
        {
            ProjectPath = projPath;

            var method =
                GetType()
                .GetMethods()
                .Where(m => m.GetCustomAttribute<PrebuildMethodAttribute>()?.ProjectName == projName)
                .FirstOrDefault();

            if (method == null)
            {
                Console.WriteLine("No prebuild method found and executed.");
                return;
            }

            Console.WriteLine($"Running prebuild method for {projName}");
            method.Invoke(this, new object[0]);
            Console.WriteLine($"Finished prebuild method for {projName}");
        }

        [PrebuildMethod(ProjectName = "SimCivil.Concept")]
        public void ConceptPrebuild()
        {
            foreach (string fileFullName in Directory.GetFiles(Path.Combine(ProjectPath, "ItemModel", "Component", "State")).Where(s => s.EndsWith("State.cs")))
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
                    res.Add($"Task<Result<{prop.Type}>> Get{prop.Identifier}();");
                    res.Add($"Task<Result> Set{prop.Identifier}({prop.Type} value);");
                }

                string target = Path.Combine(ProjectPath, "ItemModel", "Component", "I" + fileNameWithoutExtension.Substring(0, fileNameWithoutExtension.Length - "State".Length) + ".cs");
                List<string> lines = File.ReadAllLines(target).ToList();

                int start = 0;
                foreach (var line in lines)
                {
                    if (line.Contains("#region StateProperty"))
                    {
                        break;
                    }
                    start++;
                }

                if (start == lines.Count)
                {
                    Console.WriteLine($"No region to modify for Component of {fileNameWithoutExtension}.");
                    continue;
                }

                int end;
                for (end = start; end < lines.Count; end++)
                {
                    if (lines[end].Contains("#endregion"))
                    {
                        break;
                    }
                }

                if (end == lines.Count)
                {
                    Console.WriteLine($"No region end for Component of {fileNameWithoutExtension}.");
                    continue;
                }

                lines.RemoveRange(start + 1, end - start - 1);

                var indent = lines[start].Length - lines[start].TrimStart().Length;

                var output = new List<string>();
                for (int i = 0; i < res.Count; i++)
                {
                    output.Add(string.Empty);
                    output.Add(new string(' ', indent) + res[i]);
                }
                output.Add(string.Empty);

                lines.InsertRange(start + 1, output);

                File.WriteAllLines(target, lines);
            }
        }
    }
}
