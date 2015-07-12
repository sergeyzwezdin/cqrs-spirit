namespace CQSNET
{
    /// <summary>
    /// Public interface for workflow locator
    /// </summary>
    public interface IWorkflowLocator
    {
        /// <summary>
        /// Resolve workflow
        /// </summary>
        /// <typeparam name="T">Workflow type</typeparam>
        /// <returns>Workflow</returns>
        T Resolve<T>()
            where T : class, IWorkflow;
    }
}