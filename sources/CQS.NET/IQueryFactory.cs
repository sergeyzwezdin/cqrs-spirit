namespace CQS.NET
{
	public interface IQueryFactory
	{
		T ResolveQuery<T>()
			where T : class, IQuery;
	}
}