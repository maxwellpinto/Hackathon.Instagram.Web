using Hackathon.Instagram.Domain.EntityFramework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hackathon.Instagram.Domain.EntityFramework.DataEntities;

namespace Hackathon.Instagram.Domain.EntityFramework.Repository
{
    public class RepositoryDataAccessInstagram : IRepositoryDataAccessInstagram
    {
        public DataAccessInstagram GetByCode(string code)
        {
            using (var db = new HackathonContext())
            {
                return db.Set<DataAccessInstagram>()
                    .Where(x => x.AccessToken == code).FirstOrDefault();
            }
        }

        public DataAccessInstagram GetIsValid()
        {
            using (var db = new HackathonContext())
            {
                return db.Set<DataAccessInstagram>()
                    .Where(x => !x.DueDate.HasValue && x.IsValid ).OrderByDescending( x => x.RegistrationDate)
                    .FirstOrDefault();
            }
        }

        public bool Save(DataAccessInstagram data)
        {
            try
            {
                using (var db = new HackathonContext())
                {
                    if(data.Id == Guid.Empty)
                    {
                        data.Id = Guid.NewGuid();
                        data.DueDate = null;
                        data.IsValid = true;
                        data.RegistrationDate = DateTime.Now;
                    }

                    db.Set<DataAccessInstagram>().Add(data);
                    db.SaveChanges();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool Update(DataAccessInstagram data)
        {
            try
            {
                using (var db = new HackathonContext())
                {
                    var table = db.Set<DataAccessInstagram>();

                    table.Attach(data);
                    db.Entry<DataAccessInstagram>(data).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
