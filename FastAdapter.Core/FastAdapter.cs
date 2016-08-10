using Android.Support.V7.Widget;
using Android.Views;
using System.Collections.Generic;
using Java.Lang;
using Android.Support.V4.Util;
using Android.Util;
using FastAdapter.Core.Listenrs;
using Java.Util;
using Android.OS;
using FastAdapter.Core;

namespace FastAdapter.Core
{
    public abstract class FastAdapter<T, VH> : RecyclerView.Adapter where VH : RecyclerView.ViewHolder where T : class
    {
        protected const string BUNDLE_SELECTIONS = "bundle_selections";
        protected const string BUNDLE_EXPANDED = "bundle_expanded";
        // we remember all adapters
        //priority queue...
        private Dictionary<int, IAdapter<T, VH>> mAdapters = new Dictionary<int, IAdapter<T, VH>>();
        // we remember all possible types so we can create a new view efficiently
        private Dictionary<int, IItem<T, VH>> mTypeInstances = new Dictionary<int, IItem<T, VH>>();
        // cache the sizes of the different adapters so we can access the items more performant 

        private SortedDictionary<int, IAdapter<T, VH>> mAdapterSizes = new SortedDictionary<int, IAdapter<T, VH>>();
        // the total size
        private int mGlobalSize = 0;

        // if enabled we will select the item via a notifyItemChanged -> will animate with the Animator
        // you can also use this if you have any custom logic for selections, and do not depend on the "selected" state of the view
        // note if enabled it will feel a bit slower because it will animate the selection
        private bool mSelectWithItemUpdate = false;
        // if we want multiSelect enabled
        private bool mMultiSelect = false;
        // if we want the multiSelect only on longClick
        private bool mSelectOnLongClick = false;
        // if a user can deselect a selection via click. required if there is always one selected item!
        private bool mAllowDeselection = true;
        // if items are selectable in general
        private bool mSelectable = false;
        // only one expanded section
        private bool mOnlyOneExpandedItem = false;
        // if we use the positionBasedStateManagement or the "stateless" management
        private bool mPositionBasedStateManagement = true;

        // we need to remember all selections to recreate them after orientation change
        private SortedSet<int> mSelections = new SortedSet<int>();
        // we need to remember all expanded items to recreate them after orientation change
        private SparseIntArray mExpanded = new SparseIntArray();

        // the listeners which can be hooked on an item
        private IOnClickListener<IItem<T, VH>> mOnPreClickListener;
        private IOnClickListener<IItem<T, VH>> mOnClickListener;
        private IOnLongClickListener<IItem<T, VH>> mOnPreLongClickListener;
        private IOnLongClickListener<IItem<T, VH>> mOnLongClickListener;
        private IOnTouchListener<IItem<T, VH>> mOnTouchListener;

        //the listeners for onCreateViewHolder or onBindViewHolder
        private IOnCreateViewHolderListener mOnCreateViewHolderListener;// = new OnCreateViewHolderListenerImpl();
        private IOnBindViewHolderListener mOnBindViewHolderListener;// = new OnBindViewHolderListenerImpl();

        public FastAdapter()
        {
            HasStableIds = true;
        }

        /**
         * Define the OnClickListener which will be used for a single item
         *
         * @param onClickListener the OnClickListener which will be used for a single item
         * @return this
         */
        public FastAdapter<T, VH> WithOnClickListener(IOnClickListener<IItem<T, VH>> onClickListener)
        {
            this.mOnClickListener = onClickListener;
            return this;
        }

        /**
         * Define the OnPreClickListener which will be used for a single item and is called after all internal methods are done
         *
         * @param onPreClickListener the OnPreClickListener which will be called after a single item was clicked and all internal methods are done
         * @return this
         */
        public FastAdapter<T, VH> WithOnPreClickListener(IOnClickListener<IItem<T, VH>> onPreClickListener)
        {
            this.mOnPreClickListener = onPreClickListener;
            return this;
        }

