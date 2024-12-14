using SchoolSystem.Core.Student;
using AutoMapper;
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

[TestFixture]
public class StudentMapperTest
{
    private IBaseMapper<StudentModel, StudentDto> _studentDtoMapper;
    private IBaseMapper<StudentDto, StudentModel> _studentModelMapper;


    [SetUp]
    public void Setup()
    {
        var config = new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

        IMapper _mapper = config.CreateMapper();
        _studentDtoMapper = new BaseMapper<StudentModel, StudentDto>(_mapper);
        _studentModelMapper = new BaseMapper<StudentDto, StudentModel>(_mapper);
    }

    [TestCaseSource(typeof(StudentTestData), nameof(StudentTestData.StudentModelTestCases))]
    public void Should_Map_Model_To_Dto(StudentModel model)
    {
        var dto = _studentDtoMapper.MapInstance(model);

        Assert.Multiple(() =>
        {
            Assert.That(dto.Id, Is.EqualTo(model.Id));
            Assert.That(dto.FullName, Is.EqualTo(model.FullName));
            Assert.That(dto.Email, Is.EqualTo(model.Email));
        });
    }

    [TestCaseSource(typeof(StudentTestData), nameof(StudentTestData.StudentDtoTestCases))]
    public void Should_Map_Dto_To_Model(StudentDto dto)
    {
        var model = _studentModelMapper.MapInstance(dto);

        Assert.Multiple(() =>
        {
            Assert.That(model.Id, Is.EqualTo(dto.Id));
            Assert.That(model.FullName, Is.EqualTo(dto.FullName));
            Assert.That(model.Email, Is.EqualTo(dto.Email));
        });
    }


    [TestCaseSource(typeof(StudentTestData), nameof(StudentTestData.StudentModelAndRelatedDtoTestCases))]
    public void Should_Complete_RoundTrip(StudentModel model, StudentDto dto)
    {
        var mappedDto = _studentDtoMapper.MapInstance(model);
        var mappedModel = _studentModelMapper.MapInstance(mappedDto);

        Assert.Multiple(() =>
        {
            Assert.That(mappedDto.Id, Is.EqualTo(dto.Id));
            Assert.That(mappedDto.FullName, Is.EqualTo(dto.FullName));
            Assert.That(mappedDto.Email, Is.EqualTo(dto.Email));
            Assert.That(mappedModel.Id, Is.EqualTo(model.Id));
            Assert.That(mappedModel.FullName, Is.EqualTo(model.FullName));
            Assert.That(mappedModel.Email, Is.EqualTo(model.Email));
        });
    }
}
