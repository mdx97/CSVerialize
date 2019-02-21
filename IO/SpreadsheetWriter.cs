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
            var columnHeadersString = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                var columnHeaderName = dt.Columns[i].ColumnName;
                if (i != 0)
                    columnHeadersString += Constants.Delimiter;
                columnHeadersString += columnHeaderName;
            }
            using (var writer = new StreamWriter(Path))
            {
                writer.WriteLine(columnHeadersString);
                foreach (var dataRow in dt.Rows)
                {
                    var rowString = "";
                    for (int i = 0; i < dataRow.ItemArray.Length; i++)
                    {
                        if (i != 0) 
                            rowString += Constants.Delimiter;
                        rowString += dataRow.ItemArray[i].ToString();
                    }
                    writer.WriteLine(rowString);
                }
            }
        }
    }
}
