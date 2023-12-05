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
        IEnumerable<TrainingDetails> GetTrainingManagedByUser(string UserId);
        IEnumerable<TrainingDetails> GetTrainingEnrolledByUser(string UserId);

    }
}
