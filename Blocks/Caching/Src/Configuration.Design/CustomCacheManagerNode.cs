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
	public class CustomCacheManagerNode : CacheManagerBaseNode
	{
		private List<EditableKeyValue> editableAttributes = new List<EditableKeyValue>();
		private string typeName;

		/// <summary>
		/// Initialize a new instance of the <see cref="CustomCacheManagerNode"/> class.
		/// </summary>
		public CustomCacheManagerNode()
			: this(new CustomCacheManagerData(Resources.DefaultCacheManagerName, string.Empty))
		{
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="CustomCacheManagerNode"/> class with a <see cref="CustomCacheStorageData"/> object.
		/// </summary>
		/// <param name="customCacheManagerData">The <see cref="CustomCacheStorageData"/> to display.</param>
		public CustomCacheManagerNode(CustomCacheManagerData customCacheManagerData)
		{
			if (null == customCacheManagerData) throw new ArgumentNullException("customCacheManagerData");

			Rename(customCacheManagerData.Name);
			foreach (string key in customCacheManagerData.Attributes)
			{
				editableAttributes.Add(new EditableKeyValue(key, customCacheManagerData.Attributes[key]));
			}
			this.typeName = customCacheManagerData.TypeName;
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
		[SRDescription("CustomCacheManagerTypeDescription", typeof(Resources))]
		[Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
		[BaseType(typeof(ICacheManager), typeof(CustomCacheManagerData))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public string Type
		{
			get { return typeName; }
			set { typeName = value; }
		}

		/// <summary>
		/// Gets a <see cref="CacheManagerDataBase"/> configuration object using the node data.
		/// </summary>
		/// <value>
		/// A <see cref="CacheManagerDataBase"/> configuration object using the node data.
		/// </value>
		[ReadOnly(true)]
		public override CacheManagerDataBase CacheManagerData
		{
			get
			{
				CustomCacheManagerData customCacheManagerData = new CustomCacheManagerData(Name, typeName);
				customCacheManagerData.Attributes.Clear();
				foreach (EditableKeyValue kv in editableAttributes)
				{
					customCacheManagerData.Attributes.Add(kv.Key, kv.Value);
				}
				return customCacheManagerData;
			}
		}
	}
}
