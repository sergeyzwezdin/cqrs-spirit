using System;

namespace CQSNET.Infrastructure
{
	public class QueryLocator : IQueryLocator
	{
		private readonly Func<Type, object> _resolveCallback;

		public QueryLocator(Func<Type, object> resolveCallback)
		{
			_resolveCallback = resolveCallback;
		}

		public T Resolve<T>()
			where T : class, IQuery
		{
			return _resolveCallback(typeof(T)) as T;
		}
	}
}