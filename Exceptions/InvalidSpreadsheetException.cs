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

		/// <summary>
		/// Creates an instance of the <see cref="InvalidSpreadsheetException"/> class.
		/// </summary>
		/// <param name="message">The error message.</param>
		public InvalidSpreadsheetException(string message)
		{
			ErrorMessage = message;
		}
	}
}
