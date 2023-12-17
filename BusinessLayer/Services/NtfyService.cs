using BusinessLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class NtfyService : INotificationService
    {
        private void Send(string body)
        {
            var url = "https://ntfy.sh/skillslab";
            var data = body;
            var content = new StringContent(data, Encoding.UTF8, "text/plain");

            using (var client = new HttpClient())
            {
                try
                {
                    var response = client.PostAsync(url, content).Result;
                }
                catch (AggregateException ex)
                {
                    throw ex;
                    throw;
                }
            }
        }
        public void NotifyTrainingRequest(string managerMail, string employeeName, string trainingName)
        {
            Send($@"NotifyTrainingRequest: {managerMail}, {employeeName}, {trainingName}");
        }

        public void NotifyTrainingRequestProcess(string employeeMail, string trainingName, bool isApproved)
        {
            Send($@"NotifyTrainingRequestProcess: {employeeMail}, {trainingName}, {isApproved}");
        }

        public void NotifyUserRegistration(string managerMail, string employeeName)
        {
            Send($@"NotifyUserRegistration: {managerMail}, {employeeName}");
        }

        public void NotifyUserRegistrationProcess(string employeeMail, string managerName, bool isApproved)
        {
            Send($@"NotifyUserRegistrationProcess: {employeeMail}, {managerName}, {isApproved}");
        }
    }
}
