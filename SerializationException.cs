using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVerialize
{
    public class SerializationException : Exception
    {
        public string ErrorMessage { get; set; }

        public SerializationException(string message)
        {
            ErrorMessage = message;
        }
    }
}
