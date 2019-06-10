using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SimCivil.Tool.PrebuildTasks.Prebuilders
{
    public class ConceptPrebuilder : IPrebuilder
    {
        public string ProjectName => "SimCivil.Concept";

        public void Build(string projPath)
        {
            foreach (string fileFullName in Directory.GetFiles(Path.Combine(projPath, "ItemModel", "Component", "State")).Where(s => s.EndsWith("State.cs")))
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
                    res.Add($"Task<{prop.Type}> Get{prop.Identifier}();");
                    res.Add($"Task Set{prop.Identifier}({prop.Type} value);");
                }

                string target = Path.Combine(projPath, "ItemModel", "Component", "I" + fileNameWithoutExtension.Substring(0, fileNameWithoutExtension.Length - "State".Length) + ".cs");
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
