using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Services.Interfaces;
using Entities.DTO;

namespace BusinessLayer.Services
{
    public class EmailService : INotificationService
    {
        private void Send(string to, string subject, string body)
        {
            try
            {
                // Set up SMTP client
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("adityakc.work@gmail.com", "tkwpcfcdznrassfl"),
                    EnableSsl = true
                };

                // Create the email
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("adityakc.work@gmail.com"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);

                // Send the email
                // TODO:  Mail Disabled
                //client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in sending email: " + ex.Message);
            }
        }

        public void NotifyUserRegistration(string managerMail, string employeeName)
        {

            string subject = $"New Employee Registration - {employeeName}";
            string htmlBody = $@"
                <html>
                <body>
                    <p>Hello, a new user has registered under you</p>
                    <p>Name: <strong>{employeeName},</strong>.</p>
                    <br/>
                    <p><strong>Action Required</strong></p>
                    <p>Please process user in user panel.</p>
                </body>
                </html>
            ";

            Send(managerMail, subject, htmlBody);
        }

        public void NotifyUserRegistrationProcess(string employeeMail, string managerName, bool isApproved)
        {
            string result = isApproved ? "Approved" : "Rejected";
            // build the message body
            string subject = $"Your Registration has been {result}";
            string htmlBody = $@"
                <html>
                <body>
                    <p>Hello, Your Manager {managerName} has <strong>{result}</strong> you.</p>
                    <br/>
                    <p>Please liaise with your manager for further information.</p>
                </body>
                </html>
            ";

            Send(employeeMail, subject, htmlBody);
        }

        public void NotifyTrainingRequest(string managerMail, string employeeName, string trainingName)
        {
            string subject = $"New Training Request";
            string htmlBody = $@"
                <html>
                <body>
                    <p>Hello, An Employee has request for an training</p>
                    <br/>
                    <p>Employee Name: {employeeName}</p>
                    <p>Training Requested: {trainingName}</p>
                </body>
                </html>
            ";

            Send(managerMail, subject, htmlBody);
        }
        
        public void NotifyTrainingRequestProcess(string employeeMail, string trainingName, bool isApproved)
        {
            string result = isApproved ? "Approved" : "Rejected";

            string subject = $"Your Training Request Has been {result}";
            string htmlBody = $@"
                <html>
                <body>
                    <p>Hello, Your Manager has <strong>{result}</strong> your training request {trainingName} you.</p>
                    <br/>
                    <p>Please liaise with your manager for further information.</p>
                </body>
                </html>
            ";

            Send(employeeMail, subject, htmlBody);

        }

    }
}
