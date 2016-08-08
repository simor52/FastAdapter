using Android.Views;
using Android.Support.V7.Widget;

namespace FastAdapter.Core.Listenrs
{
    public interface IOnLongClickListener<IItem>
    {
        /// <summary>
        /// Returns true if this listner was handled.
        /// </summary>
        /// <param name="v">The view clicked.</param>
        /// <param name="adapter">The adapter wich is responsible for this item.</param>
        /// <param name="item">The item that's been clicked.</param>
        /// <param name="position">The items's global position.</param>
        /// <returns></returns>
        bool OnLongClick(View v, IAdapter<IItem> adapter, IItem item, int position);
    }
}