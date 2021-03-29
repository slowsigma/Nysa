using System;

namespace Nysa.ComponentModel
{

    public class DocumentedValue
    {
        public String Expression    { get; private set; }
        public String Meaning       { get; private set; }

        public DocumentedValue(String expression)
            : this(expression, String.Empty)
        {
        }

        public DocumentedValue(String expression, String meaning)
        {
            this.Expression = (expression   == null ? String.Empty : expression);
            this.Meaning    = (meaning      == null ? String.Empty : meaning);
        }

    }

}
