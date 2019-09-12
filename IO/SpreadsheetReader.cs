using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSVerialize.Exceptions;

namespace CSVerialize.IO
{
	/// <summary>
	/// Handles the reading operations for a single spreadsheet file.
	/// </summary>
	internal class SpreadsheetReader
	{
		private string Path;

		/// <summary>
		/// Initializes a new instance of the <see cref="SpreadsheetReader"/> class.
		/// </summary>
		/// <param name="path">The absolute path of the spreadsheet.</param>
		public SpreadsheetReader(string path)
		{
			Path = path;
		}

		/// <summary>
		/// Reads data from the spreadsheet.
		/// </summary>
		/// <returns>A DataTable representation of the spreadsheet.</returns>
		public DataTable Read()
		{
			var dt = new DataTable();
			using (var reader = new StreamReader(Path))
			{
				ReadHeaders(reader, dt);
				while (!reader.EndOfStream)
					ReadRow(reader, dr);
			}
			return dt;
		}

		/// <summary>
		/// Reads the column headers from the spreadsheet.
		/// </summary>
		/// <param name="reader">Controls reading operations from the spreadsheet.</param>
		/// <param name="dt">The DataTable to write the column header names to.</param>
		private static void ReadHeaders(ref StreamReader reader, ref DataTable dt)
		{
			string[] columnHeaders = reader.ReadLine().Split(Constants.Delimiter);
			foreach (var header in columnHeaders)
				dt.Columns.Add(header);
		}

		/// <summary>
		/// Reads a single row from the spreadsheet.
		/// </summary>
		/// <param name="reader">Controls reading operations from the spreadsheet.</param>
		/// <param name="dt">The DataTable to add the row to.</param>
		private static void ReadRow(ref StreamReader reader, ref DataTable dt)
		{
			var dr = dt.NewRow();
			string[] lineValues = reader.ReadLine().Split(Constants.Delimiter);
			if (lineValues.Length != dt.Columns.Count)
				throw new InvalidSpreadsheetException($"Spreadsheet '{Path}' is not properly formatted!");
			for (int i = 0; i < lineValues.Length; i++)
				dr[i] = lineValues[i];
			dt.Rows.Add(dr);
		}
	}
}
