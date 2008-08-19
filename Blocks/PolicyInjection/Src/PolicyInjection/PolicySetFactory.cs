//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection
{
    /// <summary>
    /// A factory class that can create <see cref="PolicySet"/>s from configuration.
    /// </summary>
    public class PolicySetFactory
    {
        private IConfigurationSource configurationSource;

        /// <summary>
        /// Creates a new <see cref="PolicySetFactory"/> that creates <see cref="PolicySet"/>s
        /// based on the default configuration.
        /// </summary>
        public PolicySetFactory()
            : this(ConfigurationSourceFactory.Create())
        {
        }

        /// <summary>
        /// Creates a new <see cref="PolicySetFactory"/> that creates <see cref="PolicySet"/>s
        /// based on the given configuration source.
        /// </summary> 
        /// <param name="configurationSource"><see cref="IConfigurationSource"/> to get the
        /// configuration information from.</param>
        public PolicySetFactory(IConfigurationSource configurationSource)
        {
            if (configurationSource == null)
            {
                throw new ArgumentNullException("configurationSource");
            }

            this.configurationSource = configurationSource;
        }

        /// <summary>
        /// Creates a new <see cref="PolicySet"/> from configuration.
        /// </summary>
        /// <returns>The created <see cref="PolicySet"/>.</returns>
        public PolicySet Create()
        {
            return EnterpriseLibraryFactory.BuildUp<PolicySet>(configurationSource);
        }
    }
}
