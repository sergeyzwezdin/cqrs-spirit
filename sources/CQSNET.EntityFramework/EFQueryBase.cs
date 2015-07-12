using System.Data.Entity;

namespace CQSNET.EntityFramework
{
    /// <summary>
    /// Base class for query based on Entity Framework
    /// </summary>
    /// <typeparam name="TContext">Type of data context</typeparam>
    public abstract class EFQueryBase<TContext> : IQuery
        where TContext : DbContext
    {
        /// <summary>
        /// Data context
        /// </summary>
        protected readonly TContext DbContext;

        /// <summary>
        /// Constructs query with some data source
        /// </summary>
        /// <param name="dbContext">Data context</param>
        protected EFQueryBase(TContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
