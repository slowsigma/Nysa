using Nysa.CodeAnalysis.VbScript;
using System;

using Nysa.CodeAnalysis.VbScript;

namespace Nysa.CodeAnalysis.VbScriptTesting;

public class Program
{
    public static void Main(string[] args)
    {
        var ptsA = "0123456789{@whatever}1".GetAttributeParts();
        var ptsB = "Call this.SomeFunc(\"{{abunch of nothing}}\")".GetAttributeParts();
        var ptsC = "Call this.Func({{fje{bleg}}})".GetAttributeParts();
        var ptsD = "{@hello}and some".GetAttributeParts();
        var ptsE = "and some {@hello}".GetAttributeParts();

        var pt = Language.ParseWithTrivia(TestData.RandomGreeting);

        Console.WriteLine("Done.");
    }

}


