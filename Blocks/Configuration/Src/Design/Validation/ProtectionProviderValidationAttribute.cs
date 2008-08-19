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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.IO;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public sealed class ProtectionProviderValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyInfo"></param>
        /// <param name="errors"></param>
        protected override void ValidateCore(object instance, System.Reflection.PropertyInfo propertyInfo, IList<ValidationError> errors)
        {
            ConfigurationNode node = instance as ConfigurationNode;
            if (node != null)
            {
                string protectionProviderName = propertyInfo.GetValue(instance, null) as string;
                if (!string.IsNullOrEmpty(protectionProviderName) && string.Compare(protectionProviderName, Resources.NoProtectionProvider) != 0)
                {
                    string configurationApplicationFile = ServiceHelper.GetApplicationConfigurationFile(node.Site);
                    if (!String.IsNullOrEmpty(configurationApplicationFile) && File.Exists(configurationApplicationFile))
                    {
                        IProtectedConfigurationSource configurationSource = ServiceHelper.GetCurrentConfigurationSource(node.Site) as IProtectedConfigurationSource;
                        if (configurationSource == null)
                        {
                            errors.Add(new ValidationError(instance as ConfigurationNode, propertyInfo.Name, Resources.ProtectionNotSupportedOnConfigurationSource));
                        }
                    }
                }
            }
        }
    }
}
