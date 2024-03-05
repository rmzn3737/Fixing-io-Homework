using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public static class Messages//newlememek için static verdik. Böylece uygulama hayatı boyunca tek intanceı oluyor.
    {
        public static string CourseAdded = "Kurs eklendi.";//Bunun diğer diller için de olan vesiyonları için DevArtchicakter ı inceleyeceğiz.
        public static string CourseNameInValid = "Kurs ismi geçersiz.";//CourseInValid bunun gibi public fieldlar pascal casing yazılır.
        public static string MaintenanceTime ="Sistem bakımda.";
        public static string CoursesListed ="Ürünler listelendi.";
        public static string CourseCountOfCategoryError="Bir kategoride en fazla 10 ürün olabilir.";
        public static string CourseNameAlreadyExist="Bu isimde zaten başka bir ürün var.";
        public static string CategoryLimitExceded="Kategori limiti aşıldığı için kurs eklenemedi.";
        public static string AuthorizationDenied = "Yetkiniz yok.";
    }
}
