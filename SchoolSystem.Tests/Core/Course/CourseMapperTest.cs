using SchoolSystem.Core.Course;
using AutoMapper;

namespace SchoolSystem.Tests.Core.Course
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CourseModel, CourseDto>();
            CreateMap<CourseDto, CourseModel>();
        }
    }

    public class CourseMapperTest
    {
        private readonly IMapper _mapper;

        public CourseMapperTest()
        {
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile<MappingProfile>();
                });

            _mapper = config.CreateMapper();
        }

        [TestCaseSource(typeof(CourseTestData), nameof(CourseTestData.CourseModelTestCases))]
        public void Should_Map_Model_To_Dto(CourseModel model)
        {
            var dto = _mapper.Map<CourseDto>(model);

            Assert.Multiple(() =>
            {
                Assert.That(dto.Id, Is.EqualTo(model.Id));
                Assert.That(dto.Name, Is.EqualTo(model.Name));
                Assert.That(dto.Credits, Is.EqualTo(model.Credits));
                Assert.That(dto.TeacherId, Is.EqualTo(model.TeacherId));
            });
        }

        [TestCaseSource(typeof(CourseTestData), nameof(CourseTestData.CourseDtoTestCases))]
        public void Should_Map_Dto_To_Model(CourseDto dto)
        {
            var model = _mapper.Map<CourseModel>(dto);

            Assert.Multiple(() =>
            {
                Assert.That(model.Id, Is.EqualTo(dto.Id));
                Assert.That(model.Name, Is.EqualTo(dto.Name));
                Assert.That(model.Credits, Is.EqualTo(dto.Credits));
                Assert.That(model.TeacherId, Is.EqualTo(dto.TeacherId));
            });
        }


        [TestCaseSource(typeof(CourseTestData), nameof(CourseTestData.CourseModelAndRelatedDtoTestCases))]
        public void Should_Complete_RoundTrip(CourseModel model, CourseDto dto)
        {
            var mappedDto = _mapper.Map<CourseDto>(model);
            var mappedModel = _mapper.Map<CourseModel>(mappedDto);

            Assert.Multiple(() =>
            {
                Assert.That(mappedDto.Id, Is.EqualTo(dto.Id));
                Assert.That(mappedDto.Name, Is.EqualTo(dto.Name));
                Assert.That(mappedDto.Credits, Is.EqualTo(dto.Credits));
                Assert.That(mappedDto.TeacherId, Is.EqualTo(dto.TeacherId));
                Assert.That(mappedModel.Id, Is.EqualTo(model.Id));
                Assert.That(mappedModel.Name, Is.EqualTo(model.Name));
                Assert.That(mappedModel.Credits, Is.EqualTo(model.Credits));
                Assert.That(mappedModel.TeacherId, Is.EqualTo(model.TeacherId));
            });
        }
    }
}
