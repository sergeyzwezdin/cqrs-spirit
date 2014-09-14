using System;

namespace CQS.NET
{
	public class WorkflowFactory : IWorkflowFactory
	{
		private readonly Func<Type, object> _resolveCallback;

		public WorkflowFactory(Func<Type, object> resolveCallback)
		{
			_resolveCallback = resolveCallback;
		}

		public T Resolve<T>()
			where T : class, IWorkflow
		{
			return _resolveCallback(typeof(T)) as T;
		}
	}
}