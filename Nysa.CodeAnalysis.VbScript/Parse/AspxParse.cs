using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HtmlAgilityPack;

namespace Nysa.CodeAnalysis.VbScript
{

    public record AspxParse : Parse
    {
        public AspxContent  Content     { get; private set; }
        public HtmlDocument Document    { get; private set; }

        public AspxParse(AspxContent content, HtmlDocument document)
        {
            this.Content    = content;
            this.Document   = document;
        }
    }

}
