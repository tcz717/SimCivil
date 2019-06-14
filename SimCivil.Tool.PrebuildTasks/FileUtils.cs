using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SimCivil.Tool.PrebuildTasks
{
    public static class FileUtils
    {
        public static bool TryInsertLinesToRegion(List<string> fileLines, List<string> lines, string regionName)
        {
            int start = 0;
            foreach (var line in fileLines)
            {
                if (line.Contains($"#region {regionName}"))
                {
                    break;
                }
                start++;
            }

            if (start == fileLines.Count)
            {
                Console.WriteLine($"No region to modify.");
                return false;
            }

            int end;
            for (end = start; end < fileLines.Count; end++)
            {
                if (fileLines[end].Contains("#endregion"))
                {
                    break;
                }
            }

            if (end == fileLines.Count)
            {
                Console.WriteLine($"No region end for.");
                return false;
            }

            fileLines.RemoveRange(start + 1, end - start - 1);

            var indent = fileLines[start].Length - fileLines[start].TrimStart().Length;

            var output = new List<string>();
            for (int i = 0; i < lines.Count; i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                {
                    output.Add(string.Empty);
                }
                else
                {
                    output.Add(new string(' ', indent) + lines[i]);
                }
            }

            fileLines.InsertRange(start + 1, output);
            return true;
        }
    }
}
