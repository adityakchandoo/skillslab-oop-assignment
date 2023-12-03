using MainLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Services.Interfaces
{
    internal interface IApplicationService
    {
        void TrainingApplication(Application application);
        void ApproveTrainingApplication(int app_id);
        void RemoveTrainingApplication(int user_id, int app_id);
    }
}
