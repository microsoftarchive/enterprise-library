//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Properties;
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Provides designtime configuration for <see cref="CustomSymmetricCryptoProviderData"/>.
    /// </summary>
    public class CustomSymmetricCryptoProviderNode : SymmetricCryptoProviderNode
    {
        private string typeName;
        private List<EditableKeyValue> editableAttributes = new List<EditableKeyValue>();

        /// <summary>
        /// Initializes with default configuration.
        /// </summary>
        public CustomSymmetricCryptoProviderNode() : this(new CustomSymmetricCryptoProviderData(Resources.CustomSymmetricCryptoProviderNodeName, (string)null))
        {
        }

        /// <summary>
        /// Constructs a new instance 
        /// with the corresponding runtime configuration data.
        /// </summary>
        /// <param name="customSymmetricCryptoProviderData">The corresponding runtime configuration data.</param>
        public CustomSymmetricCryptoProviderNode(CustomSymmetricCryptoProviderData customSymmetricCryptoProviderData) : base(customSymmetricCryptoProviderData)
        {
            Rename(customSymmetricCryptoProviderData.Name);
            foreach (string key in customSymmetricCryptoProviderData.Attributes)
            {
                editableAttributes.Add(new EditableKeyValue(key, customSymmetricCryptoProviderData.Attributes[key]));
            }
            this.typeName = customSymmetricCryptoProviderData.TypeName;
        }

        /// <summary>
        /// Gets or sets fully qualified assembly name of the <see cref="ISymmetricCryptoProvider"/>.
        /// </summary>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(ISymmetricCryptoProvider), typeof(CustomSymmetricCryptoProviderData))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("CustomSymmetricCryptoProviderTypeNameDescription", typeof(Resources))]
        public string Type
        {
            get { return typeName; }
            set { typeName = value; }
        }

		/// <summary>
		/// Gets or sets the attributes for the custom cache storage.
		/// </summary>
		/// <value>
		/// The attributes for the custom cache storage.
		/// </value>	
		[SRDescription("CustomSymmetricCryptosProviderNodeExtensionsDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public List<EditableKeyValue> Attributes
		{
			get { return editableAttributes; }
		}

		/// <summary>
        /// Gets a <see cref="CustomSymmetricCryptoProviderData"/> configuration object using the node data.
        /// </summary>
		/// <value>
        /// A <see cref="CustomSymmetricCryptoProviderData"/> configuration object using the node data.
		/// </value>
        public override SymmetricProviderData SymmetricCryptoProviderData
        {
            get
            {
                CustomSymmetricCryptoProviderData customSymmetricCryptoProvider = new CustomSymmetricCryptoProviderData(Name, typeName);
                customSymmetricCryptoProvider.Attributes.Clear();
                foreach (EditableKeyValue kv in editableAttributes)
                {
                    customSymmetricCryptoProvider.Attributes.Add(kv.Key, kv.Value);
                }
                return customSymmetricCryptoProvider;
            }
        }
    }
}
