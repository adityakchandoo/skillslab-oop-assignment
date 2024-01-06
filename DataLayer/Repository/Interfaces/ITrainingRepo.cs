using DataLayer.Generic;
using Entities.DbCustom;
using Entities.DbModels;
using Entities.DTO;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Interfaces
{
    public interface ITrainingRepo : IDataAccessLayer<Training>
    {
        Task CreateTrainingWithPrerequisite(Training training, DataTable prerequisites);
        Task<IEnumerable<TrainingEnrollCount>> GetAllTrainingWithEnrollCountAsync();

        Task<TrainingWithUserStatusPG> GetAllTrainingAsync(int UserId, int pageNumber);
        Task<TrainingWithUserStatusPG> GetTrainingEnrolledByUserAsync(int UserId, EnrollStatusEnum status, int pageNumber);
        Task<TrainingWithUserStatusPG> GetTrainingEnrolledByUserAsync(int UserId, int pageNumber);
        Task<IEnumerable<UserTraining>> GetUserTrainingByStatusAndManagerIdAsync(EnrollStatusEnum enrollStatusEnum, int UserId);
        Task<IEnumerable<TrainingEmployeeDetails>> GetAllTrainingEmployeeDetailsByTrainingId(int trainingId);
        Task SoftDeleteTrainingAsync(int trainingId);
        Task AutoProcess();
    }
}
