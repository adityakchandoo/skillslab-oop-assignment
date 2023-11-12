using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Entities
{
    public class Application
    {
        public Application(User user, List<string> attachment, DateTime date, int selectedTrainingId)
        {
            this.user = user;
            Attachment = attachment;
            Date = date;
            SelectedTrainingId = selectedTrainingId;
        }

        public User user { get; set; }
        public List<string> Attachment { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int SelectedTrainingId { get; set; }

    }
}
