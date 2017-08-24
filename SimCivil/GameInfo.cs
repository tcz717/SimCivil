using CommandLineParser.Arguments;
using CommandLineParser.Validation;

namespace SimCivil
{
    /// <summary>
    /// Basic infomation about a game.
    /// </summary>
    [ArgumentGroupCertification("n,c", EArgumentGroupCondition.AllOrNoneUsed)]
    [ArgumentRequiresOtherArgumentsCertification("s", "c")]
    public class GameInfo
    {
        /// <summary>
        /// If create new game.
        /// </summary>
        [SwitchArgument('c', "create", false, Description = "Create new game.")]
        public bool IsCreate;
        /// <summary>
        /// Game's name.
        /// </summary>
        [ValueArgument(typeof(string), 'n', Description = "New game's name.")]
        public string Name;
        /// <summary>
        /// Directory path to store all data.
        /// </summary>
        [ValueArgument(typeof(string), 'd', Optional = false, Description = "Directory to store all data.")]
        public string StoreDirectory;
        /// <summary>
        /// Magic Number about how to generate map.
        /// </summary>
        [ValueArgument(typeof(int), 's', Description = "Magic Number about how to generate map.")]
        public int Seed;
        /// <summary>
        /// Json configuration file about how to build game services. 
        /// </summary>
        [ValueArgument(typeof(string), 'j', Description = "Json configuration file about how to build game services.")]
        public string Config = null;
    }
}