using MainLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.DAL.Interfaces
{
    internal interface ITrainingDAL
    {
        IEnumerable<User> GetTrainingsByUser(int user_id);
        User GetTraining(int user_id);
        void CreateTraining(User user);
        void UpdateTraining(User user);
        void DeleteTraining(int user_id);
    }
}
