using SchoolSystem.Core.Student;
using AutoMapper;


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
    private readonly IMapper _mapper;

    public StudentMapperTest()
    {
        var config = new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

        _mapper = config.CreateMapper();
    }

    [TestCaseSource(typeof(StudentTestData), nameof(StudentTestData.StudentModelTestCases))]
    public void Should_Map_Model_To_Dto(StudentModel model)
    {
        var dto = _mapper.Map<StudentDto>(model);

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
        var model = _mapper.Map<StudentModel>(dto);

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
        var mappedDto = _mapper.Map<StudentDto>(model);
        var mappedModel = _mapper.Map<StudentModel>(mappedDto);

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
