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

using System.Windows;
using System.Windows.Data;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Converters;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    /// <summary>
    /// Utility class for custom editors to establish bindings between <see cref="FrameworkElement"/> and a <see cref="BindableProperty"/> 
    /// <br/>
    /// This is used by the design-time infrastructure and is not intended to be used directly from your code.
    /// </summary>
    public static class CustomEditorBinder
    {
        ///<summary>
        /// Creates a binding between a <see cref="UIElement.IsEnabledProperty"/> and
        /// the <see cref="BindableProperty.ReadOnly"/> value.
        ///</summary>
        ///<param name="element">The visual element to bind to.</param>
        ///<param name="property">The property to bind with.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public static void BindProperty(FrameworkElement element, BindableProperty property)
        {
            Guard.ArgumentNotNull(element, "element");

            var enabledBinding = new Binding("ReadOnly");
            enabledBinding.Converter = new BooleanInverseConverter();
            enabledBinding.Source = property;
            element.SetBinding(UIElement.IsEnabledProperty, enabledBinding);

        }
    }
}
