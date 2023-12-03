using MainLibrary.Entities;
using MainLibrary.Repo.Interfaces;
using MainLibrary.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepo _departmentRepo;
        public DepartmentService(IDepartmentRepo departmentRepo)
        {
            _departmentRepo = departmentRepo;            
        }

        public void AddDepartment(Department department)
        {
            _departmentRepo.CreateDepartment(department);
        }

        public void DeleteDepartment(int departmentId)
        {
            _departmentRepo.DeleteDepartment(departmentId);
        }

        public IEnumerable<Department> GetAllDepartments()
        {
            return _departmentRepo.GetAllDepartments();
        }

        public Department GetDepartment(int departmentId)
        {
            return _departmentRepo.GetDepartment(departmentId);
        }

        public void UpdateDepartment(Department department)
        {
            _departmentRepo.UpdateDepartment(department);
        }
    }
}
