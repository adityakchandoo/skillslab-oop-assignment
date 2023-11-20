using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using MainLibrary.Repo.Interfaces;
using MainLibrary.Entities;

namespace MainLibrary.Repo
{
    public class TrainingRepo : ITrainingRepo
    {
        DbContext _dbContext;
        public TrainingRepo(DbContext dbContext)
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
