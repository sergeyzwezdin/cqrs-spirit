namespace CQS.NET
{
	public interface ICommandsFactory
	{
		void ExecuteCommand<T>(T command)
			where T : class, ICommand;
	}
}