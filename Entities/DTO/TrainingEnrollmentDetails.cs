using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class TrainingEnrollmentDetails
    {
        public int EnrollmentPrerequisiteAttachmentId { get; set; }
        public int PrerequisiteId { get; set; }
        public int TrainingId { get; set; }
        public string PrerequisiteName { get; set; }
        public string OriginalFilename { get; set; }
        public Guid FileKey { get; set; }
    }
}
