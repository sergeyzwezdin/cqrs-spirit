using System;
using System.Threading.Tasks;

namespace CqrsSpirit.Objects
{
    /// <summary>
    /// Base class for command handled based on unviersal object store
    /// </summary>
    /// <typeparam name="TCommand">Command type that handled by this handler</typeparam>
    /// <typeparam name="TContext">Type of source</typeparam>
    public abstract class ObjectCommandHandlerBase<TCommand, TContext> : ICommandHandler<TCommand>,
                                                                         IDisposable
            where TCommand : ICommand
            where TContext : class
    {
        /// <summary>
        /// Data source
        /// </summary>
        protected readonly TContext DbContext;

        /// <summary>
        /// Constructs handler with some data source
        /// </summary>
        /// <param name="dbContext">Data source</param>
        protected ObjectCommandHandlerBase(TContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>
        /// Execute commahnd handler
        /// </summary>
        /// <param name="command">Command to handle</param>
        /// <returns></returns>
        public abstract Task ExecuteAsync(TCommand command);

        public void Dispose()
        {
            (DbContext as IDisposable)?.Dispose();
        }
    }
}