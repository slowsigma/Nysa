using System;

using Nysa.ComponentModel;

namespace Nysa.Scheduling.RuleVisualizer
{

    public abstract partial class ViewModelObject<TParent> : ModelObject
        where TParent : ModelObject
    {
        public TParent Parent { get; private set; }

        protected ViewModelObject(TParent parent) { this.Parent = parent; }
    }

}
