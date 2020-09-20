using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Text.Lexing.Mini
{

    public class Categories : Dictionary<String, Category>
    {

        public void Add(Category category)
            => this.Add(category.Symbol, category);

        public void Add(String symbol, Rule rule)
            => this.Add(new Category(symbol, rule));

    }

}
