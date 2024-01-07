using DataLayer.Generic;
using DataLayer.Repository.Interfaces;
using Entities.AppLogger;
using Entities.DbModels;
using System.Data.SqlClient;

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
