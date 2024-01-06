using Entities.DbCustom;
using Entities.DbModels;
using Entities.DTO;
using Entities.Enums;
using Entities.FormDTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces
{
    public interface ITrainingService
    {
        Task<Training> GetTrainingAsync(int id);
        Task AddTrainingAsync(Training training);
        Task EditTrainingAsync(AddTrainingFormDTO training);
        Task DeleteTrainingAsync(int id);


        Task<IEnumerable<TrainingEnrollCount>> GetAllTrainingWithEnrollCountAsync();
        Task<TrainingWithUserStatusPG> GetAllTrainingAsync(int UserId, int pageNumber);
        Task<TrainingWithUserStatusPG> GetTrainingEnrolledByUserAsync(int UserId, EnrollStatusEnum status, int pageNumber);
        Task<TrainingWithUserStatusPG> GetTrainingEnrolledByUserAsync(int UserId, int pageNumber);
        Task<IEnumerable<UserTraining>> GetTrainingPendingForManagerAsync(int UserId);


        Task AddTrainingWithTrainingPrerequisiteAsync(AddTrainingFormDTO training);
        Task ApplyTrainingAsync(int UserId, int trainingId, List<UploadFileStore> uploadFileStore);
        Task<IEnumerable<TrainingWithContentDTO>> GetTrainingWithContentsAsync(int trainingId);
        Task SaveTrainingContentWithAttachmentAsync(AddTrainingContentDTO addTrainingContentDTO);
        Task SoftDeleteTrainingAsync(int trainingId);
        Task SoftDeleteTrainingContentAsync(int trainingContentId);

        Task<Stream> ExportSelectedEmployeesAsync(int trainingId);
        Task AutoProcess();
    }
}
