using Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces
{
    public interface INotificationService
    {
        Task NotifyUserRegistrationAsync(string managerMail, string employeeName);
        Task NotifyUserRegistrationProcessAsync(string employeeMail, string managerName, bool isApproved);
        Task NotifyTrainingRequestAsync(string managerMail, string employeeName, string trainingName);
        Task NotifyTrainingRequestProcessAsync(string employeeMail, string trainingName, bool isApproved, string declineReason);
    }
}
