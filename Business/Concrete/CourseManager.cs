using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccsess.Abstract;
using DataAccsess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using ValidationException = FluentValidation.ValidationException;

namespace Business.Concrete
{
    public class CourseManager:ICourseService
    {
        ICourseDal _courseDal;
        private ICategoryService _categoryService;

        public CourseManager(ICourseDal courseDal, ICategoryService categoryService)
        {
            _courseDal = courseDal;
            _categoryService= categoryService;
        }

        public IDataResult<List<Course>> GetAll()
        {
            
            //İş Kodları
            //Yetkisi var mı ? İf kodları
            //Kural bir iş sınıfı başka sınıfları newlemez.
            if (DateTime.Now.Hour==16)//Yani sistem bu saatlerde bakımda.
            {
                return new ErrorDataResult<List<Course>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Course>>(_courseDal.GetAll(),Messages.CoursesListed) ;
            //throw new NotImplementedException();
        }

        public IDataResult<List<Course>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Course>>(_courseDal.GetAll(c => c.CategoryId == id));
        }

        public IDataResult<Course> GetByID(int courseId)
        {
            return new SuccessDataResult<Course>(_courseDal.Get(c => c.CourseId == courseId)) ;
        }

        public IDataResult<List<Course>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Course>> (_courseDal.GetAll(c => c.Price >= min && c.Price <= max));
        }

        public IDataResult<List<CourseDetailDto>>  GetCourseDetails()
        {
            if (DateTime.Now.Hour == 4)//Yani sistem bu saatlerde bakımda.Hoca yapmayın demişti!!!
            {
                return new ErrorDataResult<List<CourseDetailDto>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<CourseDetailDto>>(_courseDal.GetCourseDetails()) ;
        }
        //Claim= İddia etmek. Encryption, Hashing, karşı taraf okuyamasın diye parolayı hasleriz.Salting, tuzlama. Encryption, geri döünüşü olan veri.
        [SecuredOperation("course.add,admin")]
        [ValidationAspect(typeof(CourseValidator))]
        public IResult Add(Course course)
        {
            ////Business, yani iş kuralları buraya yazılacak, yani ürünü eklemeden önce yapılacak kontroller.

            //if (course.CourseName.Length<2==true)//Biz burada bu şekilde yaptık ancak sektörde try catch bloğuyla da yapan kurumlar var, her iki yaklaşım da kullanılıyor.
            //{
            //    return new ErrorResult(Messages.CourseNameInValid);//Kurs ismi en az 2 karakter olmalıdır."İleride stringi burada yamayacağız, magic strin demek oluyor bu.
            //}

            //validation
            //Cross Cutting Conserns
            //Log, cache, transaction, authorization, …  bunları araştıralım.
            //var context = new ValidationContext<Course>(course);//**Bu yöntem doğrulamanın en spagetti code yöntemi.
            //CourseValidator courseValidator = new CourseValidator();
            //var result = courseValidator.Validate(context);
            //if (!result.IsValid)
            //{
            //    throw new ValidationException(result.Errors);
            //}
            ////Loglama
            //Cacheremowe
            //Performance
            //Trnasaction
            //Yetkilendirme

            //ValidationTool.Validate(new CourseValidator(),course);
            //Bir kategoride en fazla 10 ürün olabilir.
            //Aynı isimde ürün eklenemez.
            //Eğer kategori sayısı 15'i geçtiyse sisteme yeni ürün eklenemez.
            //Alttaki IResult result kurala uymayan result.
            IResult result = BusinessRules.Run(CheckIfCourseCountOfCategoryCorrect(course.CategoryId),
                CheckIfCourseCountOfCategoryCorrect(course.CategoryId),CheckIfCategoryLimitExceded());//Daha sonra yeni bir kural gelirse buraya , ile eklemek yeterli.

            if (result != null)//Kurala uymayan bir durum oluşmuşsa.
            {
                return result;
            }
            _courseDal.Add(course);
            return new SuccessResult(Messages.CourseAdded);

            //if (CheckIfCourseCountOfCategoryCorrect(course.CategoryId).Success)
            //{
            //    if (CheckIfCourseNameExists(course.CourseName).Success)
            //    {
                    
            //    }
                
            //}
            //return new ErrorResult();

        }

        public IResult Update(Course course)
        {
            var result = _courseDal.GetAll(c => c.CategoryId == course.CategoryId).Count;
            if (result >= 10)
            {
                
                return new ErrorResult(Messages.CourseCountOfCategoryError);
            }
            _courseDal.Add(course);
            return new SuccessResult(Messages.CourseAdded);
        }

        private IResult CheckIfCourseCountOfCategoryCorrect(int categoryId)
        {
            //select count(*) from courses where categoryId=1; alttaki koda arka planda bunu yapıyor.
            var result = _courseDal.GetAll(c => c.CategoryId == categoryId).Count;
            if (result >= 15)
            {
                return new ErrorResult(Messages.CourseCountOfCategoryError);
            }

            return new SuccessResult();
        }

        private IResult CheckIfCourseNameExists(string courseName)
        {
            //select count(*) from courses where categoryId=1; alttaki koda arka planda bunu yapıyor.
            var result = _courseDal.GetAll(c => c.CourseName == courseName).Any();//Any() var mı demek.
            if (result)
            {

                return new ErrorResult(Messages.CourseNameAlreadyExist);//Böyle bir ismi zaten var demek.
            }

            return new SuccessResult();
        }

        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count>15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }

            return new SuccessResult();
        }
    } 

    
}
