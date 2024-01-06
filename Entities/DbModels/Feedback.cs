using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
