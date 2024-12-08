namespace SchoolSystem.Core.Base.BaseInterfaces
{
    public interface IBaseMapper<TSource, TDestination>
    {
        TDestination MapInstance(TSource source);
        IEnumerable<TDestination> MapList(IEnumerable<TSource> source);
    }
}