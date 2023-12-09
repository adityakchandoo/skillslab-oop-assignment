using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    public class TrainingContentAttachment
    {
        [Key]
        public int TrainingContentAttachmentId { get; set; }
        public int TrainingContentId { get; set; }
        public string OriginalFilename { get; set; }
        public string SystemFilename { get; set; }
    }
}
