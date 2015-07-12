using System;

namespace CQSNET.Infrastructure
{
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
}