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
        public void Prebuild(string projPath, string projName)
        {
            var cls =
                GetType().Assembly.GetTypes().Where(type 
                    => type.GetCustomAttribute<PrebuildAttribute>()?.ProjectName == projName
                    && type.GetInterface(nameof(IPrebuilder)) != null)
                .FirstOrDefault();

            if (cls == null)
            {
                Console.WriteLine("No prebuild method found and executed.");
                return;
            }

            Console.WriteLine($"Running prebuild method for {projName}");
            var builder = (IPrebuilder)Activator.CreateInstance(cls);
            builder.Build(projPath, projName);
            Console.WriteLine($"Finished prebuild method for {projName}");
        }
    }
}
