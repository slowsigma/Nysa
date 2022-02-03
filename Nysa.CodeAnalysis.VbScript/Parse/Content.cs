using System;

namespace CodeAnalysis.VbScript
{

    public abstract class Content
    {
        public String Source { get; private set; }
        public String Value  { get; private set; }

        protected Content(String source, String value)
        {
            this.Source = source;
            this.Value  = value;
        }
    }

}