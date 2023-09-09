using System;

namespace Nysa.Logics
{

    public abstract record Arg;

    // Notes: An argument name is any argument string that starts with a dash or forward
    //        slash followed by an alpha (a..z, A..Z) character.
    //        An argument string that is not classified as an argument name is an argument
    //        value. 

    // When iterating through argument strings, this type is
    // only possible while an argument name has not yet been
    // encountered.
    public sealed record LooseArg(
        String Value
    ) : Arg;

    // A FlagArg is returned for any argument name that has no
    // argument value following it.
    public sealed record FlagArg(
        String Name
    ) : Arg;

    // A ValueArg is returned for each argument value following
    // an argument name.
    public sealed record ValueArg(
        String Name,
        String Value
    ) : Arg;

}