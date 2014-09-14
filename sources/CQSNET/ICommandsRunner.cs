namespace CQSNET
{
	public interface ICommandsRunner
	{
		void Execute<T>(T command)
			where T : class, ICommand;
	}
}