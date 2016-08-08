using Android.Views;
using Android.Support.V7.Widget;

namespace FastAdapter.Core.Listenrs
{
    public class OnCreateViewHolderListener : IOnCreateViewHolderListener
    {
        /**
         * is called inside the onCreateViewHolder method and creates the viewHolder based on the provided viewTyp
         *
         * @param parent   the parent which will host the View
         * @param viewType the type of the ViewHolder we want to create
         * @return the generated ViewHolder based on the given viewType
         */
        RecyclerView.ViewHolder OnPreCreateViewHolder(ViewGroup parent, int viewType)
        {

        }

        /**
         * is called after the viewHolder was created and the default listeners were added
         *
         * @param viewHolder the created viewHolder after all listeners were set
         * @return the viewHolder given as param
         */
        RecyclerView.ViewHolder OnPostCreateViewHolder(RecyclerView.ViewHolder viewHolder)
        {

        }
    }
}