        /**
         * Define the OnLongClickListener which will be used for a single item
         *
         * @param onLongClickListener the OnLongClickListener which will be used for a single item
         * @return this
         */
        public FastAdapter<T, VH> WithOnLongClickListener(IOnLongClickListener<IItem<T, VH>> onLongClickListener)
        {
            this.mOnLongClickListener = onLongClickListener;
            return this;
        }

        /**
         * Define the OnLongClickListener which will be used for a single item and is called after all internal methods are done
         *
         * @param onPreLongClickListener the OnLongClickListener which will be called after a single item was clicked and all internal methods are done
         * @return this
         */
        public FastAdapter<T, VH> WithOnPreLongClickListener(IOnLongClickListener<IItem<T, VH>> onPreLongClickListener)
        {
            this.mOnPreLongClickListener = onPreLongClickListener;
            return this;
        }

        /**
         * Define the TouchListener which will be used for a single item
         *
         * @param onTouchListener the TouchListener which will be used for a single item
         * @return this
         */
        public FastAdapter<T, VH> WithOnTouchListener(IOnTouchListener<IItem<T, VH>> onTouchListener)
        {
            this.mOnTouchListener = onTouchListener;
            return this;
        }

        /**
         * allows you to set a custom OnCreateViewHolderListener which will be used before and after the ViewHolder is created
         * You may check the OnCreateViewHolderListenerImpl for the default behavior
         *
         * @param onCreateViewHolderListener the OnCreateViewHolderListener (you may use the OnCreateViewHolderListenerImpl)
         */
        public FastAdapter<T, VH> WithOnCreateViewHolderListener(IOnCreateViewHolderListener onCreateViewHolderListener)
        {
            this.mOnCreateViewHolderListener = onCreateViewHolderListener;
            return this;
        }

        /**
         * allows you to set an custom OnBindViewHolderListener which is used to bind the view. This will overwrite the libraries behavior.
         * You may check the OnBindViewHolderListenerImpl for the default behavior
         *
         * @param onBindViewHolderListener the OnBindViewHolderListener
         */
        public FastAdapter<T, VH> WithOnBindViewHolderListener(IOnBindViewHolderListener onBindViewHolderListener)
        {
            this.mOnBindViewHolderListener = onBindViewHolderListener;
            return this;
        }


        /**
     * select between the different selection behaviors.
     * there are now 2 different variants of selection. you can toggle this via `withSelectWithItemUpdate(boolean)` (where false == default - variant 1)
     * 1.) direct selection via the view "selected" state, we also make sure we do not animate here so no notifyItemChanged is called if we repeatly press the same item
     * 2.) we select the items via a notifyItemChanged. this will allow custom selected logics within your views (isSelected() - do something...) and it will also animate the change via the provided itemAnimator. because of the animation of the itemAnimator the selection will have a small delay (time of animating)
     *
     * @param selectWithItemUpdate true if notifyItemChanged should be called upon select
     * @return this
     */
        public FastAdapter<T, VH> WithSelectWithItemUpdate(bool selectWithItemUpdate)
        {
            this.mSelectWithItemUpdate = selectWithItemUpdate;
            return this;
        }

        /**
         * Enable this if you want multiSelection possible in the list
         *
         * @param multiSelect true to enable multiSelect
         * @return this
         */
        public FastAdapter<T, VH> WithMultiSelect(bool multiSelect)
        {
            mMultiSelect = multiSelect;
            return this;
        }

        /**
         * Disable this if you want the selection on a single tap
         *
         * @param selectOnLongClick false to do select via single click
         * @return this
         */
        public FastAdapter<T, VH> WithSelectOnLongClick(bool selectOnLongClick)
        {
            mSelectOnLongClick = selectOnLongClick;
            return this;
        }

