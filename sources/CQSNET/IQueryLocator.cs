namespace CQSNET
{
    /// <summary>
    /// Public interface for query locator
    /// </summary>
    public interface IQueryLocator
    {
        /// <summary>
        /// Resolve query by type
        /// </summary>
        /// <typeparam name="T">Query type</typeparam>
        /// <returns>Query</returns>
        T Resolve<T>()
            where T : class, IQuery;
    }
}