using FluentAssertions;
using AutoMapper;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Common.BaseClasses;
using SchoolSystem.Core.Course;

namespace SchoolSystem.Tests.Core.Teacher;


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

public class TeacherMapperTest
{
    private IBaseMapper<TeacherModel, TeacherDto> _courseDtoMapper;
    private IBaseMapper<TeacherDto, TeacherModel> _courseModelMapper;

    public TeacherMapperTest()
    {
        var config = new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

        IMapper _mapper = config.CreateMapper();
        _courseDtoMapper = new BaseMapper<TeacherModel, TeacherDto>(_mapper);
        _courseModelMapper = new BaseMapper<TeacherDto, TeacherModel>(_mapper);
    }

    [Theory]
    [MemberData(nameof(TeacherTestData.TeacherModelTestCases), MemberType = typeof(TeacherTestData))]
    public void Should_Map_Model_To_Dto(TeacherModel model)
    {
        var dto = _courseDtoMapper.MapInstance(model);

        dto.Id.Should().Be(model.Id);
        dto.FullName.Should().Be(model.FullName);
        dto.Email.Should().Be(model.Email);
    }

    [Theory]
    [MemberData(nameof(TeacherTestData.TeacherDtoTestCases), MemberType = typeof(TeacherTestData))]
    public void Should_Map_Dto_To_Model(TeacherDto dto)
    {
        var model = _courseModelMapper.MapInstance(dto);

        model.Id.Should().Be(dto.Id);
        model.FullName.Should().Be(dto.FullName);
        model.Email.Should().Be(dto.Email);
    }

    [Theory]
    [MemberData(nameof(TeacherTestData.TeacherModelAndRelatedDtoTestCases), MemberType = typeof(TeacherTestData))]
    public void Should_Complete_RoundTrip(TeacherModel model, TeacherDto dto)
    {
        var mappedDto = _courseDtoMapper.MapInstance(model);
        var mappedModel = _courseModelMapper.MapInstance(mappedDto);

        mappedDto.Id.Should().Be(dto.Id);
        mappedDto.FullName.Should().Be(dto.FullName);
        mappedDto.Email.Should().Be(dto.Email);
        mappedModel.Id.Should().Be(model.Id);
        mappedModel.FullName.Should().Be(model.FullName);
        mappedModel.Email.Should().Be(model.Email);
    }
}
