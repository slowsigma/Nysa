using System.Security.Cryptography;
using System.Text;

using Nysa.Logics;

var value = args.Value("value").Map(v => Encoding.UTF8.GetBytes(v));
var key   = args.Value("key").Map(k => Encoding.UTF8.GetBytes(k));
var box   = args.Value("box").Map(b => Convert.FromBase64String(b));

if (value is Some<Byte[]> someValue && key is Some<Byte[]> someValueKey)
{
    using (var db = new Rfc2898DeriveBytes(someValueKey.Value, new Byte[] { 14, 31, 17 }, 77))
    {
        var hv = db.GetBytes(someValue.Value.Length);

        var elements = new List<Byte>();

        for (Int32 i = 0; i < someValue.Value.Length; i++)
            elements.Add((Byte)(someValue.Value[i] ^ hv[i % hv.Length]));

        Console.WriteLine("Box:");
        Console.WriteLine(Convert.ToBase64String(elements.ToArray()));
    }
}
else if (key is Some<Byte[]> someBoxKey && box is Some<Byte[]> someBox)
{
    using (var db = new Rfc2898DeriveBytes(someBoxKey.Value, new Byte[] { 14, 31, 17 }, 77))
    {
        var hv = db.GetBytes(someBox.Value.Length);

        var elements = new List<Byte>();

        for (Int32 i = 0; i < someBox.Value.Length; i++)
            elements.Add((Byte)(someBox.Value[i] ^ hv[i % hv.Length]));

        Console.WriteLine("Value:");
        Console.WriteLine(Encoding.UTF8.GetString(elements.ToArray()));
    }
}