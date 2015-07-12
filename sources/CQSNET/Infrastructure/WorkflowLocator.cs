using System;

namespace CQSNET.Infrastructure
{
#pragma warning disable 1591
    public class WorkflowLocator : IWorkflowLocator
    {
        private readonly Func<Type, object> _resolveCallback;

        public WorkflowLocator(Func<Type, object> resolveCallback)
        {
            _resolveCallback = resolveCallback;
        }

        public T Resolve<T>()
            where T : class, IWorkflow
        {
            return _resolveCallback(typeof(T)) as T;
        }
    }
#pragma warning restore 1591
}