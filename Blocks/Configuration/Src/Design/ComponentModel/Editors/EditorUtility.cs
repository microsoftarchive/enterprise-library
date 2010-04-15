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

using System.ComponentModel;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    /// <summary>
    /// The <see cref="EditorUtility"/> assist in locating and quierying <see cref="BindableProperty"/>
    /// from a <see cref="ITypeDescriptorContext"/>.
    /// </summary>
    /// <remarks>
    /// This utility class helps when creating custom editors that need to be displayed from within the Visual 
    /// Studio integrated editor.
    /// </remarks>
    public static class EditorUtility
    {
        ///<summary>
        /// Extracts the <see cref="BindableProperty"/> from a <see cref="ITypeDescriptorContext"/>.
        ///</summary>
        ///<param name="context">The context to extract the bindable property from.</param>
        ///<returns>
        /// Returns the <see cref="BindableProperty"/> or <see langword="null"/> if one could not be located.
        ///</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public static BindableProperty GetBindableProperty(ITypeDescriptorContext context)
        {
            Guard.ArgumentNotNull(context, "context");

            return context.PropertyDescriptor as BindableProperty;
        }

        ///<summary>
        /// Evaluates a bindable property to determine if the bindable is marked with <see cref="Common.Configuration.Design.EditorWithReadOnlyTextAttribute"/>.
        ///</summary>
        ///<param name="bindableProperty">The property to evaluate.</param>
        ///<returns>
        /// Returns <see langword="true"/> if the bindable indicates the text on an editor should only be editable from a popup editor.<br/>
        /// Otherwise, returns <see langword="false"/>.
        /// </returns>
        public static bool IsTextReadOnly(BindableProperty bindableProperty)
        {
            var textReadOnlyAttribute = bindableProperty.Attributes
                .OfType<Common.Configuration.Design.EditorWithReadOnlyTextAttribute>()
                .FirstOrDefault();

            if (textReadOnlyAttribute == null)
                return false;

            return textReadOnlyAttribute.ReadonlyText;
        }
    }
}
