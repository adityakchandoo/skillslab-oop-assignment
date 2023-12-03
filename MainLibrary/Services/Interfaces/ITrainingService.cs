using MainLibrary.DTO;
using MainLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Services.Interfaces
{
    public interface ITrainingService
    {
        IEnumerable<TrainingDetails> GetAllTraining();
        Training GetTraining(int id);
        void AddTraining(Training training);
        void AddTrainingAndTrainingPrerequisite(TrainingDTO training);
        void EditTraining(Training training);
        void DeleteTraining(int id);
    }
}
