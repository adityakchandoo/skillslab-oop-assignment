using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbCustom
{
    public class TrainingWithUserStatusPG
    {
        public int totalPages { get; set; }
        public IEnumerable<TrainingWithUserStatus> trainingWithUserStatus { get; set; }
    }
}
