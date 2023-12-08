using DataLayer.Generic;
using Entities.DbCustom;
using Entities.DbModels;
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
        IEnumerable<Training> GetTrainingEnrolledByUser(string UserId);
        IEnumerable<Training> GetUsersManagedBy(string UserId);
        int CreateTrainingReturningID(Training training);

    }
}
