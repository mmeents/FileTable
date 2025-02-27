using MessagePack;

namespace FileTables
{
    [MessagePackObject]
    public class FilePackage
    {
      [Key(0)]
      public IEnumerable<ColumnModel> Columns { get; set;} = new List<ColumnModel> (); 

      [Key(1)]
      public IEnumerable<FieldModel> Fields { get; set;} = new List<FieldModel>();

      [Key(2)]
      public IEnumerable<RowModel> Rows { get; set;} = new List<RowModel>();


    }
}
