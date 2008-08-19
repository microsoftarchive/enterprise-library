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
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Properties;
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Provides designtime configuration for <see cref="CustomHashProviderData"/>.
    /// </summary>
    public class CustomHashProviderNode : HashProviderNode
    {
        private string typeName;
        private List<EditableKeyValue> editableAttributes = new List<EditableKeyValue>();
        private CustomHashProviderData customHashProviderData;

        /// <summary>
        /// Initializes with default configuration.
        /// </summary>
        public CustomHashProviderNode() : this(new CustomHashProviderData())
        {
            Name = Resources.CustomHashProviderNodeName;
        }

        /// <summary>
        /// Constructs a new instance 
        /// with the corresponding runtime configuration data.
        /// </summary>
        /// <param name="customHashProviderData">The corresponding runtime configuration data.</param>
        public CustomHashProviderNode(CustomHashProviderData customHashProviderData) : base(customHashProviderData)
        {
            this.customHashProviderData = customHashProviderData;

            Rename(customHashProviderData.Name);
            foreach (string key in customHashProviderData.Attributes)
            {
                editableAttributes.Add(new EditableKeyValue(key, customHashProviderData.Attributes[key]));
            }
            this.typeName = customHashProviderData.TypeName;
        }

        /// <summary>
        /// Gets or sets fully qualified assembly name of the <see cref="IHashProvider"/>.
        /// </summary>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(IHashProvider), typeof(CustomHashProviderData))]
        [SRDescription("HashProviderTypeNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
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
        [SRDescription("CustomHashProviderNodeExtensionsDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public List<EditableKeyValue> Attributes
        {
            get { return editableAttributes; }
        }

        /// <summary>
        /// Gets a <see cref="CustomHashProviderData"/> configuration object using the node data.
        /// </summary>
        /// <value>
        /// A <see cref="CustomHashProviderData"/> configuration object using the node data.
        /// </value>
        public override HashProviderData HashProviderData
        {
            get
            {
                CustomHashProviderData customHashProviderData = new CustomHashProviderData(Name, typeName);
                customHashProviderData.Attributes.Clear();
                foreach (EditableKeyValue kv in editableAttributes)
                {
                    customHashProviderData.Attributes.Add(kv.Key, kv.Value);
                }
                return customHashProviderData;
            }
        }
    }
}