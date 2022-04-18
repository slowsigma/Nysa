using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript
{

    public static class ContentFunctions
    {
        private static (String Hash, String Value) Read(this FileInfo file)
        {
            var sha = SHA256.Create();

            var value = File.ReadAllText(file.FullName);

            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(value));

            return (Convert.ToBase64String(hash), value);
        }

        private static String Normalized(this String filePath)
            => filePath.Trim().Replace("\\", "/");

        public static Suspect<Content> ToContent(this FileInfo file)
            =>   !file.Exists                         ? (new FileNotFoundException()).Failed<Content>()
               : file.Extension.DataEndsWith(".xsl")  ? file.Read().Make(t => (new XslContent(file.FullName.Normalized(), t.Hash, t.Value)).Confirmed<Content>())
               : file.Extension.DataEndsWith(".xslt") ? file.Read().Make(t => (new XslContent(file.FullName.Normalized(), t.Hash, t.Value)).Confirmed<Content>())
               : file.Extension.DataEndsWith(".htm")  ? file.Read().Make(t => (new HtmlContent(file.FullName.Normalized(), t.Hash, t.Value)).Confirmed<Content>())
               : file.Extension.DataEndsWith(".html") ? file.Read().Make(t => (new HtmlContent(file.FullName.Normalized(), t.Hash, t.Value)).Confirmed<Content>())
               : file.Extension.DataEndsWith(".vbs")  ? file.Read().Make(t => (new VbScriptContent(file.FullName.Normalized(), t.Hash, t.Value)).Confirmed<Content>())
               :                                        (new InvalidOperationException()).Failed<Content>();

        public static Suspect<Content> ToContent(this String filePath)
            => (new FileInfo(filePath)).ToContent();
    }

}