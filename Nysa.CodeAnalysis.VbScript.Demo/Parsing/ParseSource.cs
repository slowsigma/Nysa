using System;
using System.Collections.Generic;
using System.Text;

using Nysa.Logics;

namespace Nysa.CodeAnalysis.VbScript.Demo
{

    public abstract class ParseSource
    {
        public abstract String Identifier { get; }

        protected Func<Suspect<String>> GetSource { get; private set; }

        protected ParseSource(Func<Suspect<String>> getSource)
        {
            this.GetSource = getSource;
        }

        public Suspect<String> Source
            => this.GetSource();
    }

}
