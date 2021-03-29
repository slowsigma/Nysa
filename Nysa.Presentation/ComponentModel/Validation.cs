using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.ComponentModel
{

    public class Validation
    {
        public enum Level
        {
            valid = 0,
            warning = 1,
            invalid = 2
        }

        // instance members
        public Func<Level> GetLevel     { get; private set; }
        public Func<String> GetMessage  { get; private set; }

        public Validation(Func<Level> getLevel, Func<String> getMessage)
        {
            this.GetLevel   = getLevel;
            this.GetMessage = getMessage;
        }
    }

}
