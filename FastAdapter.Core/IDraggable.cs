using Android.Support.V7.Widget;
using System.Collections.Generic;

namespace FastAdapter.Core
{
    public interface IDraggable<T, VH> where VH : RecyclerView.ViewHolder where T : class
    {
        /// <summary>
        /// Gets and sets whether if this item is draggable.
        /// </summary>
        bool Draggable { get; set; }
    }
}