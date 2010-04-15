//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{

    /// <summary>
    /// Base class for fluent interface builders that extend the <see cref="IConfigureSecuritySettings"/> interface.
    /// </summary>
    public abstract class ConfigureSecuritySettingsExtension : IConfigureSecuritySettingsExtension
    {
        IConfigureSecuritySettingsExtension contextExtension;

        /// <summary>
        /// Creates an instance of <see cref="ConfigureSecuritySettingsExtension"/> passing the security configuration's fluent interface builder.
        /// </summary>
        /// <param name="context">The current security configuration's fluent interface builder.<br/>
        /// This interface must implement <see cref="IConfigureSecuritySettingsExtension"/>.</param>
        protected ConfigureSecuritySettingsExtension(IConfigureSecuritySettings context)
        {
            contextExtension = context as IConfigureSecuritySettingsExtension;

            if (contextExtension == null) throw new ArgumentException(
                string.Format(CultureInfo.CurrentCulture, Resources.ExceptionParameterMustImplementType, typeof(IConfigureSecuritySettingsExtension).FullName),
                "context");
        }

        /// <summary>
        /// Returns the current security configuration's <see cref="SecuritySettings"/> instance.
        /// </summary>
        protected SecuritySettings SecuritySettings
        {
            get { return contextExtension.SecuritySettings; }
        }

        SecuritySettings IConfigureSecuritySettingsExtension.SecuritySettings
        {
            get { return contextExtension.SecuritySettings; }
        }
    }

    /// <summary>
    /// Allows access to the underlying configuration classes that are used for the <see cref="SecuritySettings"/> instance being configured.
    /// </summary>
    public interface IConfigureSecuritySettingsExtension
    {

        /// <summary>
        /// Returns the <see cref="SecuritySettings"/> instance that is currently being build up.
        /// </summary>
        SecuritySettings SecuritySettings { get; }
    }
}
