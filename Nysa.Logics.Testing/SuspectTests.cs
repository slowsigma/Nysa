using System;

using Nysa.Logics;

namespace Nysa.Logics.Testing;

public static class SuspectTests
{

    public static Unit TestCombine()
    {
        var checkOne = (Suspect<Int32>)(new Exception("One is bad.")).Failed<Int32>();
        var checkTwo = (Suspect<Int32>)(new Exception("Two is bad.")).Failed<Int32>();
        var checkThree = (Suspect<Int32>)(42.Confirmed());
        var checkFour = (Suspect<Int32>)(97.Confirmed());

        var x = checkOne.Combine(checkTwo, (a, b) => (IntOne: a, IntTwo: b))
                        .Combine(checkThree, (p, c) => (p.IntOne, p.IntTwo, IntThree: c));

        Console.WriteLine("Simple Combine() test:");
        if (x is Confirmed<(Int32 IntOne, Int32 IntTwo, Int32 IntThree)> goodInts)
        {
            Console.WriteLine("This is bad and should not have happened.");
        }
        else if (x is Failed<(Int32 IntOne, Int32 IntTwo, Int32 IntThree)> badInts)
        {
            Console.WriteLine(badInts.Value.ToString());
        }
        else
            Console.WriteLine("Program Error.");

        var all = new Suspect<Int32>[] { checkOne, checkTwo, checkThree, checkFour };

        Console.WriteLine("Simple Split() test:");
        var (good, bad) = all.Split();
        foreach (var g in good)
            Console.WriteLine($"Good: {g}");
        foreach (var b in bad)
            Console.WriteLine($"Bad: {b}");

        Console.WriteLine("Simple ConfirmedOnly() test:");
        foreach (var v in all.ConfirmedOnly())
            Console.WriteLine($"Good: {v}");

        Console.WriteLine("Simple FailedOnly() test:");
        foreach (var f in all.FailedOnly())
            Console.WriteLine($"Bad: {f}");

        return Unit.Value;
    }

}