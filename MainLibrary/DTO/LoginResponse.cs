using MainLibrary.Entities;
using MainLibrary.Entities.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.DTO
{
    public class AuthenticateResponse
    {
        public bool IsLoginSuccessful;
        public string RedirectPath { get; set; }
        public User user { get; set; }
    }
}
