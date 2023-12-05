using BusinessLayer.Services.Interfaces;
using DataLayer.Repository.Interfaces;
using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
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
            _departmentRepo.Insert(department);
        }

        public void DeleteDepartment(int departmentId)
        {
            _departmentRepo.Delete(new Department() { DepartmentId = departmentId } );
        }

        public IEnumerable<Department> GetAllDepartments()
        {
            return _departmentRepo.GetMany();
        }

        public Department GetDepartment(int departmentId)
        {
            return _departmentRepo.GetByPK(departmentId);
        }

        public void UpdateDepartment(Department department)
        {
            _departmentRepo.Update(department);
        }
    }
}
