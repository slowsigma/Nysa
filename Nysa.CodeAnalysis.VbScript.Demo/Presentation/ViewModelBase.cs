using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.ComponentModel;

namespace Nysa.CodeAnalysis.VbScript.Demo
{

    public abstract partial class ViewModelBase<TParent> : IModelObject
        where TParent : IModelObject
    {
        public TParent Parent { get; private set; }

        protected ViewModelBase(TParent parent) { this.Parent = parent; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }

}
