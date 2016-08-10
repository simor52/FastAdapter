using Android.Support.V7.Widget;
using System.Collections.Generic;

namespace FastAdapter.Core
{
    public interface IExpandable<T, VH> where VH : RecyclerView.ViewHolder where T : class
    {

        /// <summary>
        /// Gets and sets whether if this item is expanded.
        /// </summary>
        bool Expanded { get; set; }


        /// <summary>
        /// Gets and sets list of sub items of this item.
        /// </summary>
        List<IItem<T, VH>> SubItems { get; set; }

        /// <summary>
        /// Returns true if this item is auto expanded.
        /// </summary>
        bool IsAutoExpanded { get; }
    }
}