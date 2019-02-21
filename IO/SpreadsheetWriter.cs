using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVerialize.IO
{
    internal class SpreadsheetWriter
    {
        private string Path;

        public SpreadsheetWriter(string path)
        {
            Path = path;
        }

        public void Write(DataTable dt)
        {
            using (var writer = new StreamWriter(Path))
            {
                WriteHeaders(dt, writer);
                foreach (var dataRow in dt.Rows)
                    WriteRow(dr, writer);
            }
        }

        private void WriteHeaders(DataTable dt, ref StreamWriter writer)
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

        private void WriteRow(DataRow dr, ref StreamWriter writer)
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
