using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Helpers
{
    public class BadAuthentication : Exception
    {
        public BadAuthentication() : base() { }
        public BadAuthentication(string message) : base(message) { }
        public BadAuthentication(string message, Exception e) : base(message, e) { }
    }
}
