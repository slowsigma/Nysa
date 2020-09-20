using System;

namespace Nysa.Text.Parsing
{

    public class ParseTree : ParseResult
    {
        public Node         Root    { get; private set; }
        public FinalChart   Chart   { get; private set; }

        public ParseTree(Node root, FinalChart chart)
        {
            this.Root  = root;
            this.Chart = chart;
        }
    }

}
