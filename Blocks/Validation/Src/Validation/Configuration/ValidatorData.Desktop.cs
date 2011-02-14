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
using System.ComponentModel;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    /// <summary>
    /// Base class for configuration objects describing validators.
    /// </summary>
    [ViewModel(ValidationDesignTime.ViewModelTypeNames.ValidatorDataViewModel)]
    partial class ValidatorData : NameTypeConfigurationElement
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ValidatorData"/> class.</para>
        /// </summary>
        public ValidatorData()
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ValidatorData"/> class with a name and a type.</para>
        /// </summary>
        /// <param name="name">The name for the instance.</param>
        /// <param name="validatorType">The type of the represented validator.</param>
        protected internal ValidatorData(string name, Type validatorType)
            : base(name, validatorType)
        { }

        private const string MessageTemplatePropertyName = "messageTemplate";
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
        [ConfigurationProperty(MessageTemplatePropertyName)]
        [Editor(CommonDesignTime.EditorTypes.MultilineText, CommonDesignTime.EditorTypes.FrameworkElement)]
        [ResourceDescription(typeof(DesignResources), "ValidatorDataMessageTemplateDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ValidatorDataMessageTemplateDisplayName")]
        public virtual string MessageTemplate
        {
            get { return (string)this[MessageTemplatePropertyName]; }
            set { this[MessageTemplatePropertyName] = value; }
        }

        private const string MessageTemplateResourceNamePropertyName = "messageTemplateResourceName";
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
        [ConfigurationProperty(MessageTemplateResourceNamePropertyName)]
        [ResourceDescription(typeof(DesignResources), "ValidatorDataMessageTemplateResourceNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ValidatorDataMessageTemplateResourceNameDisplayName")]
        [Category("CategoryLocalization")]
        public virtual string MessageTemplateResourceName
        {
            get { return (string)this[MessageTemplateResourceNamePropertyName]; }
            set { this[MessageTemplateResourceNamePropertyName] = value; }
        }

        private const string MessageTemplateResourceTypeNamePropertyName = "messageTemplateResourceType";
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
        [ConfigurationProperty(MessageTemplateResourceTypeNamePropertyName)]
        [ResourceDescription(typeof(DesignResources), "ValidatorDataMessageTemplateResourceTypeNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ValidatorDataMessageTemplateResourceTypeNameDisplayName")]
        [Category("CategoryLocalization")]
        [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
        [BaseType(typeof(object))]
        public virtual string MessageTemplateResourceTypeName
        {
            get { return (string)this[MessageTemplateResourceTypeNamePropertyName]; }
            set { this[MessageTemplateResourceTypeNamePropertyName] = value; }
        }

        private const string TagPropertyName = "tag";
        /// <summary>
        /// Gets or sets the tag that will characterize the results logged by the represented validator.
        /// </summary>
        [ConfigurationProperty(TagPropertyName)]
        [ResourceDescription(typeof(DesignResources), "ValidatorDataTagDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ValidatorDataTagDisplayName")]
        public virtual string Tag
        {
            get { return (string)this[TagPropertyName]; }
            set { this[TagPropertyName] = value; }
        }
    }
}
