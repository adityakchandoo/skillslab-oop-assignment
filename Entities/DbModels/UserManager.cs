using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    public class UserManager
    {
        [Key]
        public int UserManagerId { get; set; }
        public int UserId { get; set; }
        public int ManagerId { get; set; }

    }
}
