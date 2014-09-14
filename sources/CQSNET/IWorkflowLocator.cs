namespace CQSNET
{
	public interface IWorkflowLocator
	{
		T Resolve<T>()
			where T : class, IWorkflow;
	}
}