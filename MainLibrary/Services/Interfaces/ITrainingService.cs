using MainLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Service.Interfaces
{
    public interface ITrainingService
    {
        IEnumerable<TrainingDetails> GetAllTraining();
        Training GetTraining(int id);
        void AddTraining(Training training);
        void EditTraining(Training training);
        void DeleteTraining(Training training);
    }
}
