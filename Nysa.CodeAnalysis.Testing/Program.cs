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

        public static void Main(string[] args)
        {

            //Tests.LexingTestDorata(args[0]);
            Tests.LexingTest(args[0]);


        }

    }

}