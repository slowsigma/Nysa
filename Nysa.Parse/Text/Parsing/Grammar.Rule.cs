using System;
using System.Collections.Generic;

namespace Nysa.Text.Parsing
{

    public partial class Grammar
    {

        private struct Rule
        {
            public String            Symbol { get; private set; }
            public List<RuleVariant> Variants { get; private set; }
            public Boolean           IsTerminal => this.Variants == Terminal;
            public NodeRetentionType NodeRetention { get; private set; }

            public Rule(String name, List<RuleVariant> variants, NodeRetentionType nodeRetention)
            {
                this.Symbol         = name;
                this.Variants       = variants;
                this.NodeRetention  = nodeRetention;
            }
        }

    }

}
