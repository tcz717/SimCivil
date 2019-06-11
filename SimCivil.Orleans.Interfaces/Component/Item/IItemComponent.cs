namespace SimCivil.Orleans.Interfaces.Component
{
    /// <summary>
    /// Base interface for item component.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IItemComponent<T> : Orleans.Interfaces.IComponent<T> where T : new()
    {
        // Nothing by now.
    }
}
