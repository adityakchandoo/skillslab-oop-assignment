using BusinessLayer.Services.Interfaces;
using DataLayer.Repository.Interfaces;
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
        public void AddPrerequisite(Prerequisite prerequisite)
        {
            _prerequisiteRepo.Insert(prerequisite);
        }

        public void DeletePrerequisite(int prerequisiteId)
        {
            _prerequisiteRepo.Delete(new Prerequisite() { PrerequisiteId = prerequisiteId });
        }

        public IEnumerable<Prerequisite> GetAllPrerequisites()
        {
            return _prerequisiteRepo.GetMany();
        }

        public Prerequisite GetPrerequisite(int prerequisiteId)
        {
            return _prerequisiteRepo.GetByPK(prerequisiteId);
        }

        public void UpdatePrerequisite(Prerequisite prerequisite)
        {
            _prerequisiteRepo.Update(prerequisite);
        }
    }
}
