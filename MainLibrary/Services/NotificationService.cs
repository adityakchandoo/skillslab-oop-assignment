using MainLibrary.DTO;
using MainLibrary.Entities;
using MainLibrary.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Service
{
    public class NotificationService : INotificationService
    {
        public void Send(NotificationDTO notification)
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
                    Subject = notification.Subject,
                    Body = notification.Body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(notification.To);

                // Send the email
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in sending email: " + ex.Message);
            }
        }
    }
}
