using MainLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Repo.Interfaces
{
    public interface ITrainingPrerequisiteRepo
    {
        TrainingPrerequisite GetTrainingPrerequisite(int id);
        IEnumerable<TrainingPrerequisite> GetAllTrainingPrerequisites();
        void CreateTrainingPrerequisite(TrainingPrerequisite trainingPrerequisite);
        void DeleteTrainingPrerequisites(int id);
        void UpdateTrainingPrerequisite(TrainingPrerequisite trainingPrerequisite);
    }
}
