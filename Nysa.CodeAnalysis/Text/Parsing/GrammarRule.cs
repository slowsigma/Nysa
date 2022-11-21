using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Text.Parsing;

public sealed record GrammarRule
{
    public Grammar      Grammar { get; init; }
    public Identifier   Id      { get; init; }
    public String       Symbol  => this.Grammar.Symbol(this.Id);
    public IReadOnlyList<Identifier> DefinitionIds { get; init; }
    public Boolean IsEmpty      => this.DefinitionIds.Count == 0;

    public GrammarRule(Grammar grammar, Identifier id, IEnumerable<Identifier> definitionIds)
    {
        this.Grammar        = grammar;
        this.Id             = id;
        this.DefinitionIds  = new List<Identifier>(definitionIds);
    }

    public IEnumerable<String> DefinitionSymbols()
        => this.DefinitionIds.Select(id => this.Grammar.Symbol(id));
    public override string ToString()
        => String.Concat(this.Grammar.Symbol(this.Id), " ::= ", String.Join(" ", this.DefinitionSymbols()));
}
