using System;
using System.Threading.Tasks;

namespace CqrsSpirit
{
    /// <summary>
    /// Interface for command handler
    /// </summary>
    /// <typeparam name="TCommand">Command type that handled by this handler</typeparam>
    public interface ICommandHandler<in TCommand> : IDisposable
            where TCommand : ICommand
    {
        /// <summary>
        /// Execute commahnd handler
        /// </summary>
        /// <param name="command">Command to handle</param>
        /// <returns></returns>
        Task ExecuteAsync(TCommand command);
    }
}