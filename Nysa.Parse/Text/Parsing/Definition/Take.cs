using Nysa.Text.Parsing.Definition;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Parse.Text.Parsing.Definition
{

    public class Take
    {


        // instance members
        public StringComparer SequenceDefault { get; private set; }

        public Take(StringComparer sequenceDefault)
        {
            this.SequenceDefault = sequenceDefault;
        }

        public Sequence This(String value) => new Sequence(value, this.SequenceDefault);

    }

}