        /**
         * If false, a user can't deselect an item via click (you can still do this programmatically)
         *
         * @param allowDeselection true if a user can deselect an already selected item via click
         * @return this
         */
        public FastAdapter<T, VH> WithAllowDeselection(bool allowDeselection)
        {
            this.mAllowDeselection = allowDeselection;
            return this;
        }

        /**
         * set if no item is selectable
         *
         * @param selectable true if items are selectable
         * @return this
         */
        public FastAdapter<T, VH> WithSelectable(bool selectable)
        {
            this.mSelectable = selectable;
            return this;
        }

        /**
         * set if we want to use the positionBasedStateManagement (high performant for lists up to Integer.MAX_INT)
         * set to false if you want to use the new stateManagement which will come with more flexibility (but worse performance on long lists)
         *
         * @param mPositionBasedStateManagement false to enable the alternative "stateLess" stateManagement
         * @return this
         */
        public FastAdapter<T, VH> WithPositionBasedStateManagement(bool mPositionBasedStateManagement)
        {
            this.mPositionBasedStateManagement = mPositionBasedStateManagement;
            return this;
        }

        /**
         * @return if items are selectable
         */
        public bool IsSelectable
        {
            get { return mSelectable; }
        }

        /**
         * @return if this FastAdapter is configured with the PositionBasedStateManagement
         */
        public bool IsPositionBasedStateManagement
        {
            get { return mPositionBasedStateManagement; }
        }

        /**
         * set if there should only be one opened expandable item
         * DEFAULT: false
         *
         * @param mOnlyOneExpandedItem true if there should be only one expanded, expandable item in the list
         * @return this
         */
        public FastAdapter<T, VH> WithOnlyOneExpandedItem(bool mOnlyOneExpandedItem)
        {
            this.mOnlyOneExpandedItem = mOnlyOneExpandedItem;
            return this;
        }

        /**
     * @return if there should be only one expanded, expandable item in the list
     */
        public bool IsOnlyOneExpandedItem
        {
            get { return mOnlyOneExpandedItem; }
        }

        /**
         * re-selects all elements stored in the savedInstanceState
         * IMPORTANT! Call this method only after all items where added to the adapters again. Otherwise it may select wrong items!
         *
         * @param savedInstanceState If the activity is being re-initialized after
         *                           previously being shut down then this Bundle contains the data it most
         *                           recently supplied in Note: Otherwise it is null.
         * @return this
         */
        public FastAdapter<T, VH> withSavedInstanceState(Bundle savedInstanceState)
        {
            return WithSavedInstanceState(savedInstanceState, "");
        }

        /**
         * re-selects all elements stored in the savedInstanceState
         * IMPORTANT! Call this method only after all items where added to the adapters again. Otherwise it may select wrong items!
         *
         * @param savedInstanceState If the activity is being re-initialized after
         *                           previously being shut down then this Bundle contains the data it most
         *                           recently supplied in Note: Otherwise it is null.
         * @param prefix             a prefix added to the savedInstance key so we can store multiple states
         * @return this
         */
        public FastAdapter<T, VH> WithSavedInstanceState(Bundle savedInstanceState, string prefix)
        {
            if (savedInstanceState != null)
            {
                //make sure already done selections are removed
                deselect();

                if (mPositionBasedStateManagement)
                {
                    //first restore opened collasable items, as otherwise may not all selections could be restored
                    var expandedItems = savedInstanceState.GetIntArray(BUNDLE_EXPANDED + prefix);
                    if (expandedItems != null)
                    {
                        foreach (var expandedItem in expandedItems)
                        {
                            expand(expandedItem);
                        }
                    }

                    //restore the selections
                    var selections = savedInstanceState.GetIntArray(BUNDLE_SELECTIONS + prefix);
                    if (selections != null)
                    {
                        foreach (var selection in selections)
                        {
                            select(selection);
                        }
                    }
                }
                else
                {
                    var expandedItems = savedInstanceState.GetStringArrayList(BUNDLE_EXPANDED + prefix);
                    var selectedItems = savedInstanceState.GetStringArrayList(BUNDLE_SELECTIONS + prefix);

                    for (int i = 0; i < GetItemCount; i++)
                    {
                        IItem<T, VH> item = GetItem(i);
                        string id = item.Identifaybale.ToString();
                        if (expandedItems != null && expandedItems.Contains(id))
                        {
                            expand(i);
                        }
                        if (selectedItems != null && selectedItems.Contains(id))
                        {
                            select(i);
                        }

                        //we also have to restore the selections for subItems
                        AdapterUtil.RestoreSubItemSelectionStatesForAlternativeStateManagement(item, selectedItems);
                    }
                }
            }
            return this;
        }

