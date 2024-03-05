using Business.Abstract;
using Business.Concrete;
using Core.Utilities.Results;
using DataAccsess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]//C# da bunun adı Attribute, Java da ise Annatotation
    public class CoursesController : ControllerBase//Api isimlendirmelerinde Courses şeklinde çoğul verilir, onun için böyle yazdık.
    {
        //Loose coupled = Gevşek bağlılık, yani bir bağımlılığı var ama soyuta bağımlı, yani servis değişirse sadece managerımızı değiştirirsek herhangi bir problemle karşılaşmayız.
        //fieldlara _courseService bu şekilde alt çizgili isim vermenin andı naming convention.
        //IoC container kullanacağız.----Inversion of control ---> Değişimin kontrolü---Bu bir konfigurasyon.
        private readonly ICourseService _courseService;//field oluşturduk.

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("getall")]//("getall") Alyans verme deniliyor.Bunun alternatifi routing yöntemi var ödevlerden araştıralım.
        public IActionResult GetAll()//public List<Course> Get()
        {
            //Dependency chain ---> bağımlılık zinciri var burada.
            //ICourseService courseService = new CourseManager(new EfCourseDal());
            var result = _courseService.GetAll();

            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _courseService.GetByID(id);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult Add(Course course)
        {
            var result = _courseService.Add(course);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}

//new Course
// {
//     CategoryId = 1, CourseId = 25, CourseName = "WEB APİ Course",
//     CourseDescription = "APİ leri Öğreniyoruz.", Price = 0
// },
// new Course
// {
//     CategoryId = 2, CourseId = 26, CourseName = "WEB APİ Course 2",
//     CourseDescription = "APİ leri Öğreniyoruz. 2", Price = 2
// },
// 
