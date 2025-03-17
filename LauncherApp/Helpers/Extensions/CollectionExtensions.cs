using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LauncherApp.Helpers.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Convert IEnumerable to ObservableCollection
        /// </summary>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> items)
        {
            return new ObservableCollection<T>(items);
        }

        /// <summary>
        /// Sort ObservableCollection in-place
        /// </summary>
        public static void Sort<T>(this ObservableCollection<T> collection, Comparison<T> comparison)
        {
            var sorted = collection.OrderBy(x => x, Comparer<T>.Create((x, y) => comparison(x, y))).ToList();
            
            for (int i = 0; i < sorted.Count; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(collection[i], sorted[i]))
                {
                    collection.Move(collection.IndexOf(sorted[i]), i);
                }
            }
        }

        /// <summary>
        /// Refresh ObservableCollection with new items
        /// </summary>
        public static void Refresh<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            collection.Clear();
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
    }
}
