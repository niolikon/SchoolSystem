using AutoMapper;
using SchoolSystem.Core.Common.BaseInterfaces;

namespace SchoolSystem.Core.Common.BaseClasses;

public class BaseMapper<TSource, TDestination>(IMapper mapper) : IBaseMapper<TSource, TDestination>
{
    private readonly IMapper _mapper = mapper;

    public TDestination MapInstance(TSource source)
    {
        return _mapper.Map<TDestination>(source);
    }

    public IEnumerable<TDestination> MapList(IEnumerable<TSource> source)
    {
        return _mapper.Map<IEnumerable<TDestination>>(source);
    }
}