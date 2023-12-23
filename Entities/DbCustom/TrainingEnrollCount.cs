using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbCustom
{
    public class TrainingEnrollCount : Training
    {
        public string DepartmentName { get; set; }
        public int NumberOfEmployeesEnrolled { get; set; }
    }

}
