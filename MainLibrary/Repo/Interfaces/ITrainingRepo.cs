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
        IEnumerable<Training> GetTrainingManagedByUser(string UserId);
        IEnumerable<Training> GetTrainingEnrolledByUser(string UserId);
        Training GetTraining(int id);
        void CreateTraining(Training training);
        void UpdateTraining(Training training);
        void DeleteTraining(int id);
    }
}
