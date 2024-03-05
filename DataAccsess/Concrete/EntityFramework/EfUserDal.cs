using Core.DataAccsess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DataAccsess.Concrete.EntityFramework;
using Microsoft.Extensions.Logging;
namespace DataAccess.Concrete.EntityFramework
{


    

    public class EfUserDal : EfEntityRepositoryBase<User, CourseContext>, IUserDal
    {
        //private readonly ILogger<EfUserDal> _logger;

        //public EfUserDal(ILogger<EfUserDal> logger)
        //{
        //    _logger = logger;
        //}

        public List<OperationClaim> GetClaims(User user)
        {
            //_logger.LogInformation("GetClaims metodu çağrıldı. Kullanıcı Id: {UserId}", user.Id);

            using (var context = new CourseContext())
            {
                var result = from operationClaim in context.OperationClaims
                    join userOperationClaim in context.UserOperationClaims
                        on operationClaim.Id equals userOperationClaim.OperationClaimId
                    where userOperationClaim.UserId == user.Id
                    select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };

                //_logger.LogInformation("GetClaims metodu sona erdi. Toplam {ClaimCount} talep alındı.", result.Count());

                return result.ToList();
            }
        }
    }




    //public class EfUserDal : EfEntityRepositoryBase<User, CourseContext>, IUserDal
    //{
    //    public List<OperationClaim> GetClaims(User user)
    //    {
    //        using (var context = new CourseContext())
    //        {
    //            var result = from operationClaim in context.OperationClaims
    //                         join userOperationClaim in context.UserOperationClaims
    //                             on operationClaim.Id equals userOperationClaim.OperationClaimId
    //                         where userOperationClaim.UserId == user.Id
    //                         select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };
    //            Console.WriteLine(result.ToList());
    //            return result.ToList();

    //        }
    //    }
    //}
}
