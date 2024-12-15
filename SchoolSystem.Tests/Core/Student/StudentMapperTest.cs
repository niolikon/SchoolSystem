using FluentAssertions;
using AutoMapper;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Common.BaseClasses;

namespace SchoolSystem.Tests.Core.Student;


public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<StudentModel, StudentDto>();
        CreateMap<StudentDto, StudentModel>();
    }
}

public class StudentMapperTest
{
    private IBaseMapper<StudentModel, StudentDto> _courseDtoMapper;
    private IBaseMapper<StudentDto, StudentModel> _courseModelMapper;

    public StudentMapperTest()
    {
        var config = new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

        IMapper _mapper = config.CreateMapper();
        _courseDtoMapper = new BaseMapper<StudentModel, StudentDto>(_mapper);
        _courseModelMapper = new BaseMapper<StudentDto, StudentModel>(_mapper);
    }

    [Theory]
    [MemberData(nameof(StudentTestData.StudentModelTestCases), MemberType = typeof(StudentTestData))]
    public void Should_Map_Model_To_Dto(StudentModel model)
    {
        var dto = _courseDtoMapper.MapInstance(model);

        dto.Id.Should().Be(model.Id);
        dto.FullName.Should().Be(model.FullName);
        dto.Email.Should().Be(model.Email);
    }

    [Theory]
    [MemberData(nameof(StudentTestData.StudentDtoTestCases), MemberType = typeof(StudentTestData))]
    public void Should_Map_Dto_To_Model(StudentDto dto)
    {
        var model = _courseModelMapper.MapInstance(dto);

        model.Id.Should().Be(dto.Id);
        model.FullName.Should().Be(dto.FullName);
        model.Email.Should().Be(dto.Email);
    }

    [Theory]
    [MemberData(nameof(StudentTestData.StudentModelAndRelatedDtoTestCases), MemberType = typeof(StudentTestData))]
    public void Should_Complete_RoundTrip(StudentModel model, StudentDto dto)
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
