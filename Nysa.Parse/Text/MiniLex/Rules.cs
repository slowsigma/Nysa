using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nysa.Text.Lexing.Mini
{

    public sealed class Rules
    {
        public static TakeFunction      Take        = v => new TakeRule(v, (1, 1));
        public static AnyFunction       Any         = a => new AnyRule(a);
        public static WhileFunction     While       = w => new WhileRule(w);
        public static UntilFunction     Until       = w => new UntilRule(w);
        public static MaybeFunction     Maybe       = w => new MaybeRule(w);
        public static OneOrMoreFunction OneOrMore   = e => e.Then(While(e));
        public static NotFunction       Not         = w => new NotRule(w);
        public static StackFunction     Stack       = (push, pop) => new StackRule(push, pop);
        public static CategoryFunction  Category    = (symbol, rule) => new Category(symbol, rule);

        // instance members
        private Dictionary<String, Rule> _Categories;

        public Rules(params Category[] categories)
        {
            this._Categories    = new Dictionary<String, Rule>(StringComparer.OrdinalIgnoreCase);
        }

        public Rules(IEnumerable<Category> categories)
        {

        }

        private void AddCategory(String symbol, Rule rule)
        {
            if (this._Categories.ContainsKey(symbol))
                throw new InvalidOperationException($"Error in {symbol}. This category is already defined.");

            this._Categories.Add(symbol, rule);
        }

    } // class Factory

}
