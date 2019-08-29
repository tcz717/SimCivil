using System.Collections.Generic;

using SimCivil.Orleans.Interfaces.Skill;

namespace SimCivil.Orleans.Interfaces.Component.State {
    public class SkillContainerState
    {
        public Dictionary<ISkill, int> Skills { get; set; }
    }
}