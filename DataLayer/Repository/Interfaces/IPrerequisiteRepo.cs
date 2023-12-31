﻿using DataLayer.Generic;
using Entities.DbCustom;
using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Interfaces
{
    public interface IPrerequisiteRepo : IDataAccessLayer<Prerequisite>
    {
        Task<IEnumerable<PrerequisiteDetails>> GetPrerequisitesByTrainingAsync(int training);
        Task<IEnumerable<PrerequisiteAvailable>> GetAllPrerequisitesByTrainingAsync(int training);
    }
}
