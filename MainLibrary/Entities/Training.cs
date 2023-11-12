using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Entities
{
    public class Training
    {
        public Training(int id, string name, string description, int treshhold, DateTime deadline)
        {
            Id = id;
            Name = name;
            Description = description;
            Treshhold = treshhold;
            Deadline = deadline;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Treshhold { get; set; }
        public DateTime Deadline { get; set; }
    }
}