        /**
     * registers an AbstractAdapter which will be hooked into the adapter chain
     *
     * @param adapter an adapter which extends the AbstractAdapter
     */
        public void RegisterAdapter(AbstractAdapter<T, VH> adapter)
        {
            if (!mAdapters.ContainsKey(adapter.Order))
            {
                mAdapters[adapter.Order] = adapter;
                cacheSizes();
            }
        }

        /**
         * register a new type into the TypeInstances to be able to efficiently create thew ViewHolders
         *
         * @param item an IItem<T, VH> which will be shown in the list
         */
        public void RegisterTypeInstance(IItem<T, VH> item)
        {
            if (!mTypeInstances.ContainsKey(item.Type))
            {
                mTypeInstances[item.Type] = item;
            }
        }

        /**
         * gets the TypeInstance remembered within the FastAdapter for an item
         *
         * @param type the int type of the item
         * @return the Item typeInstance
         */
        public IItem<T, VH> GetTypeInstance(int type)
        {
            return mTypeInstances[type];
        }

        /**
         * helper method to get the position from a holder
         * overwrite this if you have an adapter adding additional items inbetwean
         *
         * @param holder the viewHolder of the item
         * @return the position of the holder
         */
        public int GetHolderAdapterPosition(RecyclerView.ViewHolder holder)
        {
            return holder.AdapterPosition;
        }

        /**
     * Creates the ViewHolder by the viewType
     *
     * @param parent   the parent view (the RecyclerView)
     * @param viewType the current viewType which is bound
     * @return the ViewHolder with the bound data
     */

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            RecyclerView.ViewHolder holder = mOnCreateViewHolderListener.OnPreCreateViewHolder(parent, viewType);

            //handle click behavior
            holder.ItemView.Click += (s, e) =>
            {

                int pos = GetHolderAdapterPosition(holder);
                if (pos != RecyclerView.NoPosition)
                {
                    bool consumed = false;
                    RelativeInfo<IItem<T, VH>> relativeInfo = GetRelativeInfo(pos);
                    Item item = relativeInfo.item;
                    if (item != null && item.isEnabled())
                    {
                        //on the very first we call the click listener from the item itself (if defined)
                        if (item is IClickable && ((IClickable)item).OnPreItemClickListener != null)
                        {
                            consumed = ((IClickable<IItem<T, VH>>)item).OnPreItemClickListener.OnClick(v, relativeInfo.Adapter, item, pos);
                        }

                        //first call the onPreClickListener which would allow to prevent executing of any following code, including selection
                        if (!consumed && mOnPreClickListener != null)
                        {
                            consumed = mOnPreClickListener.OnClick(v, relativeInfo.Adapter, item, pos);
                        }

                        //if this is a expandable item :D
                        if (!consumed && item is IExpandable)
                        {
                            if (((IExpandable)item).IsAutoExpanding() && ((IExpandable)item).SubItems != null)
                            {
                                toggleExpandable(pos);
                            }
                        }

                        //if there should be only one expanded item we want to collapse all the others but the current one
                        if (mOnlyOneExpandedItem)
                        {
                            int[] expandedItems = getExpandedItems();
                            for (int i = expandedItems.Count() - 1; i >= 0; i--)
                            {
                                if (expandedItems[i] != pos)
                                {
                                    collapse(expandedItems[i], true);
                                }
                            }
                        }

                        //handle the selection if the event was not yet consumed, and we are allowed to select an item (only occurs when we select with long click only)
                        if (!consumed && !mSelectOnLongClick && mSelectable)
                        {
                            handleSelection(v, item, pos);
                        }

                        //before calling the global adapter onClick listener call the item specific onClickListener
                        if (item is IClickable && ((IClickable)item).OnItemClickListener != null)
                        {
                            consumed = ((IClickable<IItem<T, VH>>)item).OnItemClickListener.OnClick(v, relativeInfo.Adapter, item, pos);
                        }

                        //call the normal click listener after selection was handlded
                        if (!consumed && mOnClickListener != null)
                        {
                            mOnClickListener.OnClick(v, relativeInfo.Adapter, item, pos);
                        }
                    }
                }
            };

