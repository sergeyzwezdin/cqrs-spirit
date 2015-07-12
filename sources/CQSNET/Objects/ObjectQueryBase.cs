namespace CQSNET.Objects
{
    public abstract class ObjectQueryBase<TContext> : IQuery
        where TContext : class
    {
        protected readonly TContext DbContext;

        protected ObjectQueryBase(TContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}