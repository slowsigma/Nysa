Notes on VbScript semantic analysis.

Given that we have scoped symbols (i.e., scope layer objects representing code frames),
along with their declarations (i.e., function, argument, variable, type, constant symbols),
can we work to tie all accessing expressions ()

The question is can we assign a likely declaration to each part of a PathExpression?

Example played out:
1. We see variable getting assigned an value from a function argument.
2. We see another variable used in the function call for the argument.
3. The function has two legs of an if where the argument is:
   3.1. Passed into a method call on a global variable (an object), or
   3.2. Is assigned a value from the result of a differnt function call.

Anything that can be PathExpression