using DataLayer.Generic;
using Entities.DbModels;
using Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Interfaces
{
    public interface ITrainingContentAttachmentRepo : IDataAccessLayer<TrainingContentAttachment>
    {
        Task<IEnumerable<TrainingContentAttachment>> GetAllTrainingContentAttachmentAsync(int trainingContentId);
    }
}
