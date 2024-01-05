using Entities.DbCustom;
using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces
{
    public interface IPrerequisiteService
    {
        Task<Prerequisite> GetPrerequisiteAsync(int prerequisiteId);
        Task<IEnumerable<Prerequisite>> GetAllPrerequisitesAsync();
        Task AddPrerequisiteAsync(Prerequisite prerequisite);
        Task DeletePrerequisiteAsync(int prerequisiteId);
        Task UpdatePrerequisiteAsync(Prerequisite prerequisite);
        Task<IEnumerable<PrerequisiteDetails>> GetPrerequisitesByTrainingAsync(int training);
        Task<IEnumerable<PrerequisiteAvailable>> GetAllPrerequisitesByTrainingAsync(int training);
    }
}
