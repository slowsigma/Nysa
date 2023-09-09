using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

namespace Nysa.Logics.Testing;

public static class ArgsTests
{

    public static Unit ConsoleDump(this IEnumerable<Arg> @this)
    {
        foreach (var arg in @this)
        {
            if (arg is LooseArg loose)
                Console.WriteLine($"LooseArg: {loose.Value}");
            else if (arg is FlagArg flag)
                Console.WriteLine($"FlagArg: {flag.Name}");
            else if (arg is ValueArg value)
                Console.WriteLine($"ValueArg: {value.Name} | {value.Value}");
            else
                Console.WriteLine("Program Error: arg is of unexpected type.");
        }

        return Unit.Value;
    }

    public static Unit ArgsTest_Alpha()
    {
        // what is an args ?

        // just strings on the command line with no lead character
        var argsOne = new String[] { "21", "hello", "goodbye", "i'm-not-happy" };

        Console.WriteLine("argsOne:");
        argsOne.AsArgs().ConsoleDump();

        // first character is '-' or '/' followed by a letter
        var argsTwo = new String[] { "-21", "-R21", "hello", "/goodbye", "i'm-not-happy" };

        Console.WriteLine("argsTwo:");
        argsTwo.AsArgs().ConsoleDump();

        // flags only
        var argsThree = new String[] { "/h", "-e", "-l", "/l", "-o" };

        Console.WriteLine("argsThree:");
        argsThree.AsArgs().ConsoleDump();


        return Unit.Value;
    }

}