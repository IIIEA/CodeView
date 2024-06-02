using System;
using System.Collections.Generic;

namespace _Fly_Connect.Scripts.Gameplay.CityScripts
{
    public class ObservableList<T> : List<T>
    {
        public event Action<int> CountChanged;

        public new void Add(T item)
        {
            base.Add(item);
            OnCountChanged();
        }

        public new void Remove(T item)
        {
            base.Remove(item);
            OnCountChanged();
        }

        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            OnCountChanged();
        }

        private void OnCountChanged()
        {
            CountChanged?.Invoke(Count);
        }
    }
}