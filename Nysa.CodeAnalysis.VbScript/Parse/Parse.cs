using System;

namespace Nysa.CodeAnalysis.VbScript
{

    public abstract record Parse
    {
        public DateTime Created { get; init; }

        protected Parse(DateTime? created = null)
        {
            this.Created = created ?? DateTime.UtcNow;
        }
    }

}