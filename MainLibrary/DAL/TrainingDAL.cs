using MainLibrary.DAL.Interfaces;
using MainLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.DAL
{
    public class TrainingDAL : ITrainingDAL
    {
        DbContext _dbContext;
        public TrainingDAL(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void CreateTraining(User user)
        {
            throw new NotImplementedException();
        }

        public void DeleteTraining(int user_id)
        {
            throw new NotImplementedException();
        }

        public User GetTraining(int user_id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetTrainingsByUser(int user_id)
        {
            throw new NotImplementedException();
        }

        public void UpdateTraining(User user)
        {
            throw new NotImplementedException();
        }
    }
}
