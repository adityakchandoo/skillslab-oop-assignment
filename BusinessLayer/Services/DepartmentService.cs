using BusinessLayer.Services.Interfaces;
using DataLayer.Repository.Interfaces;
using Entities.DbModels;
using System.Collections.Generic;
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

        public async Task AddDepartmentAsync(Department department)
        {
            await _departmentRepo.Insert(department);
        }

        public async Task DeleteDepartmentAsync(int departmentId)
        {
            await _departmentRepo.Delete(new Department() { DepartmentId = departmentId });
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            return await _departmentRepo.GetMany();
        }

        public async Task<Department> GetDepartmentAsync(int departmentId)
        {
            return await _departmentRepo.GetByPKAsync(departmentId);
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            await _departmentRepo.Update(department);
        }
    }
}
