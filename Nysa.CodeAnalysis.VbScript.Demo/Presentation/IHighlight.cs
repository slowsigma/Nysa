using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nysa.ComponentModel;

public interface IHighlight<T> : IModelObject
{
    void Highlight(T item);
}