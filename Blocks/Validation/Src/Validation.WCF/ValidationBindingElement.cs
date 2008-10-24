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

using System.ServiceModel.Channels;
using System.Xml;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF
{
    /// <summary>
    /// Represents the binding element that contains the message validation policy.
    /// </summary>
    public class ValidationBindingElement : BindingElement
    {
        private static XmlDocument doc = new XmlDocument();

        /// <summary>
        /// When overridden in a derived class, returns a copy of the binding element object.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.Channels.BindingElement"></see> object that is a deep clone of the original.
        /// </returns>
        public override BindingElement Clone()
        {
            return this; 
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override T GetProperty<T>(BindingContext context)
        {
            return context.GetInnerProperty<T>();
        }
    }
}
