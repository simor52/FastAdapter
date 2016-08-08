using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;

namespace FastAdapter.Core
{
    public interface IItem<T, VH> : IIdentifyable<T> where T : class
        where VH : RecyclerView.ViewHolder  
    {
        /// <summary>
        /// Sets the tag of this item and returns it.
        /// </summary>
        /// <param name="tag">The tag to be added.</param>
        /// <returns></returns>
        T WithTag(object tag);

        /// <summary>
        /// Gets the tag of this item.
        /// </summary>
        object Tag { get; }

        /// <summary>
        /// Sets whether if this item is enabled.
        /// </summary>
        /// <param name="isEnabled">Boolean to be added.</param>
        /// <returns></returns>
        T WithEnabled(bool isEnabled);

        /// <summary>
        /// Returns true if this item is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Sets whether if this item is selected.
        /// </summary>
        /// <param name="isSelected">The tag to be added.</param>
        /// <returns></returns>
        T WithSetSelected(bool isSelected);

        /// <summary>
        /// Returns true if this item is selected.
        /// </summary>
        bool IsSelected { get; }

        /// <summary>
        /// Gets the type of this item.
        /// </summary>
        int Type { get; }

        /// <summary>
        /// Gets the layout resource id of this item.
        /// </summary>
        int LayoutResource { get; }

        /// <summary>
        /// Generates a view base on the defined layout resource.
        /// </summary>
        /// <param name="ctx">The context to be added.</param>
        /// <returns></returns>
        View GenerateView(Context ctx);

        /// <summary>
        /// Generates a view base on the defined layout resource and passes the LayoutPramas from parent.
        /// </summary>
        /// <param name="ctx">The context to be added.</param>
        /// <param name="parent">The parent view to be added.</param>
        /// <returns></returns>
        View GenerateView(Context ctx, ViewGroup parent);

        /// <summary>
        /// Generates a view holder for this item with the given parent.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        VH GetViewHolder(ViewGroup parent);

        /// <summary>
        /// Binds the data of this item to the given view holder.
        /// </summary>
        /// <param name="viewHolder"></param>
        void BindViewHolder(VH viewHolder);

        /// <summary>
        /// Checks if this item equals to the item with given identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Equals(int id);
    }
}