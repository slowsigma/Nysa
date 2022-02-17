using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript
{

    public static class ContentFunctions
    {
        public static Suspect<Content> ToContent(this FileInfo file)
            =>   !file.Exists                         ? (new FileNotFoundException()).Failed<Content>()
               : file.Extension.DataEndsWith(".xsl")  ? (new XslContent(file.FullName, File.ReadAllText(file.FullName))).Confirmed<Content>()
               : file.Extension.DataEndsWith(".xslt") ? (new XslContent(file.FullName, File.ReadAllText(file.FullName))).Confirmed<Content>()
               : file.Extension.DataEndsWith(".htm")  ? (new HtmlContent(file.FullName, File.ReadAllText(file.FullName))).Confirmed<Content>()
               : file.Extension.DataEndsWith(".html") ? (new HtmlContent(file.FullName, File.ReadAllText(file.FullName))).Confirmed<Content>()
               : file.Extension.DataEndsWith(".vbs")  ? (new VbScriptContent(file.FullName, File.ReadAllText(file.FullName))).Confirmed<Content>()
               :                                        (new InvalidOperationException()).Failed<Content>();

        public static Suspect<Content> ToContent(this String filePath)
            => (new FileInfo(filePath)).ToContent();

        public static VbScriptContent ToVbScriptContent(this String vbScript, String source)
            => new VbScriptContent(source, vbScript);

    }

}