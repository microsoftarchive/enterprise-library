//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
	/// Provides data for the <seealso cref="IConfigurationUIHierarchy.Saved"/> event of the <see cref="IConfigurationUIHierarchyService"/>.
    /// </summary>    
    public class HierarchySavedEventArgs : EventArgs
    {
        private readonly IConfigurationUIHierarchy uiHierarchy;

        /// <summary>
		/// Initialize a new instance of the <see cref="HierarchySavedEventArgs"/> class with a <see cref="IConfigurationUIHierarchy"/> object.
        /// </summary>
        /// <param name="uiHierarchy">
		/// An <see cref="IConfigurationUIHierarchy"/> object.
        /// </param>
        public HierarchySavedEventArgs(IConfigurationUIHierarchy uiHierarchy)
        {
            this.uiHierarchy = uiHierarchy;
        }

        /// <summary>
		/// Gets the <see cref="IConfigurationUIHierarchy"/> that was added.
        /// </summary>
        /// <value>
		/// The <see cref="IConfigurationUIHierarchy"/> that was added.
        /// </value>
        public IConfigurationUIHierarchy UIHierarchy
        {
            get { return uiHierarchy; }
        }

    }
}