using MainLibrary.DTO;
using MainLibrary.Entities;
using MainLibrary.Repo;
using MainLibrary.Repo.Interfaces;
using MainLibrary.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MainLibrary.Service
{
    public class TrainingService : ITrainingService
    {
        private readonly ITrainingRepo _trainingRepo;
        private readonly ITrainingPrerequisiteRepo _trainingPrerequisiteRepo;
        public TrainingService(ITrainingRepo trainingRepo, ITrainingPrerequisiteRepo trainingPrerequisiteRepo)
        {
            _trainingRepo = trainingRepo;
            _trainingPrerequisiteRepo = trainingPrerequisiteRepo;
        }

        public void AddTraining(Training training)
        {
            _trainingRepo.CreateTraining(training);
        }

        public void AddTrainingAndTrainingPrerequisite(TrainingDTO training)
        {
            Training dbTraining = new Training()
            {
                Name = training.Name,
                Description = training.Description,
                Threshold = training.Threshold,
                Deadline = training.Deadline,
                ManagerId = training.ManagerId,
                PreferedDepartmentId = training.PriorityDepartmentId
            };

            int insertedId = _trainingRepo.CreateTraining(dbTraining);

            TrainingPrerequisite dbTrainingPrerequisite = new TrainingPrerequisite()
            {
                TrainingId = insertedId
            };

            foreach (int id in training.Prerequisites)
            {
                dbTrainingPrerequisite.PrerequisiteId = id;
                _trainingPrerequisiteRepo.CreateTrainingPrerequisite(dbTrainingPrerequisite);
            }
        }

        public void DeleteTraining(int id)
        {
            _trainingRepo.DeleteTraining(id);
        }

        public void EditTraining(Training training)
        {
            _trainingRepo.UpdateTraining(training);
        }

        public IEnumerable<TrainingDetails> GetAllTraining()
        {
            return _trainingRepo.GetAllTraining();
        }

        public Training GetTraining(int id)
        {
            return _trainingRepo.GetTraining(id);
        }
    }
}
