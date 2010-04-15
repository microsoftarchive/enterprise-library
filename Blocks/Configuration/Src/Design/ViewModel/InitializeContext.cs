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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// An initialization context used for those elements and properties during explicit initialization calls <see cref="ElementViewModel.Initialize"/>
    /// and <see cref="Property.Initialize"/>.
   /// </summary>
    public class InitializeContext
    {
        private readonly IDesignConfigurationSource loadSource;

        ///<summary>
        /// Initializes a new instance of <see cref="InitializeContext"/>
        /// that was not loaded from source.
        /// <seealso cref="WasLoadedFromSource"/>
        ///</summary>
        public InitializeContext() : this(null)
        {
        }

        ///<summary>
        /// Initializes a enw instance of <see cref="InitializeContext"/>
        /// with the <see cref="IDesignConfigurationSource"/> source specified.
        ///</summary>
        ///<param name="loadSource"></param>
        public InitializeContext(IDesignConfigurationSource loadSource)
        {
            this.loadSource = loadSource;
        }

        ///<summary>
        /// Indicates if the initialize context was loaded from a <see cref="IDesignConfigurationSource"/>
        /// or not.
        ///</summary>
        public bool WasLoadedFromSource
        {
            get { return loadSource != null; }
        }

        ///<summary>
        /// Retrieves the <see cref="IDesignConfigurationSource"/>.
        ///</summary>
        public IDesignConfigurationSource LoadSource
        {
            get { return loadSource; }
        }
    }
}
