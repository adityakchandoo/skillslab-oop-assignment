using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class TrainingWithContentDTO
    {
        public TrainingContent TrainingContent { get; set; }
        public List<TrainingContentAttachment> TrainingContentAttachments { get; set;}
    }
}
