using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQSNET.Infrastructure
{
    public class CommandsRunner : ICommandsRunner
    {
        private readonly Func<Type, IEnumerable<object>> _resolveCallback;


        public CommandsRunner(Func<Type, IEnumerable<object>> resolveCallback)
        {
            _resolveCallback = resolveCallback;
        }


        public void Execute<T>(T command)
            where T : class, ICommand
        {
            // Initialize context
            IEnumerable<ICommandHandler<T>> commandHandlers =
                _resolveCallback(typeof(ICommandHandler<T>))
                .OfType<ICommandHandler<T>>()
                .ToArray();

            IEnumerable<ICommandAsyncHandler<T>> asyncCommandHandlers =
                _resolveCallback(typeof(ICommandAsyncHandler<T>))
                .OfType<ICommandAsyncHandler<T>>()
                .ToArray();

            if (commandHandlers.Any() || asyncCommandHandlers.Any())
            {
                List<Task> commandsTasks = new List<Task>();

                // Execute command
                foreach (ICommandHandler<T> commandHandler in commandHandlers)
                {
                    commandHandler.Execute(command);
                }

                foreach (ICommandAsyncHandler<T> commandHandler in asyncCommandHandlers)
                {
                    commandsTasks.Add(commandHandler.ExecuteAsync(command));
                }

                Task.WaitAll(commandsTasks.ToArray());

                // Dispose context
                foreach (ICommandHandler<T> commandHandler in commandHandlers)
                {
                    commandHandler.Dispose();
                }
                foreach (ICommandAsyncHandler<T> commandHandler in asyncCommandHandlers)
                {
                    commandHandler.Dispose();
                }
            }
            else
                throw new ArgumentException("Unknown command \"" + typeof(T).FullName + "\"");
        }

        public async Task ExecuteAsync<T>(T command) where T : class, ICommand
        {
            // Initialize context
            IEnumerable<ICommandHandler<T>> commandHandlers =
                _resolveCallback(typeof(ICommandHandler<T>))
                .OfType<ICommandHandler<T>>()
                .ToArray();

            IEnumerable<ICommandAsyncHandler<T>> asyncCommandHandlers =
                _resolveCallback(typeof(ICommandAsyncHandler<T>))
                .OfType<ICommandAsyncHandler<T>>()
                .ToArray();

            if (commandHandlers.Any() || asyncCommandHandlers.Any())
            {
                List<Task> commandsTasks = new List<Task>();

                // Execute command
                foreach (ICommandHandler<T> commandHandler in commandHandlers)
                {
                    ICommandHandler<T> handler = commandHandler;
                    commandsTasks.Add(Task.Factory.StartNew(() => handler.Execute(command)));
                }

                foreach (ICommandAsyncHandler<T> commandHandler in asyncCommandHandlers)
                {
                    commandsTasks.Add(commandHandler.ExecuteAsync(command));
                }

                await Task.WhenAll(commandsTasks.ToArray());

                // Dispose context
                foreach (ICommandHandler<T> commandHandler in commandHandlers)
                {
                    commandHandler.Dispose();
                }

                foreach (ICommandAsyncHandler<T> commandHandler in asyncCommandHandlers)
                {
                    commandHandler.Dispose();
                }
            }
            else
                throw new ArgumentException("Unknown command \"" + typeof(T).FullName + "\"");
        }
    }
}