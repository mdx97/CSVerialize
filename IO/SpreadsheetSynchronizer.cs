using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVerialize.IO
{
    public class SpreadsheetSynchronizer
    {
        private DataTable _Table;
        internal DataTable Table
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

        public SpreadsheetSynchronizer(string path)
        {
            _Table = new DataTable();
            Writer = new SpreadsheetWriter(path);
            Reader = new SpreadsheetReader(path);
        }

        // DataTable -> CSV File
        private void SyncTableDown()
        {
            Writer.Write(_Table);
        }

        // CSV File -> DataTable
        private void SyncTableUp()
        {
            _Table = Reader.Read();
        }
    }
}
