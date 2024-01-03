using DataLayer.Generic;
using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Interfaces
{
    public interface ITrainingContentRepo : IDataAccessLayer<TrainingContent>
    {
        Task<IEnumerable<TrainingContent>> GetAllTrainingContentAsync(int trainingId);
        Task CreateTrainingContentWithAttachment(TrainingContent trainingContent, DataTable attachment);
        Task SoftDeleteTrainingContentAsync(int trainingContentId);
    }
}
