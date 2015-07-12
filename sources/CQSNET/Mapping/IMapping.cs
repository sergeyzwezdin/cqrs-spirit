namespace CQSNET.Mapping
{
    /// <summary>
    /// Public interface for mapping definition (determines how to map exact type to another)
    /// </summary>
    /// <typeparam name="TFrom">From type</typeparam>
    /// <typeparam name="TTo">To type</typeparam>
    public interface IMapping<in TFrom, out TTo>
    {
        /// <summary>
        /// Map
        /// </summary>
        /// <param name="source">Initial object</param>
        /// <returns>Result of mapping</returns>
        TTo Map(TFrom source);
    }
}