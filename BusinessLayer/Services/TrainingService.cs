﻿using BusinessLayer.Services.Interfaces;
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

        public Training GetTraining(int id)
        {
            var train = _trainingRepo.GetByPK(id);


            return _trainingRepo.GetByPK(id);
        }
        public void AddTraining(Training training)
        {
            _trainingRepo.Insert(training);
        }
        public void EditTraining(Training training)
        {
            _trainingRepo.Update(training);
        }
        public void DeleteTraining(int id)
        {
            _trainingRepo.Delete(new Training() { TrainingId = id });
        }


        public IEnumerable<TrainingEnrollCount> GetAllTrainingWithEnrollCount()
        {
            return _trainingRepo.GetAllTrainingWithEnrollCount();
        }
        public IEnumerable<TrainingStatus> GetAllTraining(int UserId)
        {
            return _trainingRepo.GetAllTraining(UserId);
        }
        public IEnumerable<TrainingStatus> GetTrainingEnrolledByUser(int UserId, EnrollStatusEnum status)
        {
            return _trainingRepo.GetTrainingEnrolledByUser(UserId,status);
        }

        public IEnumerable<TrainingStatus> GetTrainingEnrolledByUser(int UserId)
        {
            return _trainingRepo.GetTrainingEnrolledByUser(UserId);
        }
        public IEnumerable<UserTraining> GetTrainingPendingForManager(int UserId)
        {
            return _trainingRepo.GetUserTrainingByStatusAndManagerId(EnrollStatusEnum.Pending, UserId);
        }


        public void AddTrainingWithTrainingPrerequisite(AddTrainingFormDTO training)
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
        public void ApplyTraining(int UserId, int trainingId, List<UploadFileStore> uploadFileStore)
        {
            // TODO: To Combine sql statements

            AppUser currentUser = _appUserRepo.GetByPK(UserId);
            AppUser currentUserManager = _appUserRepo.GetUserManager(UserId);

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

                _enrollmentPrerequisiteAttachmentRepo.Insert(
                    new EnrollmentPrerequisiteAttachment() { 
                        EnrollmentId = InsertedId,
                        TrainingPrerequisiteId = File.FileId,
                        OriginalFilename = File.FileName,
                        FileKey = genFileSystemName
                    });
            }

            var training = _trainingRepo.GetByPK(trainingId);

            var employeeName = currentUser.FirstName + " " + currentUser.LastName;

            _notificationService.NotifyTrainingRequest(currentUserManager.Email, employeeName, training.Name);
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
        public void SaveTrainingWithContents(AddTrainingContentDTO addTrainingContentDTO)
        {
            var trainingContent = new TrainingContent()
            {
                TrainingId = addTrainingContentDTO.TrainingId,
                Name = addTrainingContentDTO.Name,
                Description = addTrainingContentDTO.Description
            };

            var InsertedId = _trainingContentRepo.CreateTrainingContentReturningID(trainingContent);

            if (addTrainingContentDTO.Files == null)
                return;

            foreach (HttpPostedFileBase File in addTrainingContentDTO.Files)
            {
                var genFileSystemName = Guid.NewGuid();

                _storageService.Put(File.InputStream, genFileSystemName.ToString());

                _trainingContentAttachmentRepo.Insert(new TrainingContentAttachment()
                {
                    TrainingContentId = InsertedId,
                    OriginalFilename = File.FileName,
                    FileKey = genFileSystemName,
                });
            }
        }

    }
}
