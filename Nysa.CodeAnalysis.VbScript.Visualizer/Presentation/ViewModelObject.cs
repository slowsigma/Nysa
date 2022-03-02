using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.ComponentModel;

namespace Nysa.CodeAnalysis.VbScript.Visualizer
{

    public abstract partial class ViewModelObject<TParent> : ModelObject
        where TParent : ModelObject
    {
        public TParent Parent { get; private set; }

        protected ViewModelObject(TParent parent) { this.Parent = parent; }
    }

}
