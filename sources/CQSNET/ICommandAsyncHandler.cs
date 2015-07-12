using System;
using System.Threading.Tasks;

namespace CQSNET
{
    public interface ICommandAsyncHandler<in TCommand> : IDisposable
        where TCommand : ICommand
    {
        Task ExecuteAsync(TCommand command);
    }
}