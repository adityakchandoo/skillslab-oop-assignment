using MainLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Repo.Interfaces
{
    public interface IPrerequisiteRepo
    {
        Prerequisite GetPrerequisite(int prerequisiteId);
        IEnumerable<Prerequisite> GetAllPrerequisites();
        void CreatePrerequisite(Prerequisite prerequisite);
        void DeletePrerequisite(int prerequisiteId);
        void UpdatePrerequisite(Prerequisite prerequisite);
    }
}
