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
using System.ComponentModel;
using System.Configuration;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class ConnectionStringPropertyViewModel : ElementProperty
    {
        public ConnectionStringPropertyViewModel(IServiceProvider serviceProvider, ElementViewModel parent, PropertyDescriptor declaringProperty) 
            : base(serviceProvider, parent, declaringProperty)
        {
        }

        public override TypeConverter Converter
        {
            get
            {
                return new ConnectionStringConverter();
            }
        }

        protected override IEnumerable<Validator> GetDefaultPropertyValidators()
        {
            yield return new ConnectionStringValidator();
        }

        private class ConnectionStringValidator : PropertyValidator
        {
            protected override void ValidateCore(Property property, string value, IList<ValidationResult> errors)
            {
                // We know the underlying value will always be a string since it's custom around connection string.
                if (string.IsNullOrEmpty((string)property.Value))
                {
                    errors.Add(new PropertyValidationResult(property, Resources.ValidationErrorConnectionStringRequired));
                }
            }
        }

        private class ConnectionStringConverter : StringConverter
        {
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                return value;
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                return Resources.ConnectionStringHiddenValue;
            }
        }

    }

#pragma warning restore 1591
}
