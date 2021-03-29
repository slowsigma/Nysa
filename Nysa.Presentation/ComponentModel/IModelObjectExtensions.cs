using System;
using System.Collections.Generic;
using System.ComponentModel;

using Nysa.Logics;

namespace Nysa.ComponentModel
{

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

                changeNotifyPropertyNames.Affect(p => modelObject.OnPropertyChanged(new PropertyChangedEventArgs(p)));

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

                changeNotifyPropertyNames.Affect(p => modelObject.OnPropertyChanged(new PropertyChangedEventArgs(p)));

                return true;
            }
            else
            {
                return false;
            }
        }

    }

}
