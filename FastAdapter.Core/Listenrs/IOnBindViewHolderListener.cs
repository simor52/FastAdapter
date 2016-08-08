using Android.Support.V7.Widget;

namespace FastAdapter.Core.Listenrs
{
    public interface IOnBindViewHolderListener
    {
        /**
         * is called in onBindViewHolder to bind the data on the ViewHolder
         *
         * @param viewHolder the viewHolder for the type at this position
         * @param position   the position of thsi viewHolder
         */
        void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position);
    }
}