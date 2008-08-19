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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Drawing.Design;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    /// <summary>
    /// Represents a <see cref="CustomSecurityCacheProviderData"/> configuration element.
    /// </summary>
    public sealed class CustomSecurityCacheProviderNode : SecurityCacheProviderNode
    {
        private string customSeurityCacheProviderTypeName;
        private List<EditableKeyValue> editableAttributes = new List<EditableKeyValue>();		        

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomSecurityCacheProviderNode"/> class.
        /// </summary>
        public CustomSecurityCacheProviderNode() 
            : this(new CustomSecurityCacheProviderData(Resources.CustomSecurityCacheNodeDefaultName, string.Empty))
        {
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="CustomSecurityCacheProviderNode"/> class with a <see cref="CustomSecurityCacheProviderData"/> instance.
		/// </summary>
		/// <param name="customSecurityCacheProviderData">A <see cref="CustomSecurityCacheProviderData"/> instance</param>
        public CustomSecurityCacheProviderNode(CustomSecurityCacheProviderData customSecurityCacheProviderData) : base()
        {
			if (null == customSecurityCacheProviderData) throw new ArgumentNullException("customSecurityCacheProviderData");

            foreach (string key in customSecurityCacheProviderData.Attributes)
            {
                editableAttributes.Add(new EditableKeyValue(key, customSecurityCacheProviderData.Attributes[key]));
            }

            customSeurityCacheProviderTypeName = customSecurityCacheProviderData.TypeName;
			Rename(customSecurityCacheProviderData.Name);
        }

		/// <summary>
		/// Gets or sets the custom attributes for the provider.
		/// </summary>
		/// <value>
		/// The custom attributes for the provider.
		/// </value>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("CustomAuthorizationProviderExtensionsDescription", typeof(Resources))]		
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
        [BaseType(typeof(ISecurityCacheProvider), typeof(CustomSecurityCacheProviderData))]
        [SRDescription("ProviderTypeNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Type
        {
            get { return customSeurityCacheProviderTypeName; }
            set { customSeurityCacheProviderTypeName = value; }
        }

        /// <summary>
        /// Gets the <see cref="CustomSecurityCacheProviderData"/> this node represents.
        /// </summary>
		/// <value>
		/// The <see cref="CustomSecurityCacheProviderData"/> this node represents.
		/// </value>
        [ReadOnly(true)]
        public override SecurityCacheProviderData SecurityCacheProviderData
        {
            get
            {
				CustomSecurityCacheProviderData customCacheProvider = new CustomSecurityCacheProviderData(Name, customSeurityCacheProviderTypeName);                
                foreach (EditableKeyValue kv in editableAttributes)
                {
                    customCacheProvider.Attributes.Add(kv.Key, kv.Value);
                }
                return customCacheProvider;
            }
        }
    }
}