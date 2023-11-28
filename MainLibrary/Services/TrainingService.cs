using MainLibrary.Entities;
using MainLibrary.Repo.Interfaces;
using MainLibrary.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Service
{
    public class TrainingService : ITrainingService
    {
        ITrainingRepo _trainingRepo;
        public TrainingService(ITrainingRepo trainingRepo)
        {
            _trainingRepo = trainingRepo;
        }

        public void AddTraining(Training training)
        {
            throw new NotImplementedException();
        }

        public void DeleteTraining(Training training)
        {
            throw new NotImplementedException();
        }

        public void EditTraining(Training training)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TrainingDetails> GetAllTraining()
        {
            return _trainingRepo.GetAllTraining();
        }

        public Training GetTraining(int id)
        {
            throw new NotImplementedException();
        }
    }
}
