Notes on VbScript semantic analysis.

Given that we have scoped symbols (i.e., scope layer objects representing code frames),
along with their declarations (i.e., function, argument, variable, type, constant symbols),
can we work to tie all accessing expressions ()

The question is can we assign a likely declaration to each part of a PathExpression?

Example played out:
1. We see variable getting assigned a value from a function argument.
2. We see another variable used in the function call for the argument.
3. The function has two legs of an if where the argument is:
   3.1. Passed into a method call on a global variable (an object), or
   3.2. Is assigned a value from the result of a differnt function call.

Anything that can be PathExpression

What about PathExpression is complicated?
1. Because VbScript does not have function pointers, a base-symbol
   always points to: global method, global object, global variable,
   VbScript class name.


-- THIS TREE SEEMS WACKY (SLIGHTLY)
Symbol
  BaseSymbol
  BlockSymbol
  HardSymbol
    ClassSymbol
    TypedSymbol
      ArgumentSymbol
      MemberSymbol
        ConstantSymbol
        FunctionSymbol
          PropertyGetSymbol
          PropertySetSymbol
        RedimSymbol
        VariableSymbol
  PropertySymbol

- PropertySymbol seems like it should be inside member