            //handle long click behavior
            holder.ItemView.LongClick += (s, e) =>
            {
                int pos = GetHolderAdapterPosition(holder);
                if (pos != RecyclerView.NoPosition)
                {
                    bool consumed = false;
                    RelativeInfo<IItem<T, VH>> relativeInfo = getRelativeInfo(pos);
                    if (relativeInfo.Item != null && relativeInfo.Item.isEnabled())
                    {
                    //first call the OnPreLongClickListener which would allow to prevent executing of any following code, including selection
                    if (mOnPreLongClickListener != null)
                        {
                            consumed = mOnPreLongClickListener.OnLongClick(v, relativeInfo.adapter, relativeInfo.Item, pos);
                        }

                    //now handle the selection if we are in multiSelect mode and allow selecting on longClick
                    if (!consumed && mSelectOnLongClick && mSelectable)
                        {
                            handleSelection(v, relativeInfo.Item, pos);
                        }

                    //call the normal long click listener after selection was handled
                    if (mOnLongClickListener != null)
                        {
                            consumed = mOnLongClickListener.OnLongClick(v, relativeInfo.Adapter, relativeInfo.Item, pos);
                        }
                    }
                    return consumed;
                }
                return false;
            };

            //handle touch behavior
            holder.ItemView.Touch += (s, e) =>
            {
                if (mOnTouchListener != null)
                {
                    int pos = GetHolderAdapterPosition(holder);
                    if (pos != RecyclerView.NoPosition)
                    {
                        RelativeInfo<IItem<T, VH>> relativeInfo = getRelativeInfo(pos);
                        return mOnTouchListener.OnTouch(s, e.Event, relativeInfo.Adapter, relativeInfo.Item, pos);
                    }
                }
                return false;
            };

            return mOnCreateViewHolderListener.OnPostCreateViewHolder(holder);
        }


