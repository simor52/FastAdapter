using Android.Support.V7.Widget;
using FastAdapter.Core.Listenrs;

namespace FastAdapter.Core
{
    public interface IClickable<T, VH> where VH : RecyclerView.ViewHolder where T : class
    {

        /// <summary>
        /// Sets the pre-click listner for this item.
        /// </summary>
        /// <param name="onItemPreClickListener">The pre-click listner to be added.</param>
        /// <returns>Returns this item</returns>
        IItem<T, VH> withOnItemPreClickListener(IOnClickListener<IItem<T, VH>> onItemPreClickListener);

        /// <summary>
        /// Gets the pre-click listner for this item.
        /// </summary>
        IOnClickListener<IItem<T, VH>> OnPreItemClickListener { get; }

        /// <summary>
        /// Sets the click listner for this item.
        /// </summary>
        /// <param name="onItemClickListener">The click listner to be added.</param>
        /// <returns>Returns this item</returns>
        IItem<T, VH> withOnItemClickListener(IOnClickListener<IItem<T, VH>> onItemClickListener);

        /// <summary>
        /// Gets the click listner for this item.
        /// </summary>
        IOnClickListener<IItem<T, VH>> OnItemClickListener { get; }
    }
}