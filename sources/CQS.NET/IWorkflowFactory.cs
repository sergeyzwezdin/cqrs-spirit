namespace CQS.NET
{
	public interface IWorkflowFactory
	{
		T Resolve<T>()
			where T : class, IWorkflow;
	}
}