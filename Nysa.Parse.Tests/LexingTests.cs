using System;
using System.Collections.Generic;
using System.Text;

using Xunit;

using Nysa.Logics;
using Nysa.Text.Lexing;

namespace Nysa.Parse.Tests
{

    public class LexingTests
    {
        [Fact]
        public void TestFactory()
        {
            var factory = new Patterns.Factory(StringComparer.OrdinalIgnoreCase);

            // the following "words" in a lexical analyzer factory are specifically
            // called out as words and not methods on objects for the following reasons
            //   1. They are all unary operators (they don't relate two thing together,
            //   2. The lexical expressions flow best with these words appearing prior to their data.

            var chars = factory;

            //var take        = factory.Take;
            //var either      = factory.Either;       // either("+-") as opposed to "+-".Either()
            //var @while      = factory.While;        // @while(numbers) as opposed to numbers.While()
            //var maybe       = factory.Maybe;        // maybe(either("+-")) as opposed to either("+-").Maybe()
            //var oneOrMore   = factory.OneOrMore;    // oneOrMore(digit) as opposed to digit.OneOrMore()
            //var not         = factory.Not;          // not(digit) as opposed to digit.Not()
            //var until       = factory.Until;        // until(digit) as opposed to digit.Until()

            var category    = factory.Category;

            var upperAlpha  = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var lowerAlpha  = @"abcdefghijklmnopqrstuvwxyz";

            var realNewLine = chars.Take("\r").Or(chars.Take("\n")).Or(chars.Take("\r\n"));
            var printable   = chars.Either(@"!", "\"", @"#$%&'()*+,-./0123456789:;<=>?@", upperAlpha, lowerAlpha, @"[\]^_`{|}~");
            var stringChar  = printable.Except("\"");
            var dateChar    = printable.Except("#");
            var idNameChar  = printable.Except("[]");
            var digit       = chars.Either("0123456789");
            var octDigit    = digit.Except("89");
            var hexDigit    = digit.Or("ABCDEF");

            var space       = chars.Either("\t\v", " ");
            var spacePlus   = chars.Take("_").Then(chars.While(space)).Then(realNewLine);

            var alpha       = chars.Either(upperAlpha, lowerAlpha);
            var digits      = chars.OneOrMore(digit);

            var numSign     = chars.Either("+-");
            var exponent    = chars.Take("E").Then(chars.Maybe(numSign)).Then(digits);
            var floatPeriod = chars.Take(".").Then(digits).Then(chars.Maybe(exponent));
            var floatFull   = digits.Then(floatPeriod);
            var floatExp    = digits.Then(exponent);

            var quote        = chars.Take("\"");
            var dblQuoteOrOther = chars.Take("\"\"").Or(chars.Not(quote));

            var idBaseOne   = alpha.Then(chars.While(digit.Or(alpha).Or("_")));
            var idBaseTwo   = chars.Take("[").Then(chars.While(idNameChar)).Then(chars.Take("]"));

            var commentOne  = chars.Take("'").Then(chars.Until(realNewLine));
            var commentTwo  = chars.Take("REM").Then(realNewLine.Or(space.Then(chars.While(space.Or(printable)))));

            category("{trivia}").Is(space.Or(spacePlus).Or(commentOne).Or(commentTwo));
            category("{literal-string}").Is(quote.Then(chars.While(dblQuoteOrOther)).Then(quote));
            category("{dateliteral}").Is(chars.Take("#").Then(chars.OneOrMore(dateChar)).Then(chars.Take("#")));
            category("{hexLiteral}").Is(chars.Take("&H").Then(chars.OneOrMore(hexDigit)).Then(chars.Maybe(chars.Take("&"))));
            category("{intLiteral}").Is(digits);
            category("{floatLiteral}").Is(floatPeriod.Or(floatFull).Or(floatExp));
            category("{octLiteral}").Is(chars.Take("&").Then(chars.OneOrMore(octDigit)).Then(chars.Maybe(chars.Take("&"))));
            category("{new-line}").Is(realNewLine);
            category("{id}").Is(idBaseOne.Or(idBaseTwo));
            category("{iddot}").Is(idBaseOne.Or(idBaseTwo).Then(chars.Take(".")));
            category("{dotid}").Is(chars.Take(".").Then(idBaseOne.Or(idBaseTwo)));
            category("{dotiddot}").Is(chars.Take(".").Then(idBaseOne.Or(idBaseTwo)).Then(chars.Take(".")));

        }
    }

}
