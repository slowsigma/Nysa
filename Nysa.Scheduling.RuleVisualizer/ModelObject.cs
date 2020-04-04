using System;
using System.Collections.Generic;
using System.ComponentModel;

using Nysa.Logics;

namespace Nysa.ComponentModel
{

    public interface IModelObject : INotifyPropertyChanged
    {
        void OnPropertyChanged(PropertyChangedEventArgs e);
    } // interface IModelObject

    public static class IModelObjectExtensions
    {

        /// <summary>
        /// Provides an extension method for IModelObject to assist in updating the fields backing the public
        /// properties of the object and to call property change notification methods if a change is actually
        /// going to occur.
        /// </summary>
        /// <typeparam name="T">The type of the field being changed (implicitly determined by the compiler).</typeparam>
        /// <param name="modelObject">The IModelObject containing the propertyField and change notification methods.</param>
        /// <param name="propertyField">A reference to the field backing a property or properties.</param>
        /// <param name="newValue">The new value to test against and possibly set on the given propertyField.</param>
        /// <param name="changeNotifyPropertyNames">Zero, one, or more property names affected by the potential propertyField update.</param>
        /// <returns>True if the propertyField was actually updated to the newValue, otherwise false.</returns>
        public static Boolean UpdateObjectProperty<T>(this IModelObject modelObject, ref T propertyField, T newValue, params String[] changeNotifyPropertyNames)
            where T : class
        {
            if (propertyField != newValue)
            {
                propertyField = newValue;

                changeNotifyPropertyNames.Send(p => modelObject.OnPropertyChanged(new PropertyChangedEventArgs(p)));

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Provides an extension method for IModelObject to assist in updating the fields backing the public
        /// properties of the object and to call property change notification methods if a change is actually
        /// going to occur.
        /// </summary>
        /// <typeparam name="T">The type of the field being changed (implicitly determined by the compiler).</typeparam>
        /// <param name="modelObject">The IModelObject containing the propertyField and change notification methods.</param>
        /// <param name="propertyField">A reference to the field backing a property or properties.</param>
        /// <param name="newValue">The new value to test against and possibly set on the given propertyField.</param>
        /// <param name="changeNotifyPropertyNames">Zero, one, or more property names affected by the potential propertyField update.</param>
        /// <returns>True if the propertyField was actually updated to the newValue, otherwise false.</returns>
        public static Boolean UpdateValueProperty<T>(this IModelObject modelObject, ref T propertyField, T newValue, params String[] changeNotifyPropertyNames)
            where T : struct
        {
            if (!propertyField.Equals(newValue))
            {
                propertyField = newValue;

                changeNotifyPropertyNames.Send(p => modelObject.OnPropertyChanged(new PropertyChangedEventArgs(p)));

                return true;
            }
            else
            {
                return false;
            }
        }

    } // class IModelObjectExtensions

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

                this.PropertyChanged
                    .AsOption()
                    .Send(pc => changeNotifyPropertyNames.Send(p => pc(this, new PropertyChangedEventArgs(p))));

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

                this.PropertyChanged
                    .AsOption()
                    .Send(pc => changeNotifyPropertyNames.Send(p => pc(this, new PropertyChangedEventArgs(p))));

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

                this.PropertyChanged
                    .AsOption()
                    .Send(pc => changeNotifyPropertyNames.Send(p => pc(this, new PropertyChangedEventArgs(p))));

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

                this.PropertyChanged
                    .AsOption()
                    .Send(pc => changeNotifyPropertyNames.Send(p => pc(this, new PropertyChangedEventArgs(p))));

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
            => affectedPropertyNames.Send(p =>
            {
                this.PropertyChanged
                    .AsOption()
                    .Send(pc => pc(this, new PropertyChangedEventArgs(p)));
            });


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
                    this.PropertyChanged
                        .AsOption()
                        .Send(pc => affectedPropertyNames.Send(p => pc(this, new PropertyChangedEventArgs(p))));
                };

            constituents.Send(c => c.PropertyChanged += new PropertyChangedEventHandler(handler));
        }

        void IModelObject.OnPropertyChanged(PropertyChangedEventArgs e)
            => this.PropertyChanged
                   .AsOption()
                   .Send(pc => pc(this, e));
        

    } // class ModelObject

}
