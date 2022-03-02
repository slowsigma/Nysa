using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Visualizer
{

    public static class ParseSourceExtensions
    {

        public static Suspect<ParseSource> ToFileSource(this FileInfo file)
            =>   !file.Exists                         ? (new FileNotFoundException()).Failed<ParseSource>()
               : file.Extension.DataEndsWith(".htm")  ? (new FileSource(file.FullName)).Confirmed<ParseSource>()
               : file.Extension.DataEndsWith(".html") ? (new FileSource(file.FullName)).Confirmed<ParseSource>()
               : file.Extension.DataEndsWith(".vbs")  ? (new FileSource(file.FullName)).Confirmed<ParseSource>()
               :                                        (new InvalidOperationException()).Failed<ParseSource>();

        public static Suspect<ParseSource> ToParseSource(this String filePath)
            => (new FileInfo(filePath)).ToFileSource();

    }

}
