using FluentAssertions;
using AutoMapper;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Common.BaseClasses;
using SchoolSystem.Core.Teacher;

namespace SchoolSystem.Tests.Core.Course;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CourseModel, CourseDto>();
        CreateMap<CourseDto, CourseModel>();
        CreateMap<TeacherModel, TeacherDto>();
        CreateMap<TeacherDto, TeacherModel>();
    }
}

public class CourseMapperTest
{
    private IBaseMapper<CourseModel, CourseDto> _courseDtoMapper;
    private IBaseMapper<CourseDto, CourseModel> _courseModelMapper;

    public CourseMapperTest()
    {
        var config = new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

        IMapper _mapper = config.CreateMapper();
        _courseDtoMapper = new BaseMapper<CourseModel, CourseDto>(_mapper);
        _courseModelMapper = new BaseMapper<CourseDto, CourseModel>(_mapper);
    }

    [Theory]
    [MemberData(nameof(CourseTestData.CourseModelTestCases), MemberType = typeof(CourseTestData))]
    public void Should_Map_Model_To_Dto(CourseModel model)
    {
        var dto = _courseDtoMapper.MapInstance(model);

        dto.Id.Should().Be(model.Id);
        dto.Name.Should().Be(model.Name);
        dto.Credits.Should().Be(model.Credits);
        dto.TeacherId.Should().Be(model.TeacherId);
    }

    [Theory]
    [MemberData(nameof(CourseTestData.CourseDtoTestCases), MemberType = typeof(CourseTestData))]
    public void Should_Map_Dto_To_Model(CourseDto dto)
    {
        var model = _courseModelMapper.MapInstance(dto);

        model.Id.Should().Be(dto.Id);
        model.Name.Should().Be(dto.Name);
        model.Credits.Should().Be(dto.Credits);
        model.TeacherId.Should().Be(dto.TeacherId);
    }

    [Theory]
    [MemberData(nameof(CourseTestData.CourseModelAndRelatedDtoTestCases), MemberType = typeof(CourseTestData))]
    public void Should_Complete_RoundTrip(CourseModel model, CourseDto dto)
    {
        var mappedDto = _courseDtoMapper.MapInstance(model);
        var mappedModel = _courseModelMapper.MapInstance(mappedDto);

        mappedDto.Id.Should().Be(dto.Id);
        mappedDto.Name.Should().Be(dto.Name);
        mappedDto.Credits.Should().Be(dto.Credits);
        mappedDto.TeacherId.Should().Be(dto.TeacherId);
        mappedModel.Id.Should().Be(model.Id);
        mappedModel.Name.Should().Be(model.Name);
        mappedModel.Credits.Should().Be(model.Credits);
        mappedModel.TeacherId.Should().Be(model.TeacherId);
    }
}
