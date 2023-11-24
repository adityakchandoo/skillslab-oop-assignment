using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MainLibrary.DTO
{
    public class RegEnrollFormDTO : RegisterFormDTO
    {
        public int TargetTraining { get; set; }
        public HttpPostedFileBase[] files { get; set; }
    }
}
