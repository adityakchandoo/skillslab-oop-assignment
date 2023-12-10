using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Entities.FormDTO
{
    public class AddTrainingContentDTO
    {
        public int TrainingId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
    }
}
