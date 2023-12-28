using Entities.DbCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Other
{
    public static class CsvConvert
    {
        public static string Convert(IEnumerable<TrainingEmployeeDetails> trainingEmployeeDetails)
        {
            StringBuilder csv = new StringBuilder();

            // Add header
            csv.AppendLine("EmployeeName,ContactEmail,ContactNumber,ManagerName");

            // Add object data
            foreach (var obj in trainingEmployeeDetails)
            {
                csv.AppendLine($"{obj.EmployeeName},{obj.ContactEmail},{obj.ContactNumber},{obj.ManagerName}");
                // Handle other properties and special cases (e.g., commas in data)
            }

            return csv.ToString();
        }
    }
}
