using SchoolSystem.Core.Teacher;
using AutoMapper;

namespace SchoolSystem.Tests.Core.Teacher
{
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
        private readonly IMapper _mapper;

        public TeacherMapperTest()
        {
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile<MappingProfile>();
                });

            _mapper = config.CreateMapper();
        }

        [TestCaseSource(typeof(TeacherTestData), nameof(TeacherTestData.TeacherModelTestCases))]
        public void Should_Map_Model_To_Dto(TeacherModel model)
        {
            var dto = _mapper.Map<TeacherDto>(model);

            Assert.Multiple(() =>
            {
                Assert.That(dto.Id, Is.EqualTo(model.Id));
                Assert.That(dto.FullName, Is.EqualTo(model.FullName));
                Assert.That(dto.Email, Is.EqualTo(model.Email));
            });
        }

        [TestCaseSource(typeof(TeacherTestData), nameof(TeacherTestData.TeacherDtoTestCases))]
        public void Should_Map_Dto_To_Model(TeacherDto dto)
        {
            var model = _mapper.Map<TeacherModel>(dto);

            Assert.Multiple(() =>
            {
                Assert.That(model.Id, Is.EqualTo(dto.Id));
                Assert.That(model.FullName, Is.EqualTo(dto.FullName));
                Assert.That(model.Email, Is.EqualTo(dto.Email));
            });
        }


        [TestCaseSource(typeof(TeacherTestData), nameof(TeacherTestData.TeacherModelAndRelatedDtoTestCases))]
        public void Should_Complete_RoundTrip(TeacherModel model, TeacherDto dto)
        {
            var mappedDto = _mapper.Map<TeacherDto>(model);
            var mappedModel = _mapper.Map<TeacherModel>(mappedDto);

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
}
