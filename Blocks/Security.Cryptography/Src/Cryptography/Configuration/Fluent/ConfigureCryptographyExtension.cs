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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Base class for fluent interface builders that extend the cryptography configuration fluent interface.
    /// </summary>
    public abstract class ConfigureCryptographyExtension : IConfigureCryptography, IConfigureCryptographyExtension
    {
        readonly IConfigureCryptographyExtension contextExtension;

        /// <summary>
        /// Creates an instance of <see cref="ConfigureCryptographyExtension"/> passing the cryptography configuration's fluent interface builder.
        /// </summary>
        /// <param name="context">The current caching configuration's fluent interface builder.<br/>
        /// This interface must implement <see cref="IConfigureCryptographyExtension"/>.</param>
        protected ConfigureCryptographyExtension(IConfigureCryptography context)
        {
            contextExtension = context as IConfigureCryptographyExtension;
            if (contextExtension == null) throw new ArgumentException(
                string.Format(Resources.Culture, Resources.ExceptionParameterMustImplementType, typeof(IConfigureCryptographyExtension).FullName)
                , "context");
        }


        /// <summary>
        /// Returns the <see cref="CryptographySettings"/> instance that is currently being build up.
        /// </summary>
        protected CryptographySettings CryptographySettings
        {
            get { return contextExtension.CryptographySettings; }
        }

        CryptographySettings IConfigureCryptographyExtension.CryptographySettings
        {
            get { return contextExtension.CryptographySettings; }
        }
    }


    /// <summary>
    /// Allows access to the underlying <see cref="CryptographySettings"/> being configured.
    /// </summary>
    public interface IConfigureCryptographyExtension
    {
        /// <summary>
        /// Returns the <see cref="CryptographySettings"/> instance that is currently being build up.
        /// </summary>
        CryptographySettings CryptographySettings { get; }
    }
}
