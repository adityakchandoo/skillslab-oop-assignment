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
        IEnumerable<TrainingDetails> GetAllTraining();
        IEnumerable<Training> GetTrainingEnrolledByUser(int UserId);
        int CreateTrainingReturningID(Training training);
        IEnumerable<UserTraining> GetUserTrainingByStatusAndManagerId(EnrollStatusEnum enrollStatusEnum, int UserId);
    }
}
