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
	/// Wraps all the reading and writing operations for a single spreadsheet.
	/// </summary>
	internal class SpreadsheetSynchronizer
	{
		private DataTable _Table;
		public DataTable Table
		{
			get
			{
				SyncTableUp();
				return _Table;
			}

			set
			{
				_Table = value;
				SyncTableDown();
			}
		}

		private SpreadsheetWriter Writer;
		private SpreadsheetReader Reader;

		/// <summary>
		/// Initializes a new instance of the <see cref="SpreadsheetSynchronizer"/> class.
		/// </summary>
		/// <param name="path">The absolute path of the spreadsheet.</param>
		public SpreadsheetSynchronizer(string path)
		{
			_Table = new DataTable();
			Writer = new SpreadsheetWriter(path);
			Reader = new SpreadsheetReader(path);
		}

		/// <summary>
		/// Writes the DataTable to the spreadsheet.
		/// </summary>
		private void SyncTableDown()
		{
			Writer.Write(_Table);
		}

		/// <summary>
		/// Reads the spreadsheet into the DataTable.
		/// </summary>
		private void SyncTableUp()
		{
			_Table = Reader.Read();
		}
	}
}
