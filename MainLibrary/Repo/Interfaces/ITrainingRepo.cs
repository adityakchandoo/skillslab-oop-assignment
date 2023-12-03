using MainLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Repo.Interfaces
{
    public interface ITrainingRepo
    {
        IEnumerable<TrainingDetails> GetAllTraining();
        IEnumerable<TrainingDetails> GetTrainingManagedByUser(string UserId);
        IEnumerable<TrainingDetails> GetTrainingEnrolledByUser(string UserId);
        Training GetTraining(int id);
        int CreateTraining(Training training);
        void UpdateTraining(Training training);
        void DeleteTraining(int id);
    }
}
