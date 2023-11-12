using MainLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Services.Interfaces
{
    internal interface INotificationService
    {
        void SendNotification(User user);
    }
}
