using FileTables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ProjectTests {
  [TestClass]
  public class FieldTests {
    private Column _column;
    private Row _row;
    private Field _field;

    [TestInitialize]
    public void Setup() {
      var columns = new Columns();
      FileTable table = new FileTable("TestTable");
      _column = table.AddColumn("TestColumn", ColumnType.String);             
      long newRowId = table.AddRow();
      _row = table.Rows[newRowId];
      _field = new Field(_row, _column, "TestValue");
    }

    [TestMethod]
    public void TestFieldInitialization() {
      Assert.AreEqual("TestColumn", _field.Column.Name);
      Assert.AreEqual("TestValue", _field.Value);
    }

    [TestMethod]
    public void TestFieldAsObject() {
      _field.Column.Type = ColumnType.String;
      Assert.AreEqual("TestValue", _field.AsObj);

      _field.Column.Type = ColumnType.Int32;
      _field.Value = "123";
      Assert.AreEqual(123, _field.AsObj);

      _field.Column.Type = ColumnType.Boolean;
      _field.Value = "true";
      Assert.AreEqual(true, _field.AsObj);
    }

    [TestMethod]
    public void TestFieldAsEncoded() {
      string encoded = _field.AsEncoded();
      Assert.IsFalse(string.IsNullOrEmpty(encoded));
    }

    [TestMethod]
    public void TestFieldAsDecoded() {
      string encoded = _field.AsEncoded();
      Field? decodedField = _row.AsDecoded(encoded);
      Assert.IsNotNull(decodedField);
      Assert.AreEqual(_field.Column.Name, decodedField.Column.Name);
      Assert.AreEqual(_field.Value, decodedField.Value);
    }

    [TestMethod]
    public void TestStringConversions() {
      string intValue = "123";
      Assert.AreEqual(123, intValue.AsInt32());

      string longValue = "123456789";
      Assert.AreEqual(123456789L, longValue.AsInt64());

      string boolValue = "true";
      Assert.AreEqual(true, boolValue.AsBoolean());

      string dateTimeValue = "2023-10-01";
      Assert.AreEqual(new DateTime(2023, 10, 01), dateTimeValue.AsDateTime());

      string decimalValue = "123.45";
      Assert.AreEqual(123.45M, decimalValue.AsDecimal());

      string base64Value = "SGVsbG8=";
      Assert.AreEqual("Hello", base64Value.AsBase64Decoded());
    }
  }
}
