using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nysa.Text
{

    public record struct Identifier
    {
        /// <summary>
        /// Used to indicate no identifier or that a token is invalid.
        /// </summary>
        public static readonly Identifier None = new Identifier(0);
        /// <summary>
        /// Used to indicate a token that is trivial or serves no purpose in some process.
        /// </summary>
        public static readonly Identifier Trivia = new Identifier(Int32.MinValue);

        public static Identifier FromNumber(Int32 number) => new Identifier(number);

        // instance members
        public Int32 Number { get; init; }
        private Identifier(Int32 number)
        {
            this.Number = number;
        }

        public Boolean IsNone { get => this.Number == 0; }
        public Boolean IsTrivia { get => this.Number == Int32.MinValue; }
    }

}
