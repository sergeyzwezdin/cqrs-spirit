using System;

namespace CQSNET.Objects
{
    /// <summary>
    /// Base class for command based on unversal object store
    /// </summary>
    /// <typeparam name="TCommand">Command type</typeparam>
    /// <typeparam name="TContext">Type of source</typeparam>
    public abstract class ObjectCommandHandlerBase<TCommand, TContext> : ICommandHandler<TCommand>, IDisposable
        where TCommand : ICommand
        where TContext : class
    {
        /// <summary>
        /// Data source
        /// </summary>
        protected readonly TContext DbContext;

        /// <summary>
        /// Constructs command handler with some data source
        /// </summary>
        /// <param name="dbContext">Data source</param>
        protected ObjectCommandHandlerBase(TContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>
        /// Exceutes command handler
        /// </summary>
        /// <param name="command">Command</param>
        public abstract void Execute(TCommand command);

        /// <summary>
        /// Command handler dispose
        /// </summary>
        public void Dispose()
        {
            IDisposable disposableContext = DbContext as IDisposable;

            if (disposableContext != null)
                disposableContext.Dispose();
        }
    }
}