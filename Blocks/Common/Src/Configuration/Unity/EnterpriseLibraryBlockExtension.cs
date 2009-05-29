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

using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity
{
    /// <summary>
    /// Base class for Enterprise Library Blocks' container extensions.
    /// </summary>
    public abstract class EnterpriseLibraryBlockExtension : UnityContainerExtension
    {
        /// <summary>
        /// Ensure that this container has been configured to resolve Enterprise Library
        /// objects.
        /// </summary>
        protected override void Initialize()
        {
            if(Container.Configure<EnterpriseLibraryCoreExtension>() == null)
            {
                Container.AddExtension(new EnterpriseLibraryCoreExtension(ConfigurationSourceFactory.Create()));
            }
        }
    }
}
