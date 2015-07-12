using System.Threading.Tasks;

namespace CQSNET
{
    public interface ICommandsRunner
    {
        void Execute<T>(T command)
            where T : class, ICommand;

        Task ExecuteAsync<T>(T command)
            where T : class, ICommand;
    }
}