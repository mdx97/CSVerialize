using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CSVerialize.IO;

namespace CSVerialize
{
    public class Methods
    {
        public static List<object> DeSerialize(SpreadsheetSynchronizer spreadsheet, Type type)
        {
            List<object> objectList = new List<object>();

            foreach (DataRow dr in spreadsheet.Table.Rows)
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

        public static void Serialize(SpreadsheetSynchronizer spreadsheet, List<object> objectList)
        {
            DataTable table = spreadsheet.Table;
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

            spreadsheet.Table = table;
        }
    }
}
