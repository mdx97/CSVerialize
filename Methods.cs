using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CSVerialize.IO;
using CSVerialize.Exceptions;

namespace CSVerialize
{
    public class Methods
    {
        private static Dictionary<string, SpreadsheetSynchronizer> Spreadsheets = new Dictionary<string, SpreadsheetSynchronizer>();

        private static SpreadsheetSynchronizer GetSpreadsheetSynchronizerForPath(string path)
        {
            if (Spreadsheets.ContainsKey(path))
            {
                return Spreadsheets[path];
            }
            else
            {
                SpreadsheetSynchronizer synchronizer = new SpreadsheetSynchronizer(path);
                Spreadsheets.Add(path, synchronizer);

                return synchronizer;
            }
        }

        public static List<object> DeSerialize(string path, Type type)
        {
            SpreadsheetSynchronizer synchronizer = GetSpreadsheetSynchronizerForPath(path);
            List<object> objectList = new List<object>();

            foreach (DataRow dr in synchronizer.Table.Rows)
            {
                object obj = Activator.CreateInstance(type);

                foreach (DataColumn column in dr.Table.Columns)
                {
                    string columnName = column.ColumnName;
                    PropertyInfo property = type.GetProperty(columnName);

                    if (property != null)
                    {
                        property.SetValue(obj, dr[columnName], null);
                    }
                    else
                    {
                        throw new SerializationException($"Column '{columnName}' does not represent a property in Type '{type}'");
                    }
                }

                objectList.Add(obj);
            }

            return objectList;
        }

        public static void Serialize(string path, List<object> objectList)
        {
            SpreadsheetSynchronizer synchronizer = GetSpreadsheetSynchronizerForPath(path);
            DataTable table = synchronizer.Table;
            table.Rows.Clear();

            foreach (object obj in objectList)
            {
                DataRow dr = table.NewRow();

                foreach (PropertyInfo property in obj.GetType().GetProperties())
                {
                    string propertyName = property.Name;

                    if (table.Columns.Contains(propertyName))
                    {
                        dr[propertyName] = property.GetValue(obj);
                    }
                    else
                    {
                        throw new SerializationException($"Property '{propertyName}' does not represent a column in the spreadsheet.");
                    }
                }

                table.Rows.Add(dr);
            }

            synchronizer.Table = table;
        }
    }
}
