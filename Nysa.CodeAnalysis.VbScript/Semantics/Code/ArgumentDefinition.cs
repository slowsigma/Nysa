using System;
using System.Collections.Generic;
using System.Text;

using Nysa.Logics;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * <Arg> ::= <ArgModifierOpt> <ExtendedID> "(" ")" | <ArgModifierOpt> <ExtendedID>
     */

    public class ArgumentDefinition : CodeNode
    {
        public Option<ArgumentModifiers>    Modifier    { get; private set; }
        public Identifier                   Name        { get; private set; }
        public Boolean                      ArraySuffix { get; private set; }

        public ArgumentDefinition(SyntaxNode source, Option<ArgumentModifiers> modifier, Identifier name, Boolean arraySuffix)
            : base(source)
        {
            this.Modifier    = modifier;
            this.Name        = name;
            this.ArraySuffix = arraySuffix;
        }
    }

}
