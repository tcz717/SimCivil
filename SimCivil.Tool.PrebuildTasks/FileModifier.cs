using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SimCivil.Tool.PrebuildTasks
{
    /// <summary>
    /// The File Modifier to rewrite file if file needs update
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class FileModifier
    {
        private List<string> oldLines;
        private string path;

        public FileModifier(string path, bool createNew = true)
        {
            if (!File.Exists(path))
            {
                if (createNew)
                {
                    oldLines = new List<string>();
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }
            else
            {
                oldLines = File.ReadAllLines(path).ToList();
            }

            this.path = path;
        }

        public List<string> Read()
        {
            return new List<string>(oldLines);
        }

        public bool Write(IList<string> newLines)
        {
            if (newLines.Count == oldLines.Count)
            {
                bool changed = false;
                for (int i = 0; i < oldLines.Count; i++)
                {
                    if (oldLines[i] != newLines[i])
                    {
                        changed = true;
                        break;
                    }
                }

                if (!changed)
                {
                    return false;
                }
            }

            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllLines(path, newLines);
            return true;
        }
    }
}
