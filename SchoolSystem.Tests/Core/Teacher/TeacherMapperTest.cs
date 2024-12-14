using SchoolSystem.Core.Teacher;
using AutoMapper;
using SchoolSystem.Core.Common.BaseClasses;
using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Tests.Core.Teacher;


namespace SchoolSystem.Tests.Core.Teacher;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TeacherModel, TeacherDto>();
        CreateMap<TeacherDto, TeacherModel>();
    }
}

public class TeacherMapperTest
{
    private IBaseMapper<TeacherModel, TeacherDto> _teacherDtoMapper;
    private IBaseMapper<TeacherDto, TeacherModel> _teacherModelMapper;


    [SetUp]
    public void Setup()
    {
        var config = new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

        IMapper _mapper = config.CreateMapper();
        _teacherDtoMapper = new BaseMapper<TeacherModel, TeacherDto>(_mapper);
        _teacherModelMapper = new BaseMapper<TeacherDto, TeacherModel>(_mapper);
    }

    [TestCaseSource(typeof(TeacherTestData), nameof(TeacherTestData.TeacherModelTestCases))]
    public void Should_Map_Model_To_Dto(TeacherModel model)
    {
        var dto = _teacherDtoMapper.MapInstance(model);

        Assert.Multiple(() =>
        {
            Assert.That(dto.Id, Is.EqualTo(model.Id));
            Assert.That(dto.FullName, Is.EqualTo(model.FullName));
            Assert.That(dto.Position, Is.EqualTo(model.Position));
            Assert.That(dto.Email, Is.EqualTo(model.Email));
        });
    }

    [TestCaseSource(typeof(TeacherTestData), nameof(TeacherTestData.TeacherDtoTestCases))]
    public void Should_Map_Dto_To_Model(TeacherDto dto)
    {
        var model = _teacherModelMapper.MapInstance(dto);

        Assert.Multiple(() =>
        {
            Assert.That(model.Id, Is.EqualTo(dto.Id));
            Assert.That(model.FullName, Is.EqualTo(dto.FullName));
            Assert.That(model.Position, Is.EqualTo(dto.Position));
            Assert.That(model.Email, Is.EqualTo(dto.Email));
        });
    }


    [TestCaseSource(typeof(TeacherTestData), nameof(TeacherTestData.TeacherModelAndRelatedDtoTestCases))]
    public void Should_Complete_RoundTrip(TeacherModel model, TeacherDto dto)
    {
        var mappedDto = _teacherDtoMapper.MapInstance(model);
        var mappedModel = _teacherModelMapper.MapInstance(mappedDto);

        Assert.Multiple(() =>
        {
            Assert.That(mappedDto.Id, Is.EqualTo(dto.Id));
            Assert.That(mappedDto.FullName, Is.EqualTo(dto.FullName));
            Assert.That(mappedDto.Position, Is.EqualTo(dto.Position));
            Assert.That(mappedDto.Email, Is.EqualTo(dto.Email));
            Assert.That(mappedModel.Id, Is.EqualTo(model.Id));
            Assert.That(mappedModel.FullName, Is.EqualTo(model.FullName));
            Assert.That(mappedModel.Position, Is.EqualTo(model.Position));
            Assert.That(mappedModel.Email, Is.EqualTo(model.Email));
        });
    }
}
