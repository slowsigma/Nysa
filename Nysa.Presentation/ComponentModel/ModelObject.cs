using System;
using System.Collections.Generic;
using System.ComponentModel;

using Nysa.Logics;

namespace Nysa.ComponentModel
{

    public abstract class ModelObject : INotifyPropertyChanged, IModelObject
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Provides a method to assist in updating the fields backing the public properties of
        /// the object and to raise property change notification events if a change is actually
        /// going to occur.
        /// </summary>
        /// <typeparam name="T">The type of the field being changed (implicitly determined by the compiler).</typeparam>
        /// <param name="propertyField">A reference to the field backing a property or properties.</param>
        /// <param name="newValue">The new value to test against and possibly set on the given propertyField.</param>
        /// <param name="changeNotifyPropertyNames">Zero, one, or more property names affected by the potential propertyField update.</param>
        /// <returns>True if the propertyField was actually updated to the newValue, otherwise false.</returns>
        protected Boolean UpdateObjectProperty<T>(ref T propertyField, T newValue, params String[] changeNotifyPropertyNames)
            where T : class
        {
            if (propertyField != newValue)
            {
                propertyField = newValue;

                if (this.PropertyChanged != null) changeNotifyPropertyNames.Affect(p => this.PropertyChanged(this, new PropertyChangedEventArgs(p)));

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Provides a method to assist in updating the fields backing the public properties of
        /// the object and to raise property change notification events if a change is actually
        /// going to occur.
        /// </summary>
        /// <typeparam name="T">The type of the field being changed (implicitly determined by the compiler).</typeparam>
        /// <param name="propertyField">A reference to the field backing a property or properties.</param>
        /// <param name="newValue">The new value to test against and possibly set on the given propertyField.</param>
        /// <param name="changeNotifyPropertyNames">Zero, one, or more property names affected by the potential propertyField update.</param>
        /// <returns>True if the propertyField was actually updated to the newValue, otherwise false.</returns>
        protected Boolean UpdateValueProperty<T>(ref T propertyField, T newValue, params String[] changeNotifyPropertyNames)
            where T : struct
        {
            if (!propertyField.Equals(newValue))
            {
                propertyField = newValue;

                if (this.PropertyChanged != null) changeNotifyPropertyNames.Affect(p => this.PropertyChanged(this, new PropertyChangedEventArgs(p)));

                return true;
            }
            else
            {
                return false;
            }
        }

        protected Boolean UpdateValueProperty<T>(ref Nullable<T> propertyField, Nullable<T> newValue, params String[] changeNotifyPropertyNames)
            where T : struct
        {
            if (!propertyField.Equals(newValue))
            {
                propertyField = newValue;

                if (this.PropertyChanged != null) changeNotifyPropertyNames.Affect(p => this.PropertyChanged(this, new PropertyChangedEventArgs(p)));

                return true;
            }
            else
            {
                return false;
            }
        }

        protected Boolean UpdateObject<T>(T value, T newValue, Action<T> setter, params String[] changeNotifyPropertyNames)
            where T : class
        {
            if (value != newValue)
            {
                setter(newValue);

                if (this.PropertyChanged != null) changeNotifyPropertyNames.Affect(p => this.PropertyChanged(this, new PropertyChangedEventArgs(p)));

                return true;
            }
            else
            {
                return false;
            }
        }

        protected Boolean UpdateValue<T>(T value, T newValue, Action<T> setter, params String[] changeNotifyPropertyNames)
            where T : struct
        {
            if (!value.Equals(newValue))
            {
                setter(newValue);

                if (this.PropertyChanged != null) changeNotifyPropertyNames.Affect(p => this.PropertyChanged(this, new PropertyChangedEventArgs(p)));

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Instructs the ModelObject to immediately notify that the affected properties have changed.
        /// </summary>
        /// <param name="affectedPropertyNames"></param>
        protected void NotifyChanged(params String[] affectedPropertyNames)
        {
            affectedPropertyNames.Affect(p =>
            {
                if (this.PropertyChanged != null) this.PropertyChanged(this, new PropertyChangedEventArgs(p));
            });
        }

        /// <summary>
        /// Intstructs the ModelObject to setup automatic change notification for a set of affected property names
        /// when any of the given constituent objects have changed.
        /// </summary>
        /// <param name="constituents"></param>
        /// <param name="affectedPropertyNames"></param>
        protected void NotifyChanged(IEnumerable<INotifyPropertyChanged> constituents, params String[] affectedPropertyNames)
        {
            Action<Object, PropertyChangedEventArgs> handler =
                (sender, e) =>
                {
                    if (this.PropertyChanged != null) affectedPropertyNames.Affect(p => this.PropertyChanged(this, new PropertyChangedEventArgs(p)));
                };

            constituents.Affect(c => c.PropertyChanged += new PropertyChangedEventHandler(handler));
        }

        void IModelObject.OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null) this.PropertyChanged(this, e);
        }

    }

}
