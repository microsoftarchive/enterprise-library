//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.Properties;
using System.Collections.Specialized;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.ComponentModel;
using System.Drawing.Design;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    /// <summary>
	/// Represents a <see cref="CustomAuthorizationProviderData"/> configuration element.
    /// </summary>
    public sealed class CustomAuthorizationProviderNode : AuthorizationProviderNode
    {
        private string customAuthoricationProviderTypeName;
        private List<EditableKeyValue> editableAttributes = new List<EditableKeyValue>();

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomAuthorizationProviderNode"/> class.
        /// </summary>
        public CustomAuthorizationProviderNode()
            : this(new CustomAuthorizationProviderData(Resources.CustomAuthorizationProviderCommandName, string.Empty))
        {
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="CustomAuthorizationProviderNode"/> class with a <see cref="CustomAuthorizationProviderData"/> instance.
		/// </summary>
		/// <param name="customAuthorizationProviderData">A <see cref="CustomAuthorizationProviderData"/> instance</param>
        public CustomAuthorizationProviderNode(CustomAuthorizationProviderData customAuthorizationProviderData) 
            : base()
        {
			if (null == customAuthorizationProviderData) throw new ArgumentNullException("customAuthorizationProviderData");

            this.customAuthoricationProviderTypeName = customAuthorizationProviderData.TypeName;
            foreach (string key in customAuthorizationProviderData.Attributes)
            {
                editableAttributes.Add(new EditableKeyValue(key, customAuthorizationProviderData.Attributes[key]));
            }

			Rename(customAuthorizationProviderData.Name);
        }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of the custom provider.
        /// </summary>
		/// <value>
		/// The <see cref="Type"/> of the custom provider.
		/// </value>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(IAuthorizationProvider), typeof(CustomAuthorizationProviderData))]
        [SRDescription("ProviderTypeNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Type
        {
            get { return customAuthoricationProviderTypeName; }
            set { customAuthoricationProviderTypeName = value; }
        }

        /// <summary>
        /// Gets or sets the custom attributes for the provider.
        /// </summary>
		/// <value>
		/// The custom attributes for the provider.
		/// </value>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("CustomAuthorizationProviderExtensionsDescription", typeof(Resources))]		
        [CustomAttributesValidation]
        public List<EditableKeyValue> Attributes
        {
            get
            {
                return editableAttributes;
            }
        }

        /// <summary>
        /// Gets the <see cref="CustomAuthorizationProviderData"/> this node represents.
        /// </summary>
		/// <value>
		/// The <see cref="CustomAuthorizationProviderData"/> this node represents.
		/// </value>
        public override AuthorizationProviderData AuthorizationProviderData
        {
            get
            {
				CustomAuthorizationProviderData customAuthData = new CustomAuthorizationProviderData(Name, customAuthoricationProviderTypeName);                
                foreach (EditableKeyValue kv in editableAttributes)
                {
                    customAuthData.Attributes.Add(kv.Key, kv.Value);
                }
                return customAuthData;
            }
        }
    }
}