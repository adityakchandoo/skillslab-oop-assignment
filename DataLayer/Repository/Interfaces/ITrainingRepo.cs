using DataLayer.Generic;
using Entities.DbCustom;
using Entities.DbModels;
using Entities.DTO;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Interfaces
{
    public interface ITrainingRepo : IDataAccessLayer<Training>
    {
        Task<int> CreateTrainingReturningIDAsync(Training training);
        Task<IEnumerable<TrainingStatus>> GetAllTrainingAsync(int UserId);
        Task<IEnumerable<TrainingEnrollCount>> GetAllTrainingWithEnrollCountAsync();
        Task<IEnumerable<TrainingStatus>> GetTrainingEnrolledByUserAsync(int UserId, EnrollStatusEnum status);
        Task<IEnumerable<TrainingStatus>> GetTrainingEnrolledByUserAsync(int UserId);
        Task<IEnumerable<UserTraining>> GetUserTrainingByStatusAndManagerIdAsync(EnrollStatusEnum enrollStatusEnum, int UserId);
        Task<IEnumerable<TrainingEmployeeDetails>> GetAllTrainingEmployeeDetailsByTrainingId(int trainingId);
    }
}
