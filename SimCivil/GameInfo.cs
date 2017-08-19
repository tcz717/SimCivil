namespace SimCivil
{
    /// <summary>
    /// Basic infomation about a game.
    /// </summary>
    public struct GameInfo
    {
        /// <summary>
        /// Game's name.
        /// </summary>
        public string Name;
        /// <summary>
        /// Directory path to store all data.
        /// </summary>
        public string StoreDirectory;
        /// <summary>
        /// Magic Number about how to generate map.
        /// </summary>
        public int Seed;
    }
}