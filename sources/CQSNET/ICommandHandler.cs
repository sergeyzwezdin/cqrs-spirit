using System;

namespace CQSNET
{
    /// <summary>
    /// Command handler
    /// </summary>
    /// <typeparam name="TCommand">Command type</typeparam>
    public interface ICommandHandler<in TCommand> : IDisposable
        where TCommand : ICommand
    {
        /// <summary>
        /// Execute command
        /// </summary>
        /// <param name="command">Command</param>
        void Execute(TCommand command);
    }
}