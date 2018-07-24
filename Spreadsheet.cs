using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVerialize
{
    public class Spreadsheet
    {
        public string Path { get; set; }

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

        public Spreadsheet(string path)
        {
            Path = path;
            _Table = new DataTable();
        }

        // DataTable -> CSV File
        private void SyncTableDown()
        {
            using (var writer = new StreamWriter(Path))
            {
                string columnHeadersString = "";

                for (int i = 0; i < _Table.Columns.Count; i++)
                {
                    string columnHeaderName = _Table.Columns[i].ColumnName;

                    if (i != 0)
                    {
                        columnHeadersString += ",";
                    }

                    columnHeadersString += columnHeaderName;
                }

                writer.WriteLine(columnHeadersString);

                foreach (DataRow dataRow in _Table.Rows)
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

        // CSV File -> DataTable
        private void SyncTableUp()
        {
            _Table = new DataTable();

            using (var reader = new StreamReader(Path))
            {
                string[] columnHeaders = reader.ReadLine().Split(',');

                foreach (string header in columnHeaders)
                {
                    _Table.Columns.Add(header);
                }

                while (!reader.EndOfStream)
                {
                    string[] lineValues = reader.ReadLine().Split(',');
                    DataRow dr = _Table.NewRow();

                    for (int i = 0; i < lineValues.Length; i++)
                    {
                        dr[i] = lineValues[i];
                    }

                    _Table.Rows.Add(dr);
                }
            }
        }
    }
}
