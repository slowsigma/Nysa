using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

using Nysa.Logics;

namespace Nysa.Windows.Markup
{

    [MarkupExtensionReturnType(typeof(PipeExtension))]
    public class PipeExtension : MarkupExtension, IValueConverter
    {
        private IValueConverter _First;
        private HashSet<Type>   _FirstTargetTypes;

        private IValueConverter _Second;
        private HashSet<Type>   _SecondSourceTypes;

        public PipeExtension()
        {
            this._First             = null;
            this._FirstTargetTypes  = new HashSet<Type>();

            this._Second            = null;
            this._SecondSourceTypes = new HashSet<Type>();
        }

        public PipeExtension(IValueConverter first, IValueConverter second)
            : this()
        {
            this.First  = first;
            this.Second = second;
        }

        [ConstructorArgument(@"first")]
        public IValueConverter First
        {
            get { return this._First; }
            set
            {
                this._First = value;

                this._FirstTargetTypes.Clear();

                if (this._First != null)
                {
                    TypeDescriptor.GetAttributes(this._First.GetType())
                                  .Cast<Object>()
                                  .Select(o => o as ValueConversionAttribute)
                                  .Where(a => a != null)
                                  .Affect(a =>
                                      {
                                          if (!this._FirstTargetTypes.Contains(a.TargetType)) this._FirstTargetTypes.Add(a.TargetType);
                                      });
                }
            }
        }

        [ConstructorArgument(@"second")]
        public IValueConverter Second
        {
            get { return this._Second; }
            set
            {
                this._Second = value;

                this._SecondSourceTypes.Clear();

                if (this._Second != null)
                {
                    TypeDescriptor.GetAttributes(this._Second.GetType())
                                  .Cast<Object>()
                                  .Select(o => o as ValueConversionAttribute)
                                  .Where(a => a != null)
                                  .Affect(a => 
                                      {
                                          if (!this._SecondSourceTypes.Contains(a.SourceType)) this._SecondSourceTypes.Add(a.SourceType);
                                      });
                }
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
            => this;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == Binding.DoNothing) return value;
            if (value == DependencyProperty.UnsetValue) return value;

            Type intermediateType = this._FirstTargetTypes.FirstOrDefault(t => this._SecondSourceTypes.Contains(t));

            return this._Second.Convert(this._First.Convert(value, intermediateType, parameter, culture), targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == Binding.DoNothing) return value;
            if (value == DependencyProperty.UnsetValue) return value;

            Type intermediateType = this._SecondSourceTypes.FirstOrDefault(s => this._FirstTargetTypes.Contains(s));

            return this._First.ConvertBack(this._Second.ConvertBack(value, intermediateType, parameter, culture), targetType, parameter, culture);
        }

    }

}
