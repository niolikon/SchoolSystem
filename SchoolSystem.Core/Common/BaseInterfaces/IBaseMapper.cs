namespace SchoolSystem.Core.Common.BaseInterfaces;

public interface IBaseMapper<TSource, TDestination>
{
    TDestination MapInstance(TSource source);

    IEnumerable<TDestination> MapList(IEnumerable<TSource> source);
}