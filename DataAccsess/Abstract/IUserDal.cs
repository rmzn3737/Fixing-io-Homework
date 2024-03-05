using Core.DataAccsess;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using Core.DataAccsess;

namespace DataAccess.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {
        List<OperationClaim> GetClaims(User user);
    }
}
