<span style="color:#04f772">CSVerialize</span>
=====

CSVerialize is a small C# library for converting CSV into strongly typed objects and vice-versa by utilizing Reflection.

#
### <span style="color:#04f772">General Usage</span>
After you have consumed the library in your project, import the CSVerialize namespace into your code.

```c#
using CSVerialize;
```
#
### <span style="color:#04f772">Serialization</span>

You will need a class that will match up with columns in a CSV table.

```c#
public class Student
{
    public string Name { get; set; }
    public string Major { get; set; }
    public int GPA { get; set; }
}
```

Use `List<object>` to store the objects you want to serialize into CSV and pass that list along with a file path for the spreadsheet to be written.

```c#
List<object> students = new List<object>();
students.Add(new Student() {Name = "Mike Allen", Major = "Computer Science", GPA = 3.0});
students.Add(new Student() {Name = Jerry Johnson, Major = "Accounting", GPA = 3.3});
students.Add(new Student() {Name = Sarah Adams, Major = "Mathematics", GPA = 3.5});

CSVerialize.Methods.Serialize(@"C:\Spreadsheets\Students.csv", students);
```

The resulting spreadsheet will be in this format:

|     Name      |       Major      |    GPA    |
|:--------------|:-----------------|:----------|
|   Mike Allen  | Computer Science |    3.0    |
| Jerry Johnson |    Accounting    |    3.3    |
|  Sarah Adams  |    Mathematics   |    3.5    |

#
### <span style="color:#04f772">De-Serialization</span>

We will use the same Student class from above to demonstrate De-Serialization.

The DeSerialize method returns a `List<object>`.

```c#
List<object> students = CSVerialize.Methods.DeSerialize(@"D:\TestSpreadsheet.csv", typeof(Student));
```