using Entities.DbCustom;
using Entities.DbModels;
using Entities.DTO;
using Entities.FormDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces
{
    public interface ITrainingService
    {
        IEnumerable<TrainingDetails> GetAllTraining();
        Training GetTraining(int id);
        IEnumerable<TrainingDetails> GetTrainingEnrolledByUser(string UserId);
        IEnumerable<TrainingDetails> GetTrainingManagedByUser(string UserId);
        void AddTraining(Training training);
        void AddTrainingAndTrainingPrerequisite(TrainingDTO training);
        void EditTraining(Training training);
        void DeleteTraining(int id);
    }
}
