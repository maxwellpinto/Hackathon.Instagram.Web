using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Instagram.Domain.EntityFramework.DataEntities
{    
    public class DataAccessInstagram 
    {
        public Guid Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime? DueDate { get; set; }
        public Boolean IsValid { get; set; }
        public Int64 IdUser { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ProfilePicture { get; set; }
        public string AccessToken { get; set; }
    }
}
