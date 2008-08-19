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

using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF
{
    /// <summary>
    /// Represents a configuration element that specifies a message validation.
    /// </summary>
    public class ValidationBindingElementExtension : BindingElementExtensionElement
    {
        /// <summary>
        /// When overridden in a derived class, gets the <see cref="T:System.Type"></see> object that represents the custom binding element.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type"></see> object that represents the custom binding type.</returns>
        public override Type BindingElementType
        {
            get { return typeof(ValidationBindingElement); }
        }

        /// <summary>
        /// When overridden in a derived class, returns a custom binding element object.
        /// </summary>
        /// <returns>
        /// A custom <see cref="T:System.ServiceModel.Channels.BindingElement"></see> object.
        /// </returns>
        protected override BindingElement CreateBindingElement()
        {
            return new ValidationBindingElement();
        }
    }
}