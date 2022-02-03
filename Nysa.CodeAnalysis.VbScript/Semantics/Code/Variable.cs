using System;
using System.Collections.Generic;
using System.Linq;

using Nysa.Logics;

using SyntaxNode  = Dorata.Text.Parsing.Node;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{
    /*
     * -- Used for both VarName and FieldName
     * <VarName>   ::= <ExtendedID> "(" <ArrayRankList> ")" | <ExtendedID>
     * <FieldName> ::= <FieldID> "(" <ArrayRankList> ")" | <FieldID>
     * 
     * <ArrayRankList> ::= <IntLiteral> "," <ArrayRankList> | <IntLiteral> | 
     * 
     * <FieldID>    ::= {ID} | "Default" | "Erase" | "Error" | "Explicit" | "Step"
     * <ExtendedID> ::= <SafeKeywordID> | {ID}
     */

    public class Variable : CodeNode
    {
        public Identifier                           Name        { get; private set; }
        public Option<IReadOnlyList<LiteralValue>>  ArrayRanks  { get; private set; }

        public Variable(SyntaxNode source, Identifier name, Option<IEnumerable<LiteralValue>> arrayRanks)
            : base(source)
        {
            this.Name       = name;
            this.ArrayRanks = arrayRanks.Map(e => (IReadOnlyList<LiteralValue>)e.ToArray());
        }
    }

}
