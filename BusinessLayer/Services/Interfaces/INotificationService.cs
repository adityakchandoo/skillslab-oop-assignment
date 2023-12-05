﻿using Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces
{
    public interface INotificationService
    {
        void Send(NotificationDTO notification);
    }
}