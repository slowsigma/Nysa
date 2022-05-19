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

        private static String GetExtension(this FileInfo file, IReadOnlyDictionary<String, String>? extMappings)
            => (extMappings != null && extMappings.ContainsKey(file.Extension))
               ? extMappings[file.Extension]
               : file.Extension;

        public static Suspect<Content> ToContent(this FileInfo file, IReadOnlyDictionary<String, String>? extMappings = null)
            =>   !file.Exists                           ? (new FileNotFoundException()).Failed<Content>()
               : file.GetExtension(extMappings)
                     .Make(e =>   e.DataEquals(".xsl")  ? file.Read().Make(t => (new XslContent(file.FullName.Normalized(), t.Hash, t.Value)).Confirmed<Content>())
                                : e.DataEquals(".xslt") ? file.Read().Make(t => (new XslContent(file.FullName.Normalized(), t.Hash, t.Value)).Confirmed<Content>())
                                : e.DataEquals(".htm")  ? file.Read().Make(t => (new HtmlContent(file.FullName.Normalized(), t.Hash, t.Value)).Confirmed<Content>())
                                : e.DataEquals(".html") ? file.Read().Make(t => (new HtmlContent(file.FullName.Normalized(), t.Hash, t.Value)).Confirmed<Content>())
                                : e.DataEquals(".vbs")  ? file.Read().Make(t => (new VbScriptContent(file.FullName.Normalized(), t.Hash, t.Value)).Confirmed<Content>())
                                : e.DataEquals(".xml")  ? file.Read().Make(t => (new XmlContent(file.FullName.Normalized(), t.Hash, t.Value)).Confirmed<Content>())
                                : (new InvalidOperationException()).Failed<Content>());

        public static Suspect<Content> ToContent(this String filePath, IReadOnlyDictionary<String, String>? extMappings = null)
            => (new FileInfo(filePath)).ToContent(extMappings);
    }

}