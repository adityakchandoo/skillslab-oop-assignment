using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Entities
{
    public class Training
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Treshhold { get; set; }
        public DateTime Deadline { get; set; }
    }
}
