using System;
using System.IO;

namespace ConceptPrebuild
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string text = File.ReadAllText(args[0]);
                File.WriteAllText(Environment.ExpandEnvironmentVariables(@"%userprofile%\Desktop\out.txt"), text);
            }
            catch (Exception ex)
            {
                File.WriteAllText(Environment.ExpandEnvironmentVariables(@"%userprofile%\Desktop\out.txt"), ex.ToString());
            }
        }
    }
}
