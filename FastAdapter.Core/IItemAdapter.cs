using Android.Support.V7.Widget;
using System.Collections.Generic;

namespace FastAdapter.Core
{
    public interface IItemAdapter<IItem> : IAdapter<IItem>
    {
        /**
     * sets the subItems of the given collapsible
     *
     * @param collapsible the collapsible which gets the subItems set
     * @param subItems    the subItems for this collapsible item
     * @return the item type of the collapsible
     */
         void SetSubItems(IExpandable<IItem> collapsible, IEnumerable<IItem> subItems);

        /**
         * set a new list of items and apply it to the existing list (clear - add) for this adapter
         *
         * @param items
         */
        IItemAdapter<IItem> Set(IEnumerable<IItem> items);

        /**
         * sets a complete new list of items onto this adapter, using the new list. Calls notifyDataSetChanged
         *
         * @param items
         */
        IItemAdapter<IItem> SetNewList(IEnumerable<IItem> items);

        /**
         * add an array of items to the end of the existing items
         *
         * @param items
         */
        IItemAdapter<IItem> Add(IEnumerable<IItem> items);

        
        
        /**
         * add a list of items at the given position within the existing items
         *
         * @param position the global position
         * @param items
         */
        IItemAdapter<IItem> Add(int position, IEnumerable<IItem> items);

        /**
         * sets an item at the given position, overwriting the previous item
         *
         * @param position the global position
         * @param item
         */
        IItemAdapter<IItem> Set(int position, IItem item);

        /**
         * removes an item at the given position within the existing icons
         *
         * @param position the global position
         */
        IItemAdapter<IItem> Remove(int position);

        /**
         * removes a range of items starting with the given position within the existing icons
         *
         * @param position  the global position
         * @param itemCount
         */
        IItemAdapter<IItem> RemoveRange(int position, int itemCount);

        /**
         * removes all items of this adapter
         */
        IItemAdapter<IItem> Clear();
    }
}