using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces
{
    public interface IDepartmentService
    {

        Department GetDepartment(int departmentId);
        IEnumerable<Department> GetAllDepartments();
        void AddDepartment(Department department);
        void DeleteDepartment(int departmentId);
        void UpdateDepartment(Department department);
    }
}
