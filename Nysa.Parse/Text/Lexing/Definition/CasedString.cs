using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Text.Lexing.Definition
{

    public struct CasedString
    {
        public static implicit operator CasedString(String value)
            => new CasedString(value, false);

        // instance members
        public String  Value            { get; private set; }
        public Boolean IsInsensitive    { get; private set; }

        internal CasedString(String value, Boolean isInsensitive)
        {
            this.Value          = value ?? String.Empty;
            this.IsInsensitive  = isInsensitive;
        }

        public CasedString ToInsensitive()
            => new CasedString(this.Value, true);
    }

}
