using BusinessLayer.Services.Interfaces;
using DataLayer.Repository;
using DataLayer.Repository.Interfaces;
using Entities.DbCustom;
using Entities.DbModels;
using Entities.DTO;
using Entities.Enums;
using Entities.FormDTO;
using System;
using System.Web;
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
        private readonly ITrainingContentRepo _trainingContentRepo;
        private readonly ITrainingContentAttachmentRepo _trainingContentAttachmentRepo;
        private readonly IAppUserRepo _appUserRepo;
        private readonly IUserTrainingEnrollmentRepo _userTrainingEnrollmentRepo;
        private readonly INotificationService _notificationService;
        private readonly IStorageService _storageService;
        private readonly IEnrollmentPrerequisiteAttachmentRepo _enrollmentPrerequisiteAttachmentRepo;

        public TrainingService(ITrainingRepo trainingRepo, ITrainingPrerequisiteRepo trainingPrerequisiteRepo, ITrainingContentRepo trainingContentRepo, ITrainingContentAttachmentRepo trainingContentAttachmentRepo, IAppUserRepo appUserRepo, IUserTrainingEnrollmentRepo userTrainingEnrollmentRepo, IEnrollmentPrerequisiteAttachmentRepo enrollmentPrerequisiteAttachmentRepo, INotificationService notificationService, IStorageService storageService)
        {
            _trainingRepo = trainingRepo;
            _trainingPrerequisiteRepo = trainingPrerequisiteRepo;
            _trainingContentRepo = trainingContentRepo;
            _trainingContentAttachmentRepo = trainingContentAttachmentRepo;
            _appUserRepo = appUserRepo;
            _userTrainingEnrollmentRepo = userTrainingEnrollmentRepo;
            _notificationService = notificationService;
            _enrollmentPrerequisiteAttachmentRepo = enrollmentPrerequisiteAttachmentRepo;
            _storageService = storageService;

        }

        public async Task<Training> GetTrainingAsync(int id)
        {
            return await _trainingRepo.GetByPKAsync(id);
        }
        public async Task AddTrainingAsync(Training training)
        {
            await _trainingRepo.Insert(training);
        }
        public async Task EditTrainingAsync(Training training)
        {
            await _trainingRepo.Update(training);
        }
        public async Task DeleteTrainingAsync(int id)
        {
            await _trainingRepo.Delete(new Training() { TrainingId = id });
        }


        public async Task<IEnumerable<TrainingEnrollCount>> GetAllTrainingWithEnrollCountAsync()
        {
            return await _trainingRepo.GetAllTrainingWithEnrollCountAsync();
        }
        public async Task<IEnumerable<TrainingStatus>> GetAllTrainingAsync(int UserId)
        {
            return await _trainingRepo.GetAllTrainingAsync(UserId);
        }
        public async Task<IEnumerable<TrainingStatus>> GetTrainingEnrolledByUserAsync(int UserId, EnrollStatusEnum status)
        {
            return await _trainingRepo.GetTrainingEnrolledByUserAsync(UserId,status);
        }

        public async Task<IEnumerable<TrainingStatus>> GetTrainingEnrolledByUserAsync(int UserId)
        {
            return await _trainingRepo.GetTrainingEnrolledByUserAsync(UserId);
        }
        public async Task<IEnumerable<UserTraining>> GetTrainingPendingForManagerAsync(int UserId)
        {
            return await _trainingRepo.GetUserTrainingByStatusAndManagerIdAsync(EnrollStatusEnum.Pending, UserId);
        }


        public async Task AddTrainingWithTrainingPrerequisiteAsync(AddTrainingFormDTO training)
        {
            // TODO: To Combine sql statements

            Training dbTraining = new Training()
            {
                Name = training.Name,
                Description = training.Description,
                MaxSeat = training.MaxSeat,
                Deadline = training.Deadline,
                PreferedDepartmentId = training.PriorityDepartmentId
            };

            int insertedId = await _trainingRepo.CreateTrainingReturningIDAsync(dbTraining);

            TrainingPrerequisite dbTrainingPrerequisite = new TrainingPrerequisite()
            {
                TrainingId = insertedId
            };

            foreach (int id in training.Prerequisites)
            {
                dbTrainingPrerequisite.PrerequisiteId = id;
                await _trainingPrerequisiteRepo.Insert(dbTrainingPrerequisite);
            }
        }
        public async Task ApplyTrainingAsync(int UserId, int trainingId, List<UploadFileStore> uploadFileStore)
        {
            // TODO: To Combine sql statements

            AppUser currentUser = await _appUserRepo.GetByPKAsync(UserId);
            AppUser currentUserManager = await _appUserRepo.GetUserManagerAsync(UserId);

            UserTrainingEnrollment enrollment = new UserTrainingEnrollment()
            {
                UserId = UserId,
                TrainingId = trainingId,
                Status = EnrollStatusEnum.Pending
            };

            int InsertedId = await _userTrainingEnrollmentRepo.CreateUserTrainingEnrollmentReturningIDAsync(enrollment);

            foreach (var File in uploadFileStore)
            {
                var genFileSystemName = Guid.NewGuid();

                _ = _storageService.Put(File.FileContent, genFileSystemName.ToString());

                await _enrollmentPrerequisiteAttachmentRepo.Insert(
                    new EnrollmentPrerequisiteAttachment() { 
                        EnrollmentId = InsertedId,
                        TrainingPrerequisiteId = File.FileId,
                        OriginalFilename = File.FileName,
                        FileKey = genFileSystemName
                    });
            }

            var training = await _trainingRepo.GetByPKAsync(trainingId);

            var employeeName = currentUser.FirstName + " " + currentUser.LastName;

            _ = _notificationService.NotifyTrainingRequestAsync(currentUserManager.Email, employeeName, training.Name);
        }
        public async Task<IEnumerable<TrainingWithContentDTO>> GetTrainingWithContentsAsync(int trainingId)
        {
            var result = new List<TrainingWithContentDTO>();
            
            var trainingContents = await _trainingContentRepo.GetMany("SELECT * FROM TrainingContent WHERE TrainingId = @TrainingId;",
                                         new Dictionary<string, object> { { "TrainingId", trainingId } });

            foreach (var item in trainingContents)
            {
                var attachments = await _trainingContentAttachmentRepo.GetMany("SELECT * FROM TrainingContentAttachment WHERE TrainingContentId = @TrainingContentId;",
                                         new Dictionary<string, object> { { "TrainingContentId", item.TrainingContentId } });

                result.Add(
                    new TrainingWithContentDTO()
                    {
                        TrainingContent = item,
                        TrainingContentAttachments = attachments.ToList()
                    });

            }
            return result;
        }
        public async Task SaveTrainingWithContentsAsync(AddTrainingContentDTO addTrainingContentDTO)
        {
            var trainingContent = new TrainingContent()
            {
                TrainingId = addTrainingContentDTO.TrainingId,
                Name = addTrainingContentDTO.Name,
                Description = addTrainingContentDTO.Description
            };

            var InsertedId = await _trainingContentRepo.CreateTrainingContentReturningIDAsync(trainingContent);

            if (addTrainingContentDTO.Files == null)
                return;

            foreach (HttpPostedFileBase File in addTrainingContentDTO.Files)
            {
                var genFileSystemName = Guid.NewGuid();

                _ = _storageService.Put(File.InputStream, genFileSystemName.ToString());

                await _trainingContentAttachmentRepo.Insert(new TrainingContentAttachment()
                {
                    TrainingContentId = InsertedId,
                    OriginalFilename = File.FileName,
                    FileKey = genFileSystemName,
                });
            }
        }

    }
}
