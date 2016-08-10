using Android.Support.V7.Widget;
using System.Collections.Generic;

namespace FastAdapter.Core
{
    public interface IItemAdapter<T, VH> : IAdapter<T, VH> where VH : RecyclerView.ViewHolder where T : class
    {
        /**
     * sets the subItems of the given collapsible
     *
     * @param collapsible the collapsible which gets the subItems set
     * @param subItems    the subItems for this collapsible item
     * @return the item type of the collapsible
     */
         void SetSubItems(IExpandable<T, VH> collapsible, IEnumerable<IItem<T, VH>> subItems);

        /**
         * set a new list of items and apply it to the existing list (clear - add) for this adapter
         *
         * @param items
         */
        IItemAdapter<T, VH> Set(IEnumerable<IItem<T, VH>> items);

        /**
         * sets a complete new list of items onto this adapter, using the new list. Calls notifyDataSetChanged
         *
         * @param items
         */
        IItemAdapter<T, VH> SetNewList(IEnumerable<IItem<T, VH>> items);

        /**
         * add an array of items to the end of the existing items
         *
         * @param items
         */
        IItemAdapter<T, VH> Add(IEnumerable<IItem<T, VH>> items);

        
        
        /**
         * add a list of items at the given position within the existing items
         *
         * @param position the global position
         * @param items
         */
        IItemAdapter<T, VH> Add(int position, IEnumerable<IItem<T, VH>> items);

        /**
         * sets an item at the given position, overwriting the previous item
         *
         * @param position the global position
         * @param item
         */
        IItemAdapter<T, VH> Set(int position, IItem<T, VH> item);

        /**
         * removes an item at the given position within the existing icons
         *
         * @param position the global position
         */
        IItemAdapter<T, VH> Remove(int position);

        /**
         * removes a range of items starting with the given position within the existing icons
         *
         * @param position  the global position
         * @param itemCount
         */
        IItemAdapter<T, VH> RemoveRange(int position, int itemCount);

        /**
         * removes all items of this adapter
         */
        IItemAdapter<T, VH> Clear();
    }
}