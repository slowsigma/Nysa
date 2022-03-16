using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Nysa.Logics;
using Nysa.Text.Parsing;
using Nysa.Text.Lexing;

//using Dorata.Text.Lexing;
//using Dorata.Text.Parsing;

using Nysa.CodeAnalysis.VbScript;

namespace Nysa.CodeAnalysis.Testing
{

    public class Program
    {
        private static String[] _DefaultEventAttributeNames = new String[]
        {
            "onafterprint",
            "onbeforeprint",
            "onbeforeunload",
            "onerror",
            "onhashchange",
            "onload",
            "onmessage",
            "onoffline",
            "ononline",
            "onpagehide",
            "onpageshow",
            "onpopstate",
            "onresize",
            "onstorage",
            "onunload",
            "onblur",
            "onchange",
            "oncontextmenu",
            "onfocus",
            "oninput",
            "oninvalid",
            "onreset",
            "onsearch",
            "onselect",
            "onsubmit",
            "onkeydown",
            "onkeypress",
            "onkeyup",
            "onclick",
            "ondblclick",
            "onmousedown",
            "onmousemove",
            "onmouseout",
            "onmouseover",
            "onmouseup",
            "onmousewheel",
            "onwheel",
            "ondrag",
            "ondragend",
            "ondragenter",
            "ondragleave",
            "ondragover",
            "ondragstart",
            "ondrop",
            "onscroll",
            "oncopy",
            "oncut",
            "onpaste",
            "onabort",
            "oncanplay",
            "oncanplaythrough",
            "oncuechange",
            "ondurationchange",
            "onemptied",
            "onended",
            "onerror",
            "onloadeddata",
            "onloadedmetadata",
            "onloadstart",
            "onpause",
            "onplay",
            "onplaying",
            "onprogress",
            "onratechange",
            "onseeked",
            "onseeking",
            "onstalled",
            "onsuspend",
            "ontimeupdate",
            "onvolumechange",
            "onwaiting",
            "ontoggle"
        };        

        public static void Main(string[] args)
        {
            var events = _DefaultEventAttributeNames.ToHashSet(StringComparer.OrdinalIgnoreCase);

            var testfile = @"C:\Git\cj-odyssey_mainline\Webs\CaseManagement\AttorneyManager\Charges\XML\tabProsecutorCharges.xsl";

            if (testfile.ToContent() is Suspect<Content> suspect && suspect is Confirmed<Content> goodContent && goodContent.Value is XslContent xslContent)
            {
                var parse = xslContent.Parse(events);
            }




            foreach (var file in Directory.EnumerateFiles(args[0], "*.vbs", SearchOption.TopDirectoryOnly))
            {
                var parse = VBScript.Parse(File.ReadAllText(file));

                if (parse is Confirmed<Node> root)
                {

                }
                else
                {
                    parse.Affect(c => {},
                                 e => { Console.WriteLine(e.ToString()); });
                }
            }
        }

    }

}