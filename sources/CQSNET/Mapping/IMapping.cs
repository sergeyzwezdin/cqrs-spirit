namespace CQSNET.Mapping
{
    public interface IMapping<in TFrom, out TTo>
    {
        TTo Map(TFrom source);
    }
}