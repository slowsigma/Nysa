using System;
using System.Collections.Generic;
using System.Text;

namespace Nysa.Logics
{
    
    public abstract class Suspect<T>
    {
        public static implicit operator Suspect<T>(Failed failed) => new Failed<T>(failed.Value);
    }

}
