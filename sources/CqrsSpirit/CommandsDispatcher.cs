using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CqrsSpirit
{
    public class CommandsDispatcher : ICommandsDispatcher
    {
        protected readonly ILogger<CommandsDispatcher> Logger;

        protected readonly Func<Type, IEnumerable<object>> ResolveCallback;

        public CommandsDispatcher(IServiceProvider serviceProvider, ILogger<CommandsDispatcher> logger)
            : this(type => new[] { serviceProvider.GetService(type) }, logger)
        {
        }

        public CommandsDispatcher(Func<Type, IEnumerable<object>> resolveCallback, ILogger<CommandsDispatcher> logger)
        {
            ResolveCallback = resolveCallback;
            Logger = logger;
        }

        public async Task ExecuteAsync<T>(T command) where T : class, ICommand
        {
            // Initialize context
            ICommandHandler<T>[] commandHandlers = null;
            try
            {
                commandHandlers =
                        ResolveCallback(typeof(ICommandHandler<T>))
                        .OfType<ICommandHandler<T>>()
                        .ToArray();
            }
            catch (Exception ex)
            {
                Logger?.LogWarning(new EventId(), ex, "Unable to resolve handlers for {Command} command", typeof(T).FullName);
                throw;
            }

            if (commandHandlers?.Any() ?? false)
            {
                List<Task> commandsTasks = new List<Task>();

                Logger?.LogInformation("Processing {Command} command", typeof(T).FullName);

                Stopwatch stopwatch = null;
                if (Logger != null)
                    stopwatch = Stopwatch.StartNew();

                // Execute command
                foreach (ICommandHandler<T> commandHandler in commandHandlers)
                {
                    Logger?.LogInformation("Running {Handler} to process {Command} command", commandHandler.GetType().FullName, typeof(T).FullName);
                    commandsTasks.Add(commandHandler.ExecuteAsync(command));
                }

                await Task.WhenAll(commandsTasks.ToArray());

                // Dispose context
                foreach (ICommandHandler<T> commandHandler in commandHandlers)
                {
                    commandHandler.Dispose();
                }

                stopwatch?.Stop();
                Logger?.LogInformation("{Command} processed succesfully in {Duration}ms", typeof(T).FullName, stopwatch?.ElapsedMilliseconds);
            }
            else
            {
                Logger?.LogWarning("There is no registered handlers for {Command} command", typeof(T).FullName);
                throw new ArgumentException("Unknown command \"" + typeof(T).FullName + "\"", nameof(command));
            }
        }
    }
}

