//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{
    /// <summary>
    /// Represents a custom cache storage that consists of key value pairs. 
    /// </summary>
    public class CustomCacheStorageNode : CacheStorageNode
    {
        private List<EditableKeyValue> editableAttributes = new List<EditableKeyValue>();
		private string typeName;

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomCacheStorageNode"/> class.
        /// </summary>
        public CustomCacheStorageNode()
            : this(new CustomCacheStorageData(Resources.DefaultCacheStorageNodeName, string.Empty))
        {
        }

        /// <summary>
		/// Initialize a new instance of the <see cref="CustomCacheStorageNode"/> class with a <see cref="CustomCacheStorageData"/> object.
        /// </summary>
        /// <param name="customCacheStorageData">The <see cref="CustomCacheStorageData"/> to display.</param>
        public CustomCacheStorageNode(CustomCacheStorageData customCacheStorageData)
        {			
			if (null == customCacheStorageData) throw new ArgumentNullException("customCacheStorageData");

			Rename(customCacheStorageData.Name);
            foreach (string key in customCacheStorageData.Attributes)
            {
                editableAttributes.Add(new EditableKeyValue(key, customCacheStorageData.Attributes[key]));
            }
            this.typeName = customCacheStorageData.TypeName;
        }

        /// <summary>
        /// Gets or sets the attributes for the custom cache storage.
        /// </summary>
		/// <value>
		/// The attributes for the custom cache storage.
		/// </value>		
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists"), SRDescription("CustomCacheStorageExtensionsDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
        [CustomAttributesValidation]
        public List<EditableKeyValue> Attributes
        {
            get
            {
                return editableAttributes;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> that implements <see cref="IBackingStore"/>.
        /// </summary>
		/// <value>
		/// The <see cref="Type"/> that implements <see cref="IBackingStore"/>.
		/// </value>
        [Required]
        [SRDescription("CustomCacheStorageNodeTypeDescription", typeof(Resources))]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(IBackingStore), typeof(CustomCacheStorageData))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Type
        {
            get { return typeName; }
            set { typeName = value; }
        }

        /// <summary>
        /// Gets a <see cref="CustomCacheStorageData"/> configuration object using the node data.
        /// </summary>
		/// <value>
		/// A <see cref="CustomCacheStorageData"/> configuration object using the node data.
		/// </value>
        [ReadOnly(true)]
		public override CacheStorageData CacheStorageData
        {
            get
            {
				CustomCacheStorageData customStorageData = new CustomCacheStorageData(Name, typeName);
                customStorageData.Attributes.Clear();
                foreach (EditableKeyValue kv in editableAttributes)
                {
					customStorageData.Attributes.Add(kv.Key, kv.Value);
                }
                return customStorageData;
            }
        }
    }
}