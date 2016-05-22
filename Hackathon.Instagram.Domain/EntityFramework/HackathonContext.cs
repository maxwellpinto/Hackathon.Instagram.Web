using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Instagram.Domain.EntityFramework
{
    public class HackathonContext : DbContext
    {
        public HackathonContext()
            :base(GetConnectionStringName())
        {

            //To ensure that  dll EntityFramework.SqlProviderServices.dll  will be added in the web projetc /bin folder
            //está sendo copiada para a pasta bin do projeto Web          
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;

            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
        }

        private static String GetConnectionStringName()
        {
            string conn = ConfigurationManager.ConnectionStrings["DafaultConnection"].ConnectionString;

            if (string.IsNullOrEmpty(conn))
                throw new System.ApplicationException(@"We not found this ConnectionString");

            return conn;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
