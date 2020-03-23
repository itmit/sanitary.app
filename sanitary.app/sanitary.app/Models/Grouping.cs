using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace sanitary.app.Models
{
    public class Grouping<K, T> : ObservableCollection<T>
    {
        public K GroupKey { get; private set; }

        public Grouping(K key, IEnumerable<T> items)
        {
            GroupKey = key;
            foreach (var item in items)
                this.Items.Add(item);
        }
    }
}
