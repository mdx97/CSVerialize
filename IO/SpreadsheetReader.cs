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
            var dt = new DataTable();
            using (var reader = new StreamReader(Path))
            {
                ReadHeaders(reader, dt);
                while (!reader.EndOfStream)
                    ReadRow(reader, dr);
            }
            return dt;
        }

        private void ReadHeaders(ref StreamReader reader, ref DataTable dt)
        {
            string[] columnHeaders = reader.ReadLine().Split(Constants.Delimiter);
            foreach (var header in columnHeaders)
                dt.Columns.Add(header);
        }

        private void ReadRow(ref StreamReader reader, ref DataTable dt)
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
