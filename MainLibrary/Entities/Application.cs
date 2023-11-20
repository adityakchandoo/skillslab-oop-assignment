using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Entities
{
    public class Application
    {
        public User user { get; set; }
        public List<string> Attachment { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int SelectedTrainingId { get; set; }

    }
}
