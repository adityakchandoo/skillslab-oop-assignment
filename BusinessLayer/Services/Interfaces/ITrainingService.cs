using Entities.DbCustom;
using Entities.DbModels;
using Entities.DTO;
using Entities.Enums;
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
        Training GetTraining(int id);
        void AddTraining(Training training);
        void EditTraining(Training training);
        void DeleteTraining(int id);


        IEnumerable<TrainingEnrollCount> GetAllTrainingWithEnrollCount();
        IEnumerable<TrainingStatus> GetAllTraining(int UserId);
        IEnumerable<TrainingStatus> GetTrainingEnrolledByUser(int UserId, EnrollStatusEnum status);
        IEnumerable<TrainingStatus> GetTrainingEnrolledByUser(int UserId);
        IEnumerable<UserTraining> GetTrainingPendingForManager(int UserId);


        void AddTrainingWithTrainingPrerequisite(AddTrainingFormDTO training);
        void ApplyTraining(int UserId, int trainingId, List<UploadFileStore> uploadFileStore);
        IEnumerable<TrainingWithContentDTO> GetTrainingWithContents(int trainingId);
        void SaveTrainingWithContents(AddTrainingContentDTO addTrainingContentDTO);

    }
}
