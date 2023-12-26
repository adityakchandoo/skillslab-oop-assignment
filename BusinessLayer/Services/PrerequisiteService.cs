using BusinessLayer.Services.Interfaces;
using DataLayer.Repository.Interfaces;
using Entities.DbCustom;
using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class PrerequisiteService : IPrerequisiteService
    {
        private readonly IPrerequisiteRepo _prerequisiteRepo;
        public PrerequisiteService(IPrerequisiteRepo prerequisiteRepo)
        {
            _prerequisiteRepo = prerequisiteRepo;

        }
        public async Task AddPrerequisiteAsync(Prerequisite prerequisite)
        {
            await _prerequisiteRepo.Insert(prerequisite);
        }

        public async Task DeletePrerequisiteAsync(int prerequisiteId)
        {
            await _prerequisiteRepo.Delete(new Prerequisite() { PrerequisiteId = prerequisiteId });
        }

        public async Task<IEnumerable<Prerequisite>> GetAllPrerequisitesAsync()
        {
            return await _prerequisiteRepo.GetMany();
        }

        public async Task<Prerequisite> GetPrerequisiteAsync(int prerequisiteId)
        {
            return await _prerequisiteRepo.GetByPKAsync(prerequisiteId);
        }

        public async Task<IEnumerable<PrerequisiteDetails>> GetPrerequisitesByTrainingAsync(int training)
        {
            return await _prerequisiteRepo.GetPrerequisitesByTrainingAsync(training);
        }

        public async Task UpdatePrerequisiteAsync(Prerequisite prerequisite)
        {
            await _prerequisiteRepo.Update(prerequisite);
        }
    }
}
