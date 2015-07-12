using System;

namespace CQSNET.Objects
{
    public abstract class ObjectCommandHandlerBase<TCommand, TContext> : ICommandHandler<TCommand>, IDisposable
        where TCommand : ICommand
        where TContext : class
    {
        protected readonly TContext DbContext;

        protected ObjectCommandHandlerBase(TContext dbContext)
        {
            DbContext = dbContext;
        }

        public abstract void Execute(TCommand command);

        public void Dispose()
        {
            IDisposable disposableContext = DbContext as IDisposable;

            if (disposableContext != null)
                disposableContext.Dispose();
        }
    }
}