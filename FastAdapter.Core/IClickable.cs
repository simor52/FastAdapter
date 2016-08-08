using Android.Support.V7.Widget;
using FastAdapter.Core.Listenrs;

namespace FastAdapter.Core
{
    public interface IClickable<IItem>
    {

        /// <summary>
        /// Sets the pre-click listner for this item.
        /// </summary>
        /// <param name="onItemPreClickListener">The pre-click listner to be added.</param>
        /// <returns>Returns this item</returns>
        IItem withOnItemPreClickListener(IOnClickListener<IItem> onItemPreClickListener);

        /// <summary>
        /// Gets the pre-click listner for this item.
        /// </summary>
        IOnClickListener<IItem> OnPreItemClickListener { get; }

        /// <summary>
        /// Sets the click listner for this item.
        /// </summary>
        /// <param name="onItemClickListener">The click listner to be added.</param>
        /// <returns>Returns this item</returns>
        IItem withOnItemClickListener(IOnClickListener<IItem> onItemClickListener);

        /// <summary>
        /// Gets the click listner for this item.
        /// </summary>
        IOnClickListener<IItem> OnItemClickListener { get; }
    }
}