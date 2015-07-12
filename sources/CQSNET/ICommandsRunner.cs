using System.Threading.Tasks;

namespace CQSNET
{
    /// <summary>
    /// Commands runner interface
    /// </summary>
    public interface ICommandsRunner
    {
        /// <summary>
        /// Execute command
        /// </summary>
        /// <typeparam name="T">Command type</typeparam>
        /// <param name="command">Command</param>
        void Execute<T>(T command)
            where T : class, ICommand;

        /// <summary>
        /// Execute command
        /// </summary>
        /// <typeparam name="T">Command type</typeparam>
        /// <param name="command">Command</param>
        /// <returns>Task</returns>
        Task ExecuteAsync<T>(T command)
            where T : class, ICommand;
    }
}