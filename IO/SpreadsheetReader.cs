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
    internal class SpreadsheetReader
    {
        private string Path;

        public SpreadsheetReader(string path)
        {
            Path = path;
        }

        public DataTable Read()
        {
            DataTable dt = new DataTable();
            using (var reader = new StreamReader(Path))
            {
                string[] columnHeaders = reader.ReadLine().Split(Constants.Delimiter);
                foreach (string header in columnHeaders)
                    dt.Columns.Add(header);

                while (!reader.EndOfStream)
                {
                    string[] lineValues = reader.ReadLine().Split(Constants.Delimiter);
                    if (lineValues.Length == dt.Columns.Count)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < lineValues.Length; i++)
                            dr[i] = lineValues[i];
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        throw new InvalidSpreadsheetException($"Spreadsheet '{Path}' is not properly formatted!");
                    }
                }
            }

            return dt;
        }
    }
}
