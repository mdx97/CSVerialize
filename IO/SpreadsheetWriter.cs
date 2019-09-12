using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVerialize.IO
{
	/// <summary>
	/// Handles the writing operations for a single spreadsheet file.
	/// </summary>
	internal class SpreadsheetWriter
	{
		private string Path;

		/// <summary>
		/// Initializes a new instance of the <see cref="SpreadsheetWriter"/> class.
		/// </summary>
		/// <param name="path">The absolute path of the spreadsheet.</param>
		public SpreadsheetWriter(string path)
		{
			Path = path;
		}

		/// <summary>
		/// Writes data to the spreadsheet.
		/// </summary>
		/// <param name="dt">The source of the data being written.</param>
		public void Write(DataTable dt)
		{
			using (var writer = new StreamWriter(Path))
			{
				WriteHeaders(dt, writer);
				foreach (var dataRow in dt.Rows)
					WriteRow(dr, writer);
			}
		}

		/// <summary>
		/// Writes column headers to the spreadsheet.
		/// </summary>
		/// <param name="dt">The source for the column header names.</param>
		/// <param name="writer">Controls writing operations to the spreadsheet.</param>
		private static void WriteHeaders(DataTable dt, ref StreamWriter writer)
		{
			var columnHeadersString = "";
			for (int i = 0; i < dt.Columns.Count; i++)
			{
				var columnHeaderName = dt.Columns[i].ColumnName;
				if (i != 0)
					columnHeadersString += Constants.Delimiter;
				columnHeadersString += columnHeaderName;
			}
			writer.WriteLine(columnHeadersString);
		}

		/// <summary>
		/// Writes a single row to the spreadsheet.
		/// </summary>
		/// <param name="dr">The source of the data being written.</param>
		/// <param name="writer">Controls writing operations to the spreadsheet.</param>
		private static void WriteRow(DataRow dr, ref StreamWriter writer)
		{
			var rowString = "";
			for (int i = 0; i < dr.ItemArray.Length; i++)
			{
				if (i != 0)
					rowString += Constants.Delimiter;
				rowString += dr.ItemArray[i].ToString();
			}
			writer.WriteLine(rowString);
		}
	}
}
