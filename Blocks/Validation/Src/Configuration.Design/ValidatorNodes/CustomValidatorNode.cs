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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes
{
    /// <summary>
    /// Respresents the designtime configuration node for an <see cref="CustomValidatorData"/>.
    /// </summary>
    public class CustomValidatorNode: SingleValidatorNodeBase
    {
        private string customValidatorTypeName;
        private List<EditableKeyValue> editableAttributes = new List<EditableKeyValue>();		        

        /// <summary>
        /// Creates an instance of <see cref="CustomValidatorNode"/> based on default values.
        /// </summary>
        public CustomValidatorNode()
            :this(new CustomValidatorData(Resources.CustomValidatorNodeName, string.Empty))
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="CustomValidatorData"/> based on runtime configuration data.
        /// </summary>
        /// <param name="validatorData">The corresponding runtime configuration data.</param>
        public CustomValidatorNode(CustomValidatorData validatorData)
            : base(validatorData)
        {
            if (null == validatorData) throw new ArgumentNullException("validatorData");

            foreach (string key in validatorData.Attributes)
            {
                editableAttributes.Add(new EditableKeyValue(key, validatorData.Attributes[key]));
            }

            customValidatorTypeName = validatorData.TypeName;
        }

        /// <summary>
        /// Gets or sets the custom attributes for the provider.
        /// </summary>
        /// <value>
        /// The custom attributes for the provider.
        /// </value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("CustomValidatorProviderExtensionsDescription", typeof(Resources))]
        [CustomAttributesValidation]
        public List<EditableKeyValue> Attributes
        {
            get { return editableAttributes; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of the custom provider.
        /// </summary>
        /// <value>
        /// The <see cref="Type"/> of the custom provider.
        /// </value>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(Validator), typeof(CustomValidatorData))]
        [SRDescription("ProviderTypeNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Type
        {
            get { return customValidatorTypeName; }
            set { customValidatorTypeName = value; }
        }

        /// <summary>
        /// Returns the runtime configuration data that is represented by this node.
        /// </summary>
        /// <returns>An instance of <see cref="CustomValidatorData"/> that can be persisted to a configuration file.</returns>
        public override ValidatorData CreateValidatorData()
        {
            CustomValidatorData customValidatorData = new CustomValidatorData(Name, customValidatorTypeName);

            SetValidatorBaseProperties(customValidatorData);
            foreach (EditableKeyValue kv in editableAttributes)
            {
                customValidatorData.Attributes.Add(kv.Key, kv.Value);
            }

            return customValidatorData;
        }
    }
}
