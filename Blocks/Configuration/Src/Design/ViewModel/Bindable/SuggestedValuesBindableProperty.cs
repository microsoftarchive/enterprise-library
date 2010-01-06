//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    public class SuggestedValuesBindableProperty : BindableProperty
    {
        Property property;

        public SuggestedValuesBindableProperty(Property property)
            :base(property)
        {
            this.property = property;
            this.property.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(property_PropertyChanged);
        }

        void property_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SuggestedValues")
            {
                OnPropertyChanged("BindableSuggestedValues");
            }
        }

        public bool SuggestedValuesEditable
        {
            get { return !property.Converter.GetStandardValuesExclusive(property); }
        }

        public IEnumerable<string> BindableSuggestedValues
        {
            get { return property.SuggestedValues.Select(x => property.Converter.ConvertToString(property, CultureInfo.CurrentUICulture, x)); }
        }


        public override System.ComponentModel.TypeConverter Converter
        {
            get
            {
                return new SuggestedValuesTypeConverter(this);   
            }
        }


        private class SuggestedValuesTypeConverter : BindablePropertyConverter
        {
            SuggestedValuesBindableProperty bindable;
            
            public SuggestedValuesTypeConverter(SuggestedValuesBindableProperty bindable)
                :base(bindable.property)
            {
                this.bindable = bindable;
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(bindable.BindableSuggestedValues.ToArray());
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return !bindable.SuggestedValuesEditable;
            }
        }
    }
}
