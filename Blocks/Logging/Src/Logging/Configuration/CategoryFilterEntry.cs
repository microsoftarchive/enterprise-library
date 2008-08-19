//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents a single <see cref="CategoryFilterEntry"/> configuration settings.
    /// </summary>
	public class CategoryFilterEntry : NamedConfigurationElement
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CategoryFilterData"/> class.</para>
        /// </summary>
		public CategoryFilterEntry()
    	{
    	}

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="CategoryFilterData"/> class with a name.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the <see cref="CategoryFilterData"/>.</para>
        /// </param>
		public CategoryFilterEntry(string name)
			: base(name)
    	{
    	}
	}
}