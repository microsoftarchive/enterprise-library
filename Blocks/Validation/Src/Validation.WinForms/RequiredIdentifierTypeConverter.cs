//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms
{
    /// <summary>
    /// Provides a type converter to convert string objects to and from other representations and validates them.
    /// </summary>
    public class RequiredIdentifierConverter : StringConverter
    {
        ///<summary>
        ///Returns whether the given value object is valid for this type and for the specified context.
        ///</summary>
        ///
        ///<returns>
        ///true if the specified value is valid for this object; otherwise, false.
        ///</returns>
        ///
        ///<param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context. </param>
        ///<param name="value">The <see cref="T:System.Object"></see> to test for validity. </param>
        public override bool IsValid(ITypeDescriptorContext context,
                                     object value)
        {
            string stringValue = value as string;
            if (stringValue == null)
                return false;

            return stringValue.Length > 0;
        }
    }
}
