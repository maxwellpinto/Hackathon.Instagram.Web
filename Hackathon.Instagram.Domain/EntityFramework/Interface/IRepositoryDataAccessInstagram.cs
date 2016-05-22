using Hackathon.Instagram.Domain.EntityFramework.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Instagram.Domain.EntityFramework.Interface
{
    public interface IRepositoryDataAccessInstagram
    {
        DataAccessInstagram GetByCode(string code);
        Boolean Save(DataAccessInstagram data);
        Boolean Update(DataAccessInstagram data);
        DataAccessInstagram GetIsValid();
    }
}
