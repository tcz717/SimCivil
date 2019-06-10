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
    public class BuilderDispatcher
    {
        public void Prebuild(string projPath)
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
    }
}
