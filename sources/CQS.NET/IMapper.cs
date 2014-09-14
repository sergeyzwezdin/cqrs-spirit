using System;

namespace CQS.NET
{
	public interface IMapper
	{
		TTo Map<TFrom, TTo>(TFrom source);

		TTo Map<TTo>(object source);

		void RegisterMapper(object mapper);

		void RegisterMapper(Type from, Type to, Delegate mapper);

		void RegisterMapper<TFrom, TTo>(Func<TFrom, TTo> mapper);
	}
}