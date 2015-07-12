using System;

namespace CQSNET.Mapping
{
    /// <summary>
    /// Public interface for mapper
    /// </summary>
    public interface IMapper
    {
        /// <summary>
        /// Map object
        /// </summary>
        /// <typeparam name="TFrom">From type</typeparam>
        /// <typeparam name="TTo">To type</typeparam>
        /// <param name="source">Source object</param>
        /// <returns>Result of mapping</returns>
        TTo Map<TFrom, TTo>(TFrom source);

        /// <summary>
        /// Map object
        /// </summary>
        /// <typeparam name="TTo">To type</typeparam>
        /// <param name="source">Source object</param>
        /// <returns>Result of mapping</returns>
        TTo Map<TTo>(object source);

        /// <summary>
        /// Register mapper object
        /// </summary>
        /// <param name="mapper">Mapper</param>
        void RegisterMapper(object mapper);

        /// <summary>
        /// Register mapper object
        /// </summary>
        /// <param name="from">From type</param>
        /// <param name="to">To type</param>
        /// <param name="mapper">Mapper</param>
        void RegisterMapper(Type from, Type to, Delegate mapper);

        /// <summary>
        /// Register mapper object
        /// </summary>
        /// <typeparam name="TFrom">From type</typeparam>
        /// <typeparam name="TTo">To type</typeparam>
        /// <param name="mapper">Mapper</param>
        void RegisterMapper<TFrom, TTo>(Func<TFrom, TTo> mapper);
    }
}