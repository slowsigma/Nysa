using System;

using Nysa.Data.TSqlClient;
using Nysa.CodeAnalysis.TSql;
using Nysa.Logics;
using Nysa.Text;

namespace Nysa.Test.Sn;

public static class Program
{

    public static void Main(String[] args)
    {
        var trial = Return.Try(() =>
        {
            if (1 == 0) { return 42; } else { throw new Exception("here it is"); }           
        });

        if (trial is Failed<Int32> badTry)
            Console.WriteLine(badTry.Value.Message);

        var isequal = "This DATA is equal to".DataEquals("This data is equal to");

        if (isequal)
            Console.WriteLine("data is equal");

        var q = With.Row(Of.Int32, Of.String)
                    .Make((i, s) => (Number: i, Value: s))
                    .ReadRecords();
    }

}
