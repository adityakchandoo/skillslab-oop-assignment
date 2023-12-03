using MainLibrary.Entities;
using MainLibrary.Repo.Interfaces;
using MainLibrary.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Services
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
            _prerequisiteRepo.CreatePrerequisite(prerequisite);
        }

        public void DeletePrerequisite(int prerequisiteId)
        {
            _prerequisiteRepo.DeletePrerequisite(prerequisiteId);
        }

        public IEnumerable<Prerequisite> GetAllPrerequisites()
        {
            return _prerequisiteRepo.GetAllPrerequisites();
        }

        public Prerequisite GetPrerequisite(int prerequisiteId)
        {
            return _prerequisiteRepo.GetPrerequisite(prerequisiteId);
        }

        public void UpdatePrerequisite(Prerequisite prerequisite)
        {
            _prerequisiteRepo.UpdatePrerequisite(prerequisite);
        }
    }
}
