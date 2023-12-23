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
        int CreateTrainingReturningID(Training training);
        IEnumerable<TrainingStatus> GetAllTraining(int UserId);
        IEnumerable<TrainingEnrollCount> GetAllTrainingWithEnrollCount();
        IEnumerable<TrainingStatus> GetTrainingEnrolledByUser(int UserId, EnrollStatusEnum status);
        IEnumerable<TrainingStatus> GetTrainingEnrolledByUser(int UserId);
        IEnumerable<UserTraining> GetUserTrainingByStatusAndManagerId(EnrollStatusEnum enrollStatusEnum, int UserId);
    }
}
