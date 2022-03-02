using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Nysa.Logics;
using Nysa.Text;

namespace Nysa.CodeAnalysis.VbScript.Visualizer
{

    public class FileSource : ParseSource
    {
        public override String Identifier => this.FilePath;

        public String FilePath { get; private set; }

        public String BasePath
            => this.FilePath.Substring(1).DataStartsWith(@":\Odyssey\Webs\")
               ? this.FilePath.Substring(0, 15)  // C:\Odyssey\Webs
               : this.FilePath.Substring(0, 32); // C:\Odyssey\CustomExtensions\Webs

        public FileSource(String filePath, Func<Suspect<String>>? getSource = null)
            : base(() => getSource == null ? Return.Try(() => File.ReadAllText(filePath)) : getSource())
        {
            this.FilePath = filePath;
        }

        public String Ext
            => (new FileInfo(this.FilePath)).Extension;
    }

}
