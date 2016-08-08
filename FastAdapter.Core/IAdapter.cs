using Android.Support.V7.Widget;
using System.Collections.Generic;

namespace FastAdapter.Core
{
    public interface IAdapter<Item> where Item : IItem
    {
        /// <summary>
        /// Gets the FastAdapter specified for this IAdapter
        /// </summary>
        FastAdapter<IItem> FastAdapter { get; }
        
        /// <summary>
        /// Gets the order of this adapter in witch should be hooked into the FastAdapter.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Gets the count of items of this adapter.
        /// </summary>
        int AdapterItemCount { get; }

        /// <summary>
        /// Gets the list of defined items within this adapter.
        /// </summary>
        List<IItem> AdapterItems { get; }

        /// <summary>
        /// return the item at the given relative position within this adapter.
        /// </summary>
        /// <param name="position">The relative position.</param>
        /// <returns>Item at the given relative position.</returns>
        IItem GetAdapterItem(int position);
        
        /// <summary>
        /// Searches for the given item and calculates it's relative position.
        /// </summary>
        /// <param name="item">The item which is searched for.</param>
        /// <returns>The relative position</returns>
        int GetAdapterPosition(IItem item);
        
        /// <summary>
        /// Gets the global position based on the given relative position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>The global position used for all methods.</returns>
        int GetGlobalPosition(int position);

        /// <summary>
        /// Gets the global item count.
        /// </summary>
        int ItemCount { get; }
        
        /// <summary>
        /// Returns the global item based on the global position
        /// </summary>
        /// <param name="index">The global position.</param>
        /// <returns>The global item based on the global position</returns>
        IItem this[int index]
        {
            get;
        }

    }
}