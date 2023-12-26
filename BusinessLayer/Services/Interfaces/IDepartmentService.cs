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

        Task<Department> GetDepartmentAsync(int departmentId);
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        Task AddDepartmentAsync(Department department);
        Task DeleteDepartmentAsync(int departmentId);
        Task UpdateDepartmentAsync(Department department);
    }
}
