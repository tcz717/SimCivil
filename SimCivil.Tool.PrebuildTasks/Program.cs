using System;
using System.Diagnostics;
using System.IO;

namespace SimCivil.Tool.PrebuildTasks
{
    class Program
    {
        static void Main(string[] args)
        {
            string projName = Path.GetFileName(Path.GetDirectoryName(args[0]));
            Console.WriteLine("Project Name: " + projName);

            new Prebuilder().Prebuild(args[0], projName);
        }
    }
}
