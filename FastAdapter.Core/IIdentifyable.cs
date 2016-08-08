namespace FastAdapter.Core
{
    public interface IIdentifyable<T> where T : class
    {
        /// <summary>
        /// Sets the identifier of this item and returns itS.
        /// </summary>
        /// <param name="identifyable">The identifier which will be used (also for getItemId() inside the adapter)</param>
        /// <returns></returns>
        T WithIdentifyable(long identifyable);

        /// <summary>
        /// Gets the identifier of this item.
        /// </summary>
        long Identifaybale { get; }
    }
}