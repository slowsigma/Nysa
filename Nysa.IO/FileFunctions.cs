using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.IO
{

    public static class FileFunctions
    {

        public static TextContent Read(this FileInfo @this)
        {
            var value = File.ReadAllText(@this.FullName);
            var hash  = String.Empty;

            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(value));

                hash = Convert.ToBase64String(bytes);
            }

            return new TextContent(@this.FullName, hash, value);
        }

    }

}
 