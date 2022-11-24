using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Text.Lexing;

public static class TokenFunctions
{

    public static TokenIdentifier ToTokenIdentifier(this Identifier @this)
        => new TokenIdentifier(@this, null);

    public static Boolean IsEqual(this TokenIdentifier @this, Identifier id)
        =>    @this.This.Equals(id)
           || (   @this.Next != null
               && @this.Next.IsEqual(id) );

    public static Boolean IsEqual(this TokenIdentifier @this, TokenIdentifier other)
    {
        TokenIdentifier? current = @this;

        while (current != null)
        {
            if (other.IsEqual(current.This))
                return true;

            current = current.Next;
        }

        return false;
    }

    public static IEnumerable<Identifier> Values(this TokenIdentifier @this)
    {
        TokenIdentifier? current = @this;

        while (current != null)
        {
            yield return current.This;

            current = current.Next;
        }
    }

    public static TokenIdentifier MergedWith(this TokenIdentifier @this, TokenIdentifier other)
    {
        if (@this.IsEqual(other))
            return @this;

        var combined = @this.Values().Concat(other.Values()).Aggregate((TokenIdentifier?)null, (a, i) => new TokenIdentifier(i, a));

        return combined == null ? new TokenIdentifier(Identifier.None, null) : combined;
    }

}