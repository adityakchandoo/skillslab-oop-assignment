using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    public class EnrollmentAttachment
    {
        [Key]
        public int EnrollmentAttachmentId { get; set; }
        public int EnrollmentId { get; set; }
        public int TrainingPrerequisiteId { get; set; }
        public string OriginalFilename { get; set; }
        public string SystemFilename { get; set; }

    }

}
