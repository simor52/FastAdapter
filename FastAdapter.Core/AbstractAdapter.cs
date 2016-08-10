using Android.Support.V7.Widget;
using Android.Views;
using System.Collections.Generic;
using Java.Lang;
using System;

namespace FastAdapter.Core
{
    public abstract class AbstractAdapter<T, VH> : RecyclerView.Adapter, IAdapter<T, VH> where VH : RecyclerView.ViewHolder where T : class
    {
        //private AbstractAdapter mParentAdapter;
        //keep a reference to the FastAdapter which contains the base logic
        private FastAdapter<T, VH> mFastAdapter;

        /**
         * Wrap the FastAdapter with this AbstractAdapter and keep it's reference to forward all events correctly
         *
         * @param fastAdapter the FastAdapter which contains the base logic
         * @return this
         */
        public AbstractAdapter<T, VH> Wrap(FastAdapter<T, VH> fastAdapter)
        {
            //this.mParentAdapter = abstractAdapter;
            this.mFastAdapter = fastAdapter;
            this.mFastAdapter.RegisterAdapter(this);
            return this;
        }

        /**
         * Wrap the AbstractAdapter with this AbstractAdapter and keep the reference to it's FastAdapter to which we forward all events correctly
         *
         * @param abstractAdapter an AbstractWrapper which wraps another AbstractAdapter or FastAdapter
         * @return this
         */
        public AbstractAdapter<T, VH> Wrap(IAdapter<T, VH> abstractAdapter)
        {
            //this.mParentAdapter = abstractAdapter;
            this.mFastAdapter = abstractAdapter.FastAdapter;
            this.mFastAdapter.RegisterAdapter(this);
            return this;
        }

        /**
         * overwrite the registerAdapterDataObserver to correctly forward all events to the FastAdapter
         *
         * @param observer
         */
        
        public override void RegisterAdapterDataObserver(RecyclerView.AdapterDataObserver observer)
        {
            base.RegisterAdapterDataObserver(observer);
            if (mFastAdapter != null)
            {
                mFastAdapter.RegisterAdapterDataObserver(observer);
            }
        }

        /**
         * overwrite the unregisterAdapterDataObserver to correctly forward all events to the FastAdapter
         *
         * @param observer
         */
     
        public override void UnregisterAdapterDataObserver(RecyclerView.AdapterDataObserver observer)
        {
            base.UnregisterAdapterDataObserver(observer);
            if (mFastAdapter != null)
            {
                mFastAdapter.UnregisterAdapterDataObserver(observer);
            }
        }

        /**
         * overwrite the getItemViewType to correctly return the value from the FastAdapter
         *
         * @param position
         * @return
         */
        public override int GetItemViewType(int position)
        {
            return mFastAdapter.GetItemViewType(position);
        }

        /**
         * overwrite the getItemId to correctly return the value from the FastAdapter
         *
         * @param position
         * @return
         */
        
        public override long GetItemId(int position)
        {
            return mFastAdapter.GetItemId(position);
        }

        /**
         * @return the reference to the FastAdapter
         */
        
        public FastAdapter<T, VH> FastAdapter
        {
            get { return mFastAdapter; }
        }

        /**
         * make sure we return the Item from the FastAdapter so we retrieve the item from all adapters
         *
         * @param position
         * @return
         */
        public IItem<T, VH> this[int index]
        {
            get { return mFastAdapter.GetItem(index); }
        }

        /**
         * make sure we return the count from the FastAdapter so we retrieve the count from all adapters
         *
         * @return
         */
        
        public override int ItemCount
        {
            get { return mFastAdapter.ItemCount; }
        }

        /**
         * the onCreateViewHolder is managed by the FastAdapter so forward this correctly
         *
         * @param parent
         * @param viewType
         * @return
         */
        
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return mFastAdapter.OnCreateViewHolder(parent, viewType);
        }

        /**
         * the onBindViewHolder is managed by the FastAdapter so forward this correctly
         *
         * @param holder
         * @param position
         */
        
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            mFastAdapter.OnBindViewHolder(holder, position);
        }

        /**
         * the onBindViewHolder is managed by the FastAdapter so forward this correctly
         *
         * @param holder
         * @param position
         * @param payloads
         */

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position, IList<Java.Lang.Object> payloads)
        {
            mFastAdapter.OnBindViewHolder(holder, position, payloads);
        }
        /**
         * the setHasStableIds is managed by the FastAdapter so forward this correctly
         *
         * @param hasStableIds
         */

        public new bool HasStableIds
        {
            get { return mFastAdapter.HasStableIds; }
            set { base.HasStableIds = value; }
        }

        public int Order
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int AdapterItemCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public List<IItem<T, VH>> AdapterItems
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /**
         * the onViewRecycled is managed by the FastAdapter so forward this correctly
         *
         * @param holder
         */

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            var vh = holder as RecyclerView.ViewHolder;
            if (vh == null)
                return;
            mFastAdapter.OnViewRecycled(vh);
        }

        /**
         * the onFailedToRecycleView is managed by the FastAdapter so forward this correctly
         *
         * @param holder
         * @return
         */
        public override bool OnFailedToRecycleView(Java.Lang.Object holder)
        {
            var vh = holder as RecyclerView.ViewHolder;
            if (vh == null)
                return false;
            return mFastAdapter.OnFailedToRecycleView(vh);
        }

        /**
         * the onViewDetachedFromWindow is managed by the FastAdapter so forward this correctly
         *
         * @param holder
         */
        public override void OnViewDetachedFromWindow(Java.Lang.Object holder)
        {
            var vh = holder as RecyclerView.ViewHolder;
            if (vh == null)
                return;
            mFastAdapter.OnViewDetachedFromWindow(vh);
        }

        /**
         * the onViewAttachedToWindow is managed by the FastAdapter so forward this correctly
         *
         * @param holder
         */
        public override void OnViewAttachedToWindow(Java.Lang.Object holder)
        {
            var vh = holder as RecyclerView.ViewHolder;
            if (vh == null)
                return;
            mFastAdapter.OnViewAttachedToWindow(vh);
        }

        /**
         * the onAttachedToRecyclerView is managed by the FastAdapter so forward this correctly
         *
         * @param recyclerView
         */
        public override void OnAttachedToRecyclerView(RecyclerView recyclerView)
        {
            mFastAdapter.OnAttachedToRecyclerView(recyclerView);
        }

        /**
         * the onDetachedFromRecyclerView is managed by the FastAdapter so forward this correctly
         *
         * @param recyclerView
         */
        public override void OnDetachedFromRecyclerView(RecyclerView recyclerView)
        {
            mFastAdapter.OnDetachedFromRecyclerView(recyclerView);
        }

        /**
         * internal mapper to remember and add possible types for the RecyclerView
         *
         * @param items
         */
        public void MapPossibleTypes(IEnumerable<IItem<T, VH>> items)
        {
            if (items != null)
            {
                foreach(var item in items)
                {
                    MapPossibleType(item);
                }
            }
        }

        /**
         * internal mapper to remember and add possible types for the RecyclerView
         *
         * @param item
         */
        public void MapPossibleType(IItem<T, VH> item)
        {
            mFastAdapter.RegisterTypeInstance(item);
        }

        public IItem<T, VH> GetAdapterItem(int position)
        {
            throw new NotImplementedException();
        }

        public int GetAdapterPosition(IItem<T, VH> item)
        {
            throw new NotImplementedException();
        }

        public int GetGlobalPosition(int position)
        {
            throw new NotImplementedException();
        }
    }
}