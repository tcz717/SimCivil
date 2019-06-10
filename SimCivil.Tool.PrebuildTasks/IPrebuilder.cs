using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Tool.PrebuildTasks
{
    public interface IPrebuilder
    {
        string ProjectName { get; }

        void Build(string projPath);
    }
}
