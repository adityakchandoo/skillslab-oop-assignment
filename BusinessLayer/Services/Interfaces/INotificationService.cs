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
        void NotifyUserRegistration(string managerMail, string employeeName);
        void NotifyUserRegistrationProcess(string employeeMail, string managerName, bool isApproved);
        void NotifyTrainingRequest(string managerMail, string employeeName, string trainingName);
        void NotifyTrainingRequestProcess(string employeeMail, string trainingName, bool isApproved);
    }
}
