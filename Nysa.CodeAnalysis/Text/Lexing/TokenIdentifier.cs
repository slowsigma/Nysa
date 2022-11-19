using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Text.Lexing;

public class TokenIdentifier
{
    public static TokenIdentifier Create(Identifier id) => id.ToTokenIdentifier();

    internal Identifier          This   { get; init; }
    internal TokenIdentifier?    Next   { get; init; }

    internal TokenIdentifier(Identifier @this, TokenIdentifier? next)
    {
        this.This   = @this;
        this.Next   = next;
    }
}
