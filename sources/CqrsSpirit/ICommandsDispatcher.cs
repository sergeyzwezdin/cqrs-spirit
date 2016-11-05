using System.Threading.Tasks;

namespace CqrsSpirit
{
    /// <summary>
    /// Commands dispatcher interface
    /// </summary>
    public interface ICommandsDispatcher
    {
        /// <summary>
        /// Execute command
        /// </summary>
        /// <typeparam name="T">Command type to handle</typeparam>
        /// <param name="command">Command to handle</param>
        Task ExecuteAsync<T>(T command) where T : class, ICommand;
    }
}