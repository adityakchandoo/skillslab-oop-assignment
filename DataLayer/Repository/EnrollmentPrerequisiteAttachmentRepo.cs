﻿using DataLayer.Generic;
using DataLayer.Repository.Interfaces;
using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class EnrollmentPrerequisiteAttachmentRepo : DataAccessLayer<EnrollmentPrerequisiteAttachment>, IEnrollmentPrerequisiteAttachmentRepo
    {
        private readonly SqlConnection _conn;
        public EnrollmentPrerequisiteAttachmentRepo(IDbContext dbContext) : base(dbContext)
        {
            _conn = dbContext.GetConn();
        }

    }
}
