using System.Collections.Generic;

namespace Project.Tools.ComponentRegistry
{
    public interface IComponentRegistry<T>
    {
        IReadOnlyList<T> Items { get; }

        void Add(T item);
        void Remove(T item);
        void Clear();
    }
}
