using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Text.Parsing;

public sealed class GrammarRule
{
    private SymbolIndex                 _Index  { get; init; }
    public Identifier                   Id      { get; init; }
    public String                       Symbol  => this._Index.Symbol(this.Id);
    public IReadOnlyList<Identifier>    DefinitionIds { get; init; }
    public Boolean IsEmpty              => this.DefinitionIds.Count == 0;

    internal GrammarRule(SymbolIndex index, Identifier id, IEnumerable<Identifier> definitionIds)
    {
        this._Index         = index;
        this.Id             = id;
        this.DefinitionIds  = new List<Identifier>(definitionIds);
    }

    public IEnumerable<String> DefinitionSymbols()
        => this.DefinitionIds.Select(id => this._Index.Symbol(id));
    public override string ToString()
        => String.Concat(this._Index.Symbol(this.Id), " ::= ", String.Join(" ", this.DefinitionSymbols()));
}
