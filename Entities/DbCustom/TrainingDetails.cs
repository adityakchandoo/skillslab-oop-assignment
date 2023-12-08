using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbCustom
{
    public class TrainingDetails
    {
        public Training Training { get; set; }
        public TrainingExtra TrainingExtra { get; set; }
    }

    public class TrainingExtra
    {
        public int TrainingId { get; set; }
        public string DepartmentName { get; set; }
        public int NumberOfEmployeesEnrolled { get; set; }
    }
}
