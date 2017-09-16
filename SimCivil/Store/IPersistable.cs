using System.Threading.Tasks;

namespace SimCivil.Store
{
    /// <summary>
    /// Object can be stored in disk or somewhere else when the game requires.
    /// </summary>
    public interface IPersistable
    {
        /// <summary>
        /// Initialize the store.
        /// </summary>
        /// <param name="info"></param>
        void Initialize(GameInfo info);

        /// <summary>
        /// Load the object.
        /// </summary>
        /// <param name="path">Directory path to store all data.</param>
        void Load(string path);

        /// <summary>
        /// Load the object async.
        /// </summary>
        /// <param name="path">Directory path to store all data.</param>
        Task LoadAsync(string path);

        /// <summary>
        /// Save the object.
        /// </summary>
        /// <param name="path">Directory path to store all data.</param>
        void Save(string path);

        /// <summary>
        /// Save the object async.
        /// </summary>
        /// <param name="path">Directory path to store all data.</param>
        Task SaveAsync(string path);
    }
}