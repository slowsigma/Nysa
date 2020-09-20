using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Text.Parsing
{

    public partial class Grammar
    {

        public partial class Factory
        {

            public class Rule
            {
                internal String             Symbol          { get; private set; }
                internal NodeRetentionType  NodeRetention   { get; private set; }
                internal List<Symbol[]>     Definitions     { get; private set; }

                internal Rule(String symbol, NodeRetentionType nodeRetention)
                {
                    this.Symbol         = symbol;
                    this.NodeRetention  = nodeRetention;
                    this.Definitions    = new List<Symbol[]>();
                }

                internal Rule(Rule fromRule)
                {
                    this.Symbol         = fromRule.Symbol;
                    this.NodeRetention  = fromRule.NodeRetention;
                    this.Definitions    = fromRule.Definitions;
                }

                internal Rule WithDefinition(Boolean isSelfAllowed, Symbol[] definition)
                {
                    var found = false;

                    for (Int32 i = 0; i < definition.Length; i++)
                    {
                        if (definition[i].Value.Equals(Grammar.Symbol.Self) ||
                            definition[i].Value.Equals(this.Symbol, StringComparison.OrdinalIgnoreCase))
                        {
                            if (!isSelfAllowed)
                                throw new ArgumentException($"Error in {this.Symbol}. This type of rule does not allow direct recursion.", nameof(definition));

                            definition[i] = this.Symbol;   // replace possible self expression value with parentSymbol

                            found = true;
                        }
                    }

                    if (found && definition.Length == 1)
                        throw new ArgumentException($"Error in {this.Symbol}. Rules are not allowed a definition with a single recursive member.", nameof(definition));

                    this.Definitions.Add(definition);
                    return this;
                }
            }

        }

    }

}
