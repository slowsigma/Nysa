using System;

using Nysa.Logics;

namespace Nysa.CodeAnalysis.VbScript
{

    public class VbScriptContent : Content
    {
        public Option<String> Element { get; private set; } // represent where a script is when contained in a larger document or table

        public VbScriptContent(String source, String value, Option<String> element)
            : base(source, value)
        {
            this.Element = element;
        }
    }

}