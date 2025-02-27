using MessagePack;

namespace FileTables
{

    [MessagePackObject]
    public class ColumnModel
    {
      [Key(0)]
      public int Id { get; set; }

      [Key(1)]
      public int Rank { get; set; }

      [Key(2)]
      public short ColumnType { get; set; }

      [Key(3)]
      public string ColumnName { get; set; } = "";    

    }
}
