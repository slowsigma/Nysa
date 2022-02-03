using System;

using Nysa.Logics;

namespace Nysa.CodeAnalysis.VbScript.Semantics
{

    public class Condition
    {
        public Func<TransformContext, TransformItem[], Boolean> Predicate { get; private set; }
        public Transform WhenTrue { get; private set; }
        public Transform WhenFalse { get; private set; }

        public Transform ToTransform()
            => (c, m) => this.Predicate(c, m) ? this.WhenTrue(c, m) : this.WhenFalse(c, m);

        public Condition(Func<TransformContext, TransformItem[], Boolean> predicate, Transform whenTrue, Transform whenFalse)
        {
            this.Predicate = predicate;
            this.WhenTrue  = whenTrue;
            this.WhenFalse = whenFalse;
        }
    }

}