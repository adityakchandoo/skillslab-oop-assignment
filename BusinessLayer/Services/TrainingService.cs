using BusinessLayer.Services.Interfaces;
using DataLayer.Repository.Interfaces;
using Entities.DbCustom;
using Entities.DbModels;
using Entities.DTO;
using Entities.FormDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BusinessLayer.Services
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
            _trainingRepo.Insert(training);
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

            int insertedId = _trainingRepo.Insert(dbTraining);

            TrainingPrerequisite dbTrainingPrerequisite = new TrainingPrerequisite()
            {
                TrainingId = insertedId
            };

            foreach (int id in training.Prerequisites)
            {
                dbTrainingPrerequisite.PrerequisiteId = id;
                _trainingPrerequisiteRepo.Insert(dbTrainingPrerequisite);
            }
        }

        public void DeleteTraining(int id)
        {
            _trainingRepo.Delete(new Training() { TrainingId = id });
        }

        public void EditTraining(Training training)
        {
            _trainingRepo.Update(training);
        }

        public IEnumerable<TrainingDetails> GetAllTraining()
        {
            return _trainingRepo.GetAllTraining();
        }

        public Training GetTraining(int id)
        {
            return _trainingRepo.GetByPK(id);
        }

        public IEnumerable<TrainingDetails> GetTrainingEnrolledByUser(string UserId)
        {
            return _trainingRepo.GetTrainingEnrolledByUser(UserId);
        }

        public IEnumerable<TrainingDetails> GetTrainingManagedByUser(string UserId)
        {
            return _trainingRepo.GetTrainingManagedByUser(UserId);
        }
    }
}
