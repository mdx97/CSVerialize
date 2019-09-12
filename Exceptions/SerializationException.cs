using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVerialize.Exceptions
{
	public class SerializationException : Exception
	{
		public string ErrorMessage { get; set; }

		/// <summary>
		/// Creates an instance of the <see cref="SerializationException"/> class.
		/// </summary>
		/// <param name="message">The error message.</param>
		public SerializationException(string message)
		{
			ErrorMessage = message;
		}
	}
}
