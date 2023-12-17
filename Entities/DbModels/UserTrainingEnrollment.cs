using Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    public class UserTrainingEnrollment
    {
        [Key]
        public int UserTrainingEnrollmentId { get; set; }
        public int UserId { get; set; }
        public int TrainingId { get; set; }
        public DateTime ApplyDate { get; set; } = DateTime.Now;
        public DateTime? EnrolledDate { get; set; } = null;
        public EnrollStatusEnum Status { get; set; }

    }

}
