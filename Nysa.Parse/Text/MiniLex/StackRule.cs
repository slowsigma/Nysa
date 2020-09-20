using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Text.Lexing.Mini
{

    public sealed class StackRule : Rule
    {
        public Rule Push { get; private set; }
        public Rule Pop  { get; private set; }

        internal StackRule(Rule push, Rule pop)
        {
            this.Push = push;
            this.Pop  = pop;
        }
    }

}
