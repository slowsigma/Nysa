using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Text.Lexing
{

    public partial class Patterns
    {

        public partial class Factory
        {
            public delegate ExplicitCategory    CategoryFunction    (String symbol);
            public delegate EitherRule          EitherFunction      (params String[] choices);
            public delegate WhileRule           WhileFunction       (Rule rule);
            public delegate TakeRule            TakeFunction        (String sequence);
            public delegate MaybeRule           MaybeFunction       (Rule rule);
            public delegate UntilRule           UntilFunction       (Rule rule);
            public delegate Rule                OneOrMoreFunction   (EitherRule eitherRule);
            public delegate NotRule             NotFunction         (Rule rule);
            public delegate StackRule           StackFunction       (Rule pushRule, Rule popRule);

            // instance members
            public CategoryFunction Category    { get; private set; }
            public TakeFunction     Take        { get; private set; }
            public EitherFunction   Either      { get; private set; }
            public WhileFunction    While       { get; private set; }
            public MaybeFunction    Maybe       { get; private set; }
            public UntilFunction    Until       { get; private set; }
            public OneOrMoreFunction OneOrMore => either => either.Then(this.While(either));
            public NotFunction      Not         { get; private set; }
            public StackFunction    Stack       { get; private set; }

            private Dictionary<String, Rule> _Categories;

            public IEqualityComparer<String> Comparer { get; private set; }
            public Factory(IEqualityComparer<String> comparer)
            {
                this.Comparer = comparer;

                this._Categories = new Dictionary<String, Rule>(StringComparer.OrdinalIgnoreCase);

                this.Category   = symbol => new ExplicitCategory(symbol, this.AddCategory);
                this.Take       = v => new TakeRule(v);
                this.Either     = a => new EitherRule(a);
                this.While      = w => new WhileRule(w);
                this.Maybe      = w => new MaybeRule(w);
                this.Until      = w => new UntilRule(w);
                this.Not        = w => new NotRule(w);
                this.Stack      = (push, pop) => new StackRule(push, pop);
            }

            private void AddCategory(String symbol, Rule rule)
            {
                if (!Nysa.Text.Parsing.Grammar.IsCategorySymbol(symbol))
                    throw new ArgumentException($"Error in {symbol}. Category symbols must begin with {{, contain one alpha character followed by zero, one or more alpha, number, dash, or underscore characters and end with }}.", nameof(symbol));
                if (this._Categories.ContainsKey(symbol))
                    throw new InvalidOperationException($"Error in {symbol}. This category is already defined.");

                this._Categories.Add(symbol, rule);
            }

        } // class Factory

    }

}
