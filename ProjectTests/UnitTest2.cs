using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjectTests {
  public interface IHashService {
    public Hashes GetHash(int Id);
    public void SaveHash(Hashes item);
  }

  public class HashService {
    public Hashes? GetHash(int Id) { return null; }

    public void SaveHash(Hashes item) { }

  }

  public class Hashes {
    public Hashes() { }
    public long Id { get; set; } = 0;
    public string Hash { get; set; } = "";
    public long Size { get; set; } = 0;
  }
}
