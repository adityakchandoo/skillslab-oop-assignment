using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class DbErrorException : Exception
    {
        public DbErrorException(string message) : base(message)
        {
        }

        public DbErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DbErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
