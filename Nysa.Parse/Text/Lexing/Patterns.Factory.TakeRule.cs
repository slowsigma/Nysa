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

            public class TakeRule : Rule
            {
                public String Value { get; private set; }

                internal TakeRule(String value) => this.Value = value;
            }

        }

    }

}
