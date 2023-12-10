using DataLayer.Generic;
using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Interfaces
{
    public interface ITrainingContentRepo : IDataAccessLayer<TrainingContent>
    {
        int CreateTrainingContentReturningID(TrainingContent trainingContent);
    }
}
