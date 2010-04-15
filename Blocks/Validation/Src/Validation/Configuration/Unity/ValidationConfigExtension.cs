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

using Microsoft.Practices.Unity.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Unity
{
    /// <summary>
    /// A configuration section extension that adds the validator element
    /// to the Unity configuration schema.
    /// </summary>
    public class ValidationConfigExtension : SectionExtension
    {
        /// <summary>
        /// Add alaises and elements to the unity section configuration schema.
        /// </summary>
        /// <param name="context">Context to make the add calls on.</param>
        public override void AddExtensions(SectionExtensionContext context)
        {
            context.AddElement<ValidationSourceElement>("validator");
        }
    }
}
