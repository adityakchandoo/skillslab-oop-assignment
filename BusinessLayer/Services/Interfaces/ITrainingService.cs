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
        IEnumerable<TrainingDetails> GetAllTrainingDetails();
        IEnumerable<Training> GetAllTraining();
        Training GetTraining(int id);
        void AddTraining(Training training);
        void EditTraining(Training training);
        void DeleteTraining(int id);
        void AddTrainingWithTrainingPrerequisite(AddTrainingFormDTO training);
        void ApplyTraining(string UserId, int trainingId, List<UploadFileStore> uploadFileStore);
        IEnumerable<TrainingWithContentDTO> GetTrainingWithContents(int trainingId);
        IEnumerable<Training> GetTrainingEnrolledByUser(string UserId);

    }
}
