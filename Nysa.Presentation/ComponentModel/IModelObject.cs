using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Nysa.ComponentModel
{

    public interface IModelObject : INotifyPropertyChanged
    {
        void OnPropertyChanged(PropertyChangedEventArgs e);
    }

}
