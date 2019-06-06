using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Tool.PrebuildTasks
{
    [AttributeUsage(AttributeTargets.Method)]
    class PrebuildMethodAttribute : Attribute
    {
        public string ProjectName { get; set; }
    }
}
