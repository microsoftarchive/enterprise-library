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

using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// A <see cref="BindableProperty"/> class that allow's the UI (User Interface) to interact with the underlying <see cref="Property"/>'s <see cref="Property.SuggestedValues"/> collection.
    /// </summary>
    /// <remarks>
    /// <see cref="Property"/> instances are shown in the designer as well in Visual Studio's property grid.<br/>
    /// Threrefore this <see cref="SuggestedValuesBindableProperty"/> class derives from <see cref="PropertyDescriptor"/>.
    /// </remarks>
    /// <seealso cref="BindableProperty"/>
    /// <seealso cref="Property"/>
    /// <seealso cref="Property.SuggestedValues"/>
    public class SuggestedValuesBindableProperty : BindableProperty
    {
        Property property;

        /// <summary>
        /// Initializes a new instance of <see cref="SuggestedValuesBindableProperty"/>.
        /// </summary>
        /// <param name="property">The <see cref="Design.ViewModel.Property"/> that should be displayed through a UI (User Interface).</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public SuggestedValuesBindableProperty(Property property)
            : base(property)
        {
            Guard.ArgumentNotNull(property, "property");

            this.property = property;
            this.property.PropertyChanged += PropertyPropertyChanged;
        }

        void PropertyPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SuggestedValues")
            {
                OnPropertyChanged("BindableSuggestedValues");
            }
        }

        /// <summary>
        /// Gets a <see cref="TypeConverter"/> implementation that can be used to interact with the underlying <see cref="Design.ViewModel.Property"/>.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="SuggestedValuesBindableProperty.SuggestedValuesBindablePropertyConverter"/> that is used to display the Value for the underlying <see cref="Property"/>.
        /// </returns>
        public override TypeConverter Converter
        {
            get
            {
                return new SuggestedValuesBindablePropertyConverter(property);
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="BindableProperty"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources. </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (property != null)
                {
                    property.PropertyChanged -= PropertyPropertyChanged;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets whether values other than the <see cref="BindableSuggestedValues"/> are allowed.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if values other than the <see cref="BindableSuggestedValues"/> are allowed.
        /// </value>
        public bool SuggestedValuesEditable
        {
            get { return !property.Converter.GetStandardValuesExclusive(property); }
        }

        /// <summary>
        /// Gets a list of suggested values to be displayed through the UI (User Interface).
        /// </summary>
        /// <value>
        /// A list of suggested values to be displayed through the UI (User Interface).
        /// </value>
        public IEnumerable<string> BindableSuggestedValues
        {
            get { return property.SuggestedValues.Select(x => property.Converter.ConvertToString(property, CultureInfo.CurrentCulture, x)); }
        }

        /// <summary>
        /// A <see cref="BindableProperty.BindablePropertyConverter"/> implementation that can be used to display and interpret a <see cref="Property"/> and its <see cref="Property.SuggestedValues"/>.
        /// </summary>
        private class SuggestedValuesBindablePropertyConverter : BindablePropertyConverter
        {
            Property property;

            /// <summary>
            /// Creates a new instance of <see cref="SuggestedValuesBindablePropertyConverter"/>.
            /// </summary>
            /// <param name="property">The <see cref="Property"/> this converter should be created for.</param>
            public SuggestedValuesBindablePropertyConverter(Property property)
                : base(property)
            {
                this.property = property;
            }

            /// <summary>
            /// Returns a collection of standard values for the data type this type converter is designed for when provided with a format context.
            /// </summary>
            /// <returns>
            /// The underlying <see cref="Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Property"/>'s <see cref="Property.SuggestedValues"/>.
            /// </returns>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context that can be used to extract additional information about the environment from which this converter is invoked. This parameter or properties of this parameter can be null. </param>
            public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                var suggestedValues = property.SuggestedValues.ToArray();
                return new StandardValuesCollection(suggestedValues);
            }

            /// <summary>
            /// Returns whether this object supports a standard set of values that can be picked from a list, using the specified context.
            /// </summary>
            /// <returns>
            /// <see langword="true"/> if the underlying <see cref="Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Property"/> has suggested values; Otherwise <see langword="false"/>;
            /// </returns>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context. </param>
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return property.HasSuggestedValues;
            }
        }
    }
}
