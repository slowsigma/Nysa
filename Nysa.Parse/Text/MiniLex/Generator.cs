using System;
using System.Collections.Generic;
using System.Text;

namespace Lexington
{

    public static class Generator
    {
        // At any point in time, including the initial start condition, we have a
        // position and a string and a function that will match some portion of the
        // string at that position or will fail there.  The length of a match is
        // significant as well as the length of a miss.
        // The point of Generator is to provide a single optimized function in
        // support of the parser. The Grammar contains syntax rules and fixed
        // token values but not the lexical rules token categories in the Grammar.
        // Categories are any acceptable free-form names, labels, values, comments,
        // etc. allowed in sentences in the language the Grammar describes.
        //
        // Types of functions that comprise a full lexical analyzer:
        //   1. Top level - Repeat and yield tokens
        //      - Uses an array of functions indexed by the first character
        //        acceptable to the respective function.
        //              
        //  
        //
        // The Generator class create a tranformation function that produces a
        // series of tokens for any given string.  
        // string and produces a series of tokens
        // There are different functions, for example
        //   1. the start function - this function typically has some kind 
        //      a conglomerate function 

        // create a function that takes a position in the input string
        // and produced

        //private Func<String, Int32, (Boolean IsHit, Span<String> Slice,  >



        private static void SubNothing()
        {


            //var span = "some string of characters".AsSpan();
            
            //var current = span[0];
            
        }


    }

}
