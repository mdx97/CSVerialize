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
	/// <summary>
	/// Contains the public interface methods for the library.
	/// </summary>
	public class Methods
	{
		private static Dictionary<string, SpreadsheetSynchronizer> Spreadsheets = new Dictionary<string, SpreadsheetSynchronizer>();

		/// <summary>
		/// Returns the related <see cref="SpreadsheetSynchronizer"/> object for the given file path.
		/// </summary>
		/// <param name="path">The absolute path of the spreadsheet.</param>
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

		/// <summary>
		/// Deserializes the spreadsheet at the given path.
		/// </summary>
		/// <param name="path">The absolute path of the spreadsheet.</param>
		/// <param name="type">The type to deserialize to.</param>
		/// <returns>A list of strongly typed objects constructed from the data in the spreadsheet.</returns>
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

		/// <summary>
		/// Deserializes a single row.
		/// </summary>
		/// <param name="dr">The row to deserialize.</param>
		/// <param name="type">The type to deserialize to.</param>
		/// <returns>A strongly typed object constructed from the data in the row.</returns>
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

		/// <summary>
		/// Serializes a list of strongly typed objects into a spreadsheet.
		/// </summary>
		/// <param name="path">The absolute path of the spreadsheet.</param>
		/// <param name="objectList">A list of strongly typed objects to serialize.</param>
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

		/// <summary>
		/// Serializes a single object into a row.
		/// </summary>
		/// <param name="obj">The strongly typed object to serialize.</param>
		/// <param name="dr">The row to store the data from the object in.</param>
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
