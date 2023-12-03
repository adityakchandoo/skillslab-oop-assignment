using MainLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Repo.Interfaces
{
    public interface IDepartmentRepo
    {
        Department GetDepartment(int departmentId);
        IEnumerable<Department> GetAllDepartments();
        void CreateDepartment(Department department);
        void DeleteDepartment(int departmentId);
        void UpdateDepartment(Department department);
    }
}
