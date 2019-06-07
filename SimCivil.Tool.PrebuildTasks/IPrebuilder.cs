using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Tool.PrebuildTasks
{
    public interface IPrebuilder
    {
        void Build(string projPath, string projName);
    }
}
