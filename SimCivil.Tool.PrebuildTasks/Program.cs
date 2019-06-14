using System;
using System.Diagnostics;
using System.IO;

namespace SimCivil.Tool.PrebuildTasks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Solution Path: " + args[0]);
            Console.WriteLine("Project Name: " + args[1]);

            new BuilderDispatcher().Prebuild(args[0], args[1]);
        }
    }
}
