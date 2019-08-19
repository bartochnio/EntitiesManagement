using System.Collections.Generic;

namespace Argh.Utilities.Control
{
    public interface IObjectSwitcher<T>
    {
        List<T> availableObjects { get; }

        T ActivateObject(string name);

        void AddAvailableObject(T obj);

        void DeactivateAll();
    }
}