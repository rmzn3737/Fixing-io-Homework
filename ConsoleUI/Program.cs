// See https://aka.ms/new-console-template for more information

using Business.Concrete;
using DataAccsess.Concrete.EntityFramework;
using DataAccsess.Concrete.InMemory;

internal class Program
{
    public static void Main(string[] args)
    {
        //DTO= Data Transformation Object
        //CourseManager courseManager = new CourseManager(new InMemoryCourseDal());
        CourseTest();
        //CategoryTest();
    }

    private static void CategoryTest()
    {
        CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());//Şimdilik newliyoruz, ilerde IoC conteyner ile yapacağız.
        foreach (var category in categoryManager.GetAll().Data)
        {
            Console.WriteLine(category.CategoryName);
        }
    }

    private static void CourseTest()
    {
        CourseManager courseManager = new CourseManager(new EfCourseDal(),new CategoryManager(new EfCategoryDal()));

        var result = courseManager.GetCourseDetails();

        if (result.Success==true)//Seçtiğin kategoriye uygun kurs varsa ona göre bir ekran yapıyor.
        {

            //*****Dikkat*****
            //Sistem bakımda yazısını iptal edip ürünleri lsitelemek için CourseManager daki GetCourseDetails metodunun içindeki if bloğunu yorum satırı yapalım.
            foreach (var course in result.Data)
            {
                Console.WriteLine(course.CourseName + " ***/*** " + course.CategoryName);
            }
        }
        else//Seçtiğin kategoriye uygun kurs yoksa ona göre bir ekran yapıyor.
        {
            Console.WriteLine(result.Message);
        }

        
    }
}
