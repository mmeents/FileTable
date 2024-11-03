using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTables {

  public class Field {
    public Row Owner { get; set; }
    public Column Column { get; set; }
    public string Value { get; set; }
    public Field(Row aRow, Column aCol, string aValue) {
      Owner = aRow;
      Column = aCol;
      Value = aValue;
    }
    public Object AsObj {
      get { return this.AsObject(); }
      set { this.Value = value?.AsString() ?? ""; }
    }
  }

  public static class FieldExt {
    public static string AsEncoded(this Field field) {
      if (field == null) { return ""; }
      string value = field.Value;
      value = value == "" ? "<null>" : value;
      return $"{field.Column.Name.AsBase64Encoded()} {value.AsBase64Encoded()}".AsBase64Encoded();
    }

    public static Field? AsDecoded(this Row row, string encoded) {
      if (!string.IsNullOrEmpty(encoded)) {
        var vals = encoded.AsBase64Decoded().Parse(" ");
        var colName = vals[0].AsBase64Decoded();
        var col = row.Owner.Cols.ByName(colName);
        var value = vals[1].AsBase64Decoded();
        value = value == "<null>" ? "" : value;
        if (col != null) {
          return new Field(row, col, value);
        }
      }
      return null;
    }

    public static Object AsObject(this Field field) {
      Object ret = "";
      switch (field.Column.Type) {
        case ColumnType.Null: ret = ""; break;
        case ColumnType.String: ret = field.Value; break;
        case ColumnType.Int32: ret = field.Value.AsInt32(); break;
        case ColumnType.DateTime: ret = field.Value.AsDateTime(); break;
        case ColumnType.Boolean: ret = field.Value.AsBoolean(); break;
        case ColumnType.Decimal: ret = field.Value.AsDecimal(); break;
        case ColumnType.Byte: ret = field.Value.AsBytes(); break;
        case ColumnType.Int64: ret = field.Value.AsInt64(); break;
      }
      return ret;
    }

    public static int AsInt32(this string value) {
      return int.Parse(value);
    }
    public static long AsInt64(this string value) {
      return long.Parse(value);
    }
    public static byte[] AsByte(this string value) {
      return Convert.FromBase64String(value);
    }
    public static bool AsBoolean(this string value) {
      return Convert.ToBoolean(value);
    }
    public static DateTime AsDateTime(this string value) {
      return DateTime.Parse(value);
    }
    public static Decimal AsDecimal(this string value) {
      return Decimal.Parse(value);
    }
    public static byte[] AsBytes(this string text) {
      return Encoding.UTF8.GetBytes(text);
    }
    public static string AsString(this byte[] bytes) {
      return Encoding.UTF8.GetString(bytes);
    }

    public static string[] Parse(this string content, string delims) {
      return content.Split(delims.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    }
    public static string ParseFirst(this string content, string delims) {
      string[] sr = content.Parse(delims);
      if (sr.Length > 0) {
        return sr[0];
      }
      return "";
    }
    public static string ParseLast(this string content, string delims) {
      string[] sr = content.Parse(delims);
      if (sr.Length > 0) {
        return sr[sr.Length - 1];
      }
      return "";
    }    

  }



}
