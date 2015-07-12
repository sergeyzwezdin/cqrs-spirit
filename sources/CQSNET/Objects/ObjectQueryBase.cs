namespace CQSNET.Objects
{
    /// <summary>
    /// Base class for query based on unversal object store
    /// </summary>
    /// <typeparam name="TContext">Type of source</typeparam>
    public abstract class ObjectQueryBase<TContext> : IQuery
        where TContext : class
    {
        /// <summary>
        /// Data source
        /// </summary>
        protected readonly TContext DbContext;

        /// <summary>
        /// Constructs query with some data source
        /// </summary>
        /// <param name="dbContext">Data source</param>
        protected ObjectQueryBase(TContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}