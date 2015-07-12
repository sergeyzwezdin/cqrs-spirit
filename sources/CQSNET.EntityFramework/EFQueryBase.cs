using System.Data.Entity;

namespace CQSNET.EntityFramework
{
    public abstract class EFQueryBase<TContext> : IQuery
        where TContext : DbContext
    {
        protected readonly TContext DbContext;

        protected EFQueryBase(TContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
