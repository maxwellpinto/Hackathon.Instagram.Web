using Hackathon.Instagram.Domain.EntityFramework.DataEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Instagram.Domain.EntityFramework.Mapping
{
    public class DataAccessInstagramMap : EntityTypeConfiguration<DataAccessInstagram>
    {
        public DataAccessInstagramMap()
        {
            this.HasKey(x => x.Id);
        }
    }
}
