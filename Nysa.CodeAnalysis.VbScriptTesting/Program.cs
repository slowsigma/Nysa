using Nysa.CodeAnalysis.VbScript;


var ptsA = "0123456789{@whatever}1".GetAttributeParts();
var ptsB = "Call this.SomeFunc(\"{{abunch of nothing}}\")".GetAttributeParts();
var ptsC = "Call this.Func({{fje{bleg}}})".GetAttributeParts();
var ptsD = "{@hello}and some".GetAttributeParts();
var ptsE = "and some {@hello}".GetAttributeParts();

Console.WriteLine("Done.");