using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Nysa.Logics;
using Nysa.Text.Parsing;
using Nysa.Text.Lexing;

//using Dorata.Text.Lexing;
//using Dorata.Text.Parsing;

namespace Nysa.CodeAnalysis.Testing
{

    public class Program
    {

        public static void Main(string[] args)
        {
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