using System;

namespace Nysa.IO
{

    public record TextContent
    {
        public String Source { get; private set; }
        public String Hash   { get; private set; }
        public String Value  { get; private set; }

        public TextContent(String source, String hash, String value)
        {
            this.Source = source;
            this.Hash   = hash;
            this.Value  = value;
        }
    }

}