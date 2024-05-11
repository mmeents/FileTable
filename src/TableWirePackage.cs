using MessagePack;

namespace FileTables {

  [MessagePackObject]
  public class TableWirePackage {
    [Key(0)]
    public ICollection<string> Columns { get; set; } = new List<string>();

    [Key(1)]
    public ICollection<string> Rows { get; set; } = new List<string>();
  }
}