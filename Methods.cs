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
                var synchronizer = new SpreadsheetSynchronizer(path);
                Spreadsheets.Add(path, synchronizer);
                return synchronizer;
            }
        }

        public static List<object> DeSerialize(string path, Type type)
        {
            var synchronizer = GetSpreadsheetSynchronizerForPath(path);
            var objectList = new List<object>();
            foreach (var dr in synchronizer.Table.Rows)
            {
                var obj = DeSerializeRow(dr, type);
                objectList.Add(obj);
            }
            return objectList;
        }

        private static object DeSerializeRow(DataRow dr, Type type)
        {
            var obj = Activator.CreateInstance(type);
            foreach (var column in dr.Table.Columns)
            {
                var columnName = column.ColumnName;
                var propertyInfo = type.GetProperty(columnName);
                if (propertyInfo == null)
                    throw new SerializationException($"Column '{columnName}' does not represent a property in Type '{type}'");
                propertyInfo.SetValue(obj, dr[columnName], null);
            }
            return obj;
        }
        
        public static void Serialize(string path, List<object> objectList)
        {
            var synchronizer = GetSpreadsheetSynchronizerForPath(path);
            var table = synchronizer.Table;
            table.Rows.Clear();
            foreach (object obj in objectList)
            {
                var dr = table.NewRow();
                SerializeObject(obj, dr);
                table.Rows.Add(dr);
            }
            synchronizer.Table = table;
        }

        private static void SerializeObject(object obj, ref DataRow dr)
        {
            foreach (var property in obj.GetType().GetProperties())
            {
                var propertyName = property.Name;
                if (!table.Columns.Contains(propertyName))
                    throw new SerializationException($"Property '{propertyName}' does not represent a column in the spreadsheet.");
                dr[propertyName] = property.GetValue(obj);
            }
        }
    }
}
