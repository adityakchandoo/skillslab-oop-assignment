using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    public class TrainingContent
    {
        [Key]
        public int TrainingContentId { get; set; }
        public int TrainingId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime PostDate { get; set; } = DateTime.Now;

    }

}
