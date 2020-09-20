using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Text.Lexing.Mini
{

    public delegate Category    CategoryFunction    (String symbol, Rule rule);
    public delegate AnyRule     AnyFunction         (params CasedString[] choices);
    public delegate WhileRule   WhileFunction       (Rule rule);
    public delegate TakeRule    TakeFunction        (CasedString sequence);
    public delegate MaybeRule   MaybeFunction       (Rule rule);
    public delegate UntilRule   UntilFunction       (Rule rule);
    public delegate Rule        OneOrMoreFunction   (AnyRule eitherRule);
    public delegate NotRule     NotFunction         (Rule rule);
    public delegate StackRule   StackFunction       (Rule pushRule, Rule popRule);

}
