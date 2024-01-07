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
using BusinessLayer.Other;
using System.IO;
using Entities.AppLogger;
using System.Data;

namespace BusinessLayer.Services
{
    public class TrainingService : ITrainingService
    {
        private ILogger _logger;
        private readonly ITrainingRepo _trainingRepo;
        private readonly ITrainingPrerequisiteRepo _trainingPrerequisiteRepo;
        private readonly ITrainingContentRepo _trainingContentRepo;
        private readonly ITrainingContentAttachmentRepo _trainingContentAttachmentRepo;
        private readonly IAppUserRepo _appUserRepo;
        private readonly IUserTrainingEnrollmentRepo _userTrainingEnrollmentRepo;
        private readonly INotificationService _notificationService;
        private readonly IStorageService _storageService;
        private readonly IEnrollmentPrerequisiteAttachmentRepo _enrollmentPrerequisiteAttachmentRepo;

        public TrainingService(ILogger logger, ITrainingRepo trainingRepo, ITrainingPrerequisiteRepo trainingPrerequisiteRepo, ITrainingContentRepo trainingContentRepo, ITrainingContentAttachmentRepo trainingContentAttachmentRepo, IAppUserRepo appUserRepo, IUserTrainingEnrollmentRepo userTrainingEnrollmentRepo, IEnrollmentPrerequisiteAttachmentRepo enrollmentPrerequisiteAttachmentRepo, INotificationService notificationService, IStorageService storageService)
        {
            _logger = logger;
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

        public async Task EditTrainingAsync(AddTrainingFormDTO trainingDto)
        {
            Training training = await _trainingRepo.GetByPKAsync(trainingDto.TrainingId);
            training.Name = trainingDto.Name;
            training.Description = trainingDto.Description;
            training.MaxSeat = trainingDto.MaxSeat;
            training.Deadline = trainingDto.Deadline;
            training.PreferedDepartmentId = trainingDto.PriorityDepartmentId;

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
        public async Task<TrainingWithUserStatusPG> GetAllTrainingAsync(int UserId, int pageNumber)
        {
            return await _trainingRepo.GetAllTrainingAsync(UserId, pageNumber);
        }
        public async Task<TrainingWithUserStatusPG> GetTrainingEnrolledByUserAsync(int UserId, EnrollStatusEnum status, int pageNumber)
        {
            return await _trainingRepo.GetTrainingEnrolledByUserAsync(UserId, status, pageNumber);
        }

        public async Task<TrainingWithUserStatusPG> GetTrainingEnrolledByUserAsync(int UserId, int pageNumber)
        {
            return await _trainingRepo.GetTrainingEnrolledByUserAsync(UserId, pageNumber);
        }
        public async Task<IEnumerable<UserTraining>> GetTrainingPendingForManagerAsync(int UserId)
        {
            return await _trainingRepo.GetUserTrainingByStatusAndManagerIdAsync(EnrollStatusEnum.Pending, UserId);
        }


        public async Task AddTrainingWithTrainingPrerequisiteAsync(AddTrainingFormDTO training)
        {
            Training Training = new Training()
            {
                Name = training.Name,
                Description = training.Description,
                MaxSeat = training.MaxSeat,
                Deadline = training.Deadline,
                PreferedDepartmentId = training.PriorityDepartmentId
            };

            DataTable Prerequisites = new DataTable();
            Prerequisites.Columns.Add("id", typeof(int));

            if (training.Prerequisites != null)
            {
                foreach (int id in training.Prerequisites)
                {
                    Prerequisites.Rows.Add(id);
                }
            }

            await _trainingRepo.CreateTrainingWithPrerequisite(Training, Prerequisites);           

        }
        public async Task ApplyTrainingAsync(int UserId, int trainingId, List<UploadFileStore> uploadFileStore)
        {
            AppUser currentUser = await _appUserRepo.GetByPKAsync(UserId);
            AppUser currentUserManager = await _appUserRepo.GetUserManagerAsync(UserId);

            if (currentUserManager == null)
            {
                throw new ArgumentException("This User has no manager");
            }

            var fileName = uploadFileStore.Select(file => file.FileName).ToArray();

            if (CommonValidations.AreAllFilesValid(fileName) == false)
                throw new ArgumentException("File Type not allowed");

            UserTrainingEnrollment enrollment = new UserTrainingEnrollment()
            {
                UserId = UserId,
                TrainingId = trainingId,
                ManagerApprovalStatus = EnrollStatusEnum.Pending,
                EnrollStatus = EnrollStatusEnum.Pending
            };

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("TrainingPrerequisiteId", typeof(int));
            dataTable.Columns.Add("OriginalFilename", typeof(string));
            dataTable.Columns.Add("FileKey", typeof(Guid));

            foreach (var File in uploadFileStore)
            {
                var genFileSystemName = Guid.NewGuid();

                _ = _storageService.Put(File.FileContent, genFileSystemName.ToString());

                dataTable.Rows.Add(File.FileId, File.FileName, genFileSystemName);
            }

            await _userTrainingEnrollmentRepo.CreateEnrollmentWithAttachments(enrollment, dataTable);

            var training = await _trainingRepo.GetByPKAsync(trainingId);

            var employeeName = currentUser.FirstName + " " + currentUser.LastName;

            _ = _notificationService.NotifyTrainingRequestAsync(currentUserManager.Email, employeeName, training.Name);
        }
        public async Task<IEnumerable<TrainingWithContentDTO>> GetTrainingWithContentsAsync(int trainingId)
        {
            var result = new List<TrainingWithContentDTO>();
            
            var trainingContents = await _trainingContentRepo.GetAllTrainingContentAsync(trainingId);

            foreach (var item in trainingContents)
            {
                var attachments = await _trainingContentAttachmentRepo.GetAllTrainingContentAttachmentAsync(item.TrainingContentId);

                result.Add(
                    new TrainingWithContentDTO()
                    {
                        TrainingContent = item,
                        TrainingContentAttachments = attachments.ToList()
                    });

            }
            return result;
        }
        public async Task SaveTrainingContentWithAttachmentAsync(AddTrainingContentDTO addTrainingContentDTO)
        {
            var trainingContent = new TrainingContent()
            {
                TrainingId = addTrainingContentDTO.TrainingId,
                Name = addTrainingContentDTO.Name,
                Description = addTrainingContentDTO.Description
            };

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("OriginalFilename", typeof(string));
            dataTable.Columns.Add("FileKey", typeof(Guid));

            if (addTrainingContentDTO.Files != null)
            {
                var fileName = addTrainingContentDTO.Files.Select(file => file.FileName).ToArray();

                if (CommonValidations.AreAllFilesValid(fileName) == false)
                    throw new ArgumentException("File Type not allowed");

                foreach (HttpPostedFileBase File in addTrainingContentDTO.Files)
                {
                    var genFileSystemName = Guid.NewGuid();

                    _ = _storageService.Put(File.InputStream, genFileSystemName.ToString());

                    dataTable.Rows.Add(File.FileName, genFileSystemName);
                }
            }

            await _trainingContentRepo.CreateTrainingContentWithAttachment(trainingContent, dataTable);
        }

        public async Task<Stream> ExportSelectedEmployeesAsync(int trainingId)
        {
            IEnumerable<TrainingEmployeeDetails> employeeDetails = await _trainingRepo.GetAllTrainingEmployeeDetailsByTrainingId(trainingId);

            string employeeDetailsCommaSeparated = CsvConvert.Convert(employeeDetails);

            var byteArray = Encoding.UTF8.GetBytes(employeeDetailsCommaSeparated);
            var stream = new MemoryStream(byteArray);

            return stream;
        }

        public async Task SoftDeleteTrainingAsync(int trainingId)
        {
            try
            {
                if (trainingId <= 0)
                {
                    throw new ArgumentException("trainingId must be a positive integer.");
                }

                await _trainingRepo.SoftDeleteTrainingAsync(trainingId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw ex;
                throw;
            }
        }


        public async Task SoftDeleteTrainingContentAsync(int trainingContentId)
        {
            try
            {
                if (trainingContentId <= 0)
                {
                    throw new ArgumentException("trainingContentId must be a positive integer.");
                }

                await _trainingContentRepo.SoftDeleteTrainingContentAsync(trainingContentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw ex;
                throw;
            }
        }

        public async Task AutoProcess()
        {
            IEnumerable<AutoProcessOutput> selectedEmployees = await _trainingRepo.AutoProcess();

            foreach (var Employee in selectedEmployees)
            {
                _ = _notificationService.NotifyTrainingRequestProcessAsync(Employee.Email, Employee.TrainingName, true, null);
            }
        }

    }
}
