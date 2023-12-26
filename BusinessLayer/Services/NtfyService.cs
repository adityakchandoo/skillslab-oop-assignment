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
        private async Task SendAsync(string body)
        {
            var url = "https://ntfy.sh/skillslab";
            var data = body;
            var content = new StringContent(data, Encoding.UTF8, "text/plain");

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsync(url, content);
                }
                catch (AggregateException ex)
                {
                    throw ex;
                    throw;
                }
            }
        }
        public async Task NotifyTrainingRequestAsync(string managerMail, string employeeName, string trainingName)
        {
            await SendAsync($"NotifyTrainingRequest: {managerMail}, {employeeName}, {trainingName}");
        }

        public async Task NotifyTrainingRequestProcessAsync(string employeeMail, string trainingName, bool isApproved)
        {
            await SendAsync($"NotifyTrainingRequestProcess: {employeeMail}, {trainingName}, {isApproved}");
        }

        public async Task NotifyUserRegistrationAsync(string managerMail, string employeeName)
        {
            await SendAsync($"NotifyUserRegistration: {managerMail}, {employeeName}");
        }

        public async Task NotifyUserRegistrationProcessAsync(string employeeMail, string managerName, bool isApproved)
        {
            await SendAsync($"NotifyUserRegistrationProcess: {employeeMail}, {managerName}, {isApproved}");
        }
    }
}
