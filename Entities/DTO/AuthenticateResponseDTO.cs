using Entities.DbCustom;
using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class AuthenticateResponse
    {
        public bool IsLoginSuccessful { get; set; }
        public string Error { get; set; }
        public AppUser AppUser { get; set; }
    }
}
