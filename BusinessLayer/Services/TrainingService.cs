using BusinessLayer.Services.Interfaces;
using DataLayer.Repository;
using DataLayer.Repository.Interfaces;
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

        public void AddTraining(Training training)
        {
            _trainingRepo.Insert(training);
        }

        public IEnumerable<TrainingDetails> GetAllTrainingDetails()
        {
            return _trainingRepo.GetAllTraining();
        }

        public IEnumerable<Training> GetAllTraining()
        {
            return _trainingRepo.GetMany();
        }

        public Training GetTraining(int id)
        {
            return _trainingRepo.GetByPK(id);
        }

        public void DeleteTraining(int id)
        {
            _trainingRepo.Delete(new Training() { TrainingId = id });
        }


        public void EditTraining(Training training)
        {
            _trainingRepo.Update(training);
        }

        public void AddTrainingWithTrainingPrerequisite(AddTrainingFormDTO training)
        {
            Training dbTraining = new Training()
            {
                Name = training.Name,
                Description = training.Description,
                Threshold = training.Threshold,
                Deadline = training.Deadline,
                PreferedDepartmentId = training.PriorityDepartmentId
            };

            int insertedId = _trainingRepo.CreateTrainingReturningID(dbTraining);

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

        public void ApplyTraining(string UserId, int trainingId, List<UploadFileStore> uploadFileStore)
        {
            AppUser currentUser = _appUserRepo.GetByPK(UserId);
            AppUser currentUserManager = _appUserRepo.GetByPK(currentUser.ManagerId);

            UserTrainingEnrollment enrollment = new UserTrainingEnrollment()
            {
                UserId = UserId,
                TrainingId = trainingId,
                Status = EnrollStatusEnum.Pending
            };

            int InsertedId = _userTrainingEnrollmentRepo.CreateUserTrainingEnrollmentReturningID(enrollment);

            foreach (var File in uploadFileStore)
            {
                var genFileSystemName = Guid.NewGuid();

                _storageService.Put(File.FileContent, genFileSystemName.ToString());

                _enrollmentPrerequisiteAttachmentRepo.Insert(new EnrollmentPrerequisiteAttachment() { EnrollmentId = InsertedId, TrainingPrerequisiteId = File.FileId, OriginalFilename = File.FileName, SystemFilename = genFileSystemName.ToString() });
            }

            NotificationDTO notificationDTO = new NotificationDTO()
            {
                To = currentUserManager.Email,
                Subject = "New Student Applied to Training",
                Body = "Check website"
            };

            //_notificationService.Send(notificationDTO);
        }

        public IEnumerable<Training> GetTrainingEnrolledByUser(string UserId)
        {
            return _trainingRepo.GetTrainingEnrolledByUser(UserId);
        }

        public IEnumerable<TrainingWithContentDTO> GetTrainingWithContents(int trainingId)
        {
            var result = new List<TrainingWithContentDTO>();
            
            var trainingContents = _trainingContentRepo.GetMany("SELECT * FROM TrainingContent WHERE TrainingId = @TrainingId;",
                                         new Dictionary<string, object> { { "TrainingId", trainingId } });

            foreach (var item in trainingContents)
            {
                var attachments = _trainingContentAttachmentRepo.GetMany("SELECT * FROM TrainingContentAttachment WHERE TrainingContentId = @TrainingContentId;",
                                         new Dictionary<string, object> { { "TrainingContentId", item.TrainingContentId } });

                result.Add(
                    new TrainingWithContentDTO()
                    {
                        TrainingContent = item,
                        TrainingContentAttachments = attachments
                    });

            }
            return result;
        }

    }
}
