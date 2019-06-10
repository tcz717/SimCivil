using System;
using System.Diagnostics;
using System.IO;

namespace SimCivil.Tool.PrebuildTasks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Project Path: " + args[0]);

            new BuilderDispatcher().Prebuild(args[0]);
        }
    }
}
