using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVerialize.Exceptions
{
    public class InvalidSpreadsheetException : Exception
    {
        public string ErrorMessage { get; set; }

        public InvalidSpreadsheetException(string message)
        {
            ErrorMessage = message;
        }
    }
}
