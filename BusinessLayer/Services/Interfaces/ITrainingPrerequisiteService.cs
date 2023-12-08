using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces
{
    public interface ITrainingPrerequisiteService
    {
        TrainingPrerequisite GetTrainingPrerequisite(int id);
        IEnumerable<TrainingPrerequisite> GetAllPrerequisites();
        void AddTrainingPrerequisite(TrainingPrerequisite trainingPrerequisite);
        void DeleteTrainingPrerequisite(int id);
        void UpdateTrainingPrerequisite(TrainingPrerequisite trainingPrerequisite);
    }
}
