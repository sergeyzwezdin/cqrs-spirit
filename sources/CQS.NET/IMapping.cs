namespace CQS.NET
{
	public interface IMapping<in TFrom, out TTo>
	{
		TTo Map(TFrom source);
	}
}