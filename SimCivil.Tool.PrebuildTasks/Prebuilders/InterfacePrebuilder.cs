using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SimCivil.Tool.PrebuildTasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static SimCivil.Tool.PrebuildTasks.FileUtils;

namespace SimCivil.Tool.PrebuildTasks.Prebuilders
{
    public class InterfacePrebuilder : IPrebuilder
    {
        public string ProjectName => "SimCivil.Orleans.Interfaces";

        public void Build(string projPath)
        {
            foreach (string fileFullName in Directory.GetFiles(Path.Combine(projPath, "Component", "Item", "State")).Where(s => s.EndsWith("State.cs")))
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileFullName);

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
                    res.Add($"Task<{prop.Type}> Get{prop.Identifier}();");
                    res.Add(string.Empty);
                    res.Add($"Task Set{prop.Identifier}({prop.Type} value);");
                }
                res.Add(string.Empty);

                string target = Path.Combine(projPath, "Component", "Item", "I" + fileNameWithoutExtension.Substring(0, fileNameWithoutExtension.Length - "State".Length) + ".cs");
                var targetFile = new FileModifier(target);
                List<string> lines = targetFile.Read();

                try
                {
                    TryInsertLinesToRegion(lines, res, "StateProperty");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"In interface of {fileNameWithoutExtension}, {e.Message}");
                }

                if (targetFile.Write(lines))
                {
                    Console.WriteLine($"Updated interface of {fileNameWithoutExtension}.");
                }
                else 
                {
                    Console.WriteLine($"Skipped update interface of {fileNameWithoutExtension}, file has no change.");
                }
            }
        }
    }
}
