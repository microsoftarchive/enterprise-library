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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    partial class ValidatorData : NamedConfigurationElement
    {
        /// <summary>
        /// Gets or sets the message template to use when logging validation results.
        /// </summary>
        /// <remarks>
        /// Either the <see cref="ValidatorData.MessageTemplate"/> or the 
        /// pair <see cref="ValidatorData.MessageTemplateResourceName"/> 
        /// and <see cref="ValidatorData.MessageTemplateResourceTypeName"/> can be used to 
        /// provide a message template for the represented validator.
        /// <para/>
        /// If both the template and the resource reference are specified, the template will be used.
        /// </remarks>
        /// <seealso cref="ValidatorData.MessageTemplateResourceName"/> 
        /// <seealso cref="ValidatorData.MessageTemplateResourceTypeName"/>
        public virtual string MessageTemplate { get; set; }

        /// <summary>
        /// Gets or sets the name of the resource to retrieve the message template to use when logging validation results.
        /// </summary>
        /// <remarks>
        /// Used in combination with <see cref="ValidatorData.MessageTemplateResourceTypeName"/>.
        /// <para/>
        /// Either the <see cref="ValidatorData.MessageTemplate"/> or the 
        /// pair <see cref="ValidatorData.MessageTemplateResourceName"/> 
        /// and <see cref="ValidatorData.MessageTemplateResourceTypeName"/> can be used to 
        /// provide a message template for the represented validator.
        /// <para/>
        /// If both the template and the resource reference are specified, the template will be used.
        /// </remarks>
        /// <seealso cref="ValidatorData.MessageTemplate"/> 
        /// <seealso cref="ValidatorData.MessageTemplateResourceTypeName"/>
        public virtual string MessageTemplateResourceName { get; set; }

        /// <summary>
        /// Gets or sets the name of the type to retrieve the message template to use when logging validation results.
        /// </summary>
        /// <remarks>
        /// Used in combination with <see cref="ValidatorData.MessageTemplateResourceName"/>.
        /// <para/>
        /// Either the <see cref="ValidatorData.MessageTemplate"/> or the 
        /// pair <see cref="ValidatorData.MessageTemplate"/> 
        /// and <see cref="ValidatorData.MessageTemplateResourceTypeName"/> can be used to 
        /// provide a message template for the represented validator.
        /// <para/>
        /// If both the template and the resource reference are specified, the template will be used.
        /// </remarks>
        /// <seealso cref="ValidatorData.MessageTemplate"/> 
        /// <seealso cref="ValidatorData.MessageTemplateResourceName"/>
        public virtual string MessageTemplateResourceTypeName { get; set; }

        /// <summary>
        /// Gets or sets the tag that will characterize the results logged by the represented validator.
        /// </summary>
        public virtual string Tag { get; set; }
    }
}