        /**
     * Binds the data to the created ViewHolder and sets the listeners to the holder.itemView
     *
     * @param holder   the viewHolder we bind the data on
     * @param position the global position
     */
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            mOnBindViewHolderListener.OnBindViewHolder(holder, position);
        }

        /**
         * Searches for the given item and calculates it's global position
         *
         * @param item the item which is searched for
         * @return the global position, or -1 if not found
         */
        public int GetPosition(IItem<T, VH> item)
        {
            if (item.Identifaybale == -1)
            {
                Log.Error("FastAdapter", "You have to define an identifier for your item to retrieve the position via this method");
                return -1;
            }

            int position = 0;
            int length = mAdapters.Count;
            for (int i = 0; i < length; i++)
            {
                IAdapter<IItem<T, VH>> adapter = mAdapters[i];
                if (adapter.Order < 0)
                {
                    continue;
                }

                int relativePosition = adapter.GetAdapterPosition(item);
                if (relativePosition != -1)
                {
                    return position + relativePosition;
                }
                position = adapter.AdapterItemCount;
            }

            return -1;
        }

        /**
         * gets the IItem<T, VH> by a position, from all registered adapters
         *
         * @param position the global position
         * @return the found IItem<T, VH> or null
         */
        public IItem<T, VH> GetItem(int position)
        {
            //if we are out of range just return null
            if (position < 0 || position >= mGlobalSize)
            {
                return null;
            }
            //now get the adapter which is responsible for the given position
            Map.Entry<Integer, IAdapter<Item>> entry = mAdapterSizes.floorEntry(position);
            return entry.getValue().getAdapterItem(position - entry.getKey());
        }

        /**
         * Internal method to get the Item as ItemHolder which comes with the relative position within it's adapter
         * Finds the responsible adapter for the given position
         *
         * @param position the global position
         * @return the adapter which is responsible for this position
         */
        public RelativeInfo<Item> getRelativeInfo(int position)
        {
            if (position < 0)
            {
                return new RelativeInfo<>();
            }

            RelativeInfo<Item> relativeInfo = new RelativeInfo<>();
            Map.Entry<Integer, IAdapter<Item>> entry = mAdapterSizes.floorEntry(position);
            if (entry != null)
            {
                relativeInfo.item = entry.getValue().getAdapterItem(position - entry.getKey());
                relativeInfo.adapter = entry.getValue();
                relativeInfo.position = position;
            }
            return relativeInfo;
        }

        /**
         * Gets the adapter for the given position
         *
         * @param position the global position
         * @return the adapter responsible for this global position
         */
        public IAdapter<Item> getAdapter(int position)
        {
            //if we are out of range just return null
            if (position < 0 || position >= mGlobalSize)
            {
                return null;
            }
            //now get the adapter which is responsible for the given position
            return mAdapterSizes.floorEntry(position).getValue();
        }

        /**
         * finds the int ItemViewType from the IItem<T, VH> which exists at the given position
         *
         * @param position the global position
         * @return the viewType for this position
         */
        @Override
    public int getItemViewType(int position)
        {
            return getItem(position).getType();
        }

        /**
         * finds the int ItemId from the IItem<T, VH> which exists at the given position
         *
         * @param position the global position
         * @return the itemId for this position
         */
        
        public override long GetItemId(int position)
        {
            return getItem(position).Identifyable;
        }

        /**
         * calculates the total ItemCount over all registered adapters
         *
         * @return the global count
         */
        public int GetItemCount
        {
            get { return mGlobalSize; }
        }

        /**
         * calculates the item count up to a given (excluding this) order number
         *
         * @param order the number up to which the items are counted
         * @return the total count of items up to the adapter order
         */
        public int GetPreItemCountByOrder(int order)
        {
            //if we are empty just return 0 count
            if (mGlobalSize == 0)
            {
                return 0;
            }

            int size = 0;

            //count the number of items before the adapter with the given order
            foreach (var adapter in  mAdapters)
            {
                if (adapter.Order == order)
                {
                    return size;
                }
                else
                {
                    size = size + adapter.GetAdapterItemCount;
                }
            }

            //get the count of items which are before this order
            return size;
        }


        /**
         * calculates the item count up to a given (excluding this) adapter (defined by the global position of the item)
         *
         * @param position the global position of an adapter item
         * @return the total count of items up to the adapter which holds the given position
         */
        public int GetPreItemCount(int position)
        {
            //if we are empty just return 0 count
            if (mGlobalSize == 0)
            {
                return 0;
            }

            //get the count of items which are before this order
            return mAdapterSizes.floorKey(position);
        }


        public class RelativeInfo<IItem<T, VH>> 
        {
            public IAdapter<IItem<T, VH>> Adapter { get; set; }
            public IItem<T, VH> Item { get; set; }
            public int Position { get; set; } = -1;
        }
    }
}