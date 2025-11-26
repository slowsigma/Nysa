using System;

namespace Nysa.CodeAnalysis.VbScriptTesting;

public static class TestData
{
    public static readonly string HelloWorld =
@"' This is a sample VBScript code

Dim message
message = ""Hello, World!""

WScript.Echo message    

Function AddNumbers(a, b)
    AddNumbers = a + b
End Function    

Dim result

result = AddNumbers(5, 10)

WScript.Echo ""The result is: "" & result
";

    public static readonly string RandomGreeting =
$"""
    option explicit

    ' This script generates a random German greeting.

    Dim x, l, t

    x = datepart("s", time)
    L = 5

    ' Initialize the random number generator with the current second.
    randomize x

    ' This function returns a greeting based on the random arg.
    function GetMessage(rndValue)
        if (rndValue < 4) then
            GetMessage = "Guten Morgen"
        elseif (rndValue < 6) then
            GetMessage = "Guten Tag"
        else
            GetMessage = "Guten Abend"
        end if
    end function

    do while (l > 1)
    x = Rnd(x)
    l = l - 1
    loop

    ' Rnd returns a value between 0 and 1, so multiply by 10 to get a larger range.
    t = x * 10

    MsgBox getmessage(t)
    ' We're done.
 """;

}
