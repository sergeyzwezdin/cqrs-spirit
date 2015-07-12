using System;
using System.Threading.Tasks;

namespace CQSNET
{
    /// <summary>
    /// Async command handler
    /// </summary>
    /// <typeparam name="TCommand">Command type</typeparam>
    public interface ICommandAsyncHandler<in TCommand> : IDisposable
        where TCommand : ICommand
    {
        /// <summary>
        /// Execute command
        /// </summary>
        /// <param name="command">Command type</param>
        /// <returns>Task</returns>
        Task ExecuteAsync(TCommand command);
    }
}