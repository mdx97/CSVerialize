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
                string columnHeadersString = "";

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string columnHeaderName = dt.Columns[i].ColumnName;

                    if (i != 0)
                    {
                        columnHeadersString += ",";
                    }

                    columnHeadersString += columnHeaderName;
                }

                writer.WriteLine(columnHeadersString);

                foreach (DataRow dataRow in dt.Rows)
                {
                    string rowString = "";

                    for (int i = 0; i < dataRow.ItemArray.Length; i++)
                    {
                        if (i != 0)
                        {
                            rowString += ",";
                        }

                        rowString += dataRow.ItemArray[i].ToString();
                    }

                    writer.WriteLine(rowString);
                }
            }
        }
    }
}
