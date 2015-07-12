namespace CQSNET
{
    public interface IQueryLocator
    {
        T Resolve<T>()
            where T : class, IQuery;
    }
}