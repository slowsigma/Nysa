using System;

namespace Nysa.CodeAnalysis.VbScript;

public static class Throw
{

    public static T CodingError<T>(String functionName, String message)
        => throw new InvalidOperationException($"Error in code at '{functionName}': {message}");
    public static T UnexpectedType<T>(String functionName, String argumentName)
        => throw new InvalidOperationException($"Unexpected type for argument '{argumentName}' in function '{functionName}'.");
    public static T UnexpectedTree<T>(String functionName, String argumentName)
        => throw new InvalidOperationException($"Unexpected semantic content in parse tree encountered in '{functionName}' for argument '{argumentName}'.");
        
}