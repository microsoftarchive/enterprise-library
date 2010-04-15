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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls.Toolkit.Windows.Controls;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using System.Windows.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    /// <summary>
    /// The <see cref="DatePickerEditor"/> is an in-line editor that can be applied to a <see cref="Property"/>.
    /// </summary>
    /// <remarks>
    /// The editor displays a visual for selecting a date and updates a property value with the result.
    /// <br/>
    /// The editor can be applied by using the <see cref="EditorAttribute"/> on a property of
    /// the <see cref="System.Configuration.ConfigurationElement"/>.  
    /// </remarks>
    /// <seealso cref="CommonDesignTime.EditorTypes.DatePickerEditor"/>
    public class DatePickerEditor : DatePicker
    {  
        /// <summary>
        /// Initializes a new instance of <see cref="DatePickerEditor"/>.
        /// </summary>
        public DatePickerEditor ()
	    {
            base.DataContextChanged += new DependencyPropertyChangedEventHandler(DatePickerEditor_DataContextChanged);
        }

        void DatePickerEditor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var property = e.NewValue as BindableProperty;
            if (property != null)
            {
                CustomEditorBinder.BindProperty(this, property);

                var propertyBinding = new Binding("Value");
                propertyBinding.Source = property;
                propertyBinding.Converter = new DateTimeToNullableDateTimeConverter();
                this.SetBinding(DatePicker.SelectedDateProperty, propertyBinding);
            }
        }

        private class DateTimeToNullableDateTimeConverter : IValueConverter
        {
            #region IValueConverter Members

            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                DateTime? result = (((DateTime)value)== DateTime.MinValue) ? (DateTime?)null : (DateTime?)value;
                return result;
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if (value == null) return DateTime.MinValue;
                return (DateTime)value;
            }

            #endregion
        }
    }
}
