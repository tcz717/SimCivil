using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Tool.PrebuildTasks
{
    [AttributeUsage(AttributeTargets.Class)]
    class PrebuildAttribute : Attribute
    {
        public string ProjectName { get; set; }
    }
}
