using MainLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Services.Interfaces
{
    public interface IPrerequisiteService
    {
        Prerequisite GetPrerequisite(int prerequisiteId);
        IEnumerable<Prerequisite> GetAllPrerequisites();
        void AddPrerequisite(Prerequisite prerequisite);
        void DeletePrerequisite(int prerequisiteId);
        void UpdatePrerequisite(Prerequisite prerequisite);
    }
}
