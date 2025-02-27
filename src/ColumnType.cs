using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTables {
  public enum ColumnType {
    Null = 0,
    Int32 = 1,
    Int64 = 2,
    String = 3,    
    DateTime = 4,
    Boolean = 5,
    Decimal = 6,
    Bytes = 7,    
  }

}
