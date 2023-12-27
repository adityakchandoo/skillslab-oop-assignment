using DataLayer;
using DataLayer.Generic;
using DataLayer.Repository.Interfaces;
using Entities.AppLogger;
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
    public class DepartmentRepo : DataAccessLayer<Department>, IDepartmentRepo
    {
        ILogger _logger;
        private readonly SqlConnection _conn;
        public DepartmentRepo(ILogger logger, IDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _conn = dbContext.GetConn();
        }
    }
}
