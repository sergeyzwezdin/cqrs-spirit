using System;
using System.Data.Entity;

namespace CQSNET.EntityFramework
{
    /// <summary>
    /// Base class for command based on Entity Framework
    /// </summary>
    /// <typeparam name="TCommand">Command type</typeparam>
    /// <typeparam name="TContext">Type of source</typeparam>
    public abstract class EFCommandHandlerBase<TCommand, TContext> : ICommandHandler<TCommand>, IDisposable
        where TCommand : ICommand
        where TContext : DbContext
    {
        /// <summary>
        /// Data context
        /// </summary>
        protected readonly TContext DbContext;

        /// <summary>
        /// Constructs command handler with some data source
        /// </summary>
        /// <param name="dbContext">Data source</param>
        protected EFCommandHandlerBase(TContext dbContext)
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
            DbContext.Dispose();
        }
    }
}