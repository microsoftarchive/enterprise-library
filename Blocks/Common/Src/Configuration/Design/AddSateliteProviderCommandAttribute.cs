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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design
{
    /// <summary>
    /// Attribute used to overwrite the Add Command for providers that depend on the availability of another block (Sattelite Providers).
    /// </summary>
    public class AddSateliteProviderCommandAttribute : CommandAttribute
    {
        readonly string sectionName;
        readonly Type defaultProviderConfigurationType;
        readonly string defaultProviderConfigurationPropertyName;
        readonly string sateliteProviderReferencePropertyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddSateliteProviderCommandAttribute"/> specifying the block dependency by its configuration section name.<br/>
        /// </summary>
        /// <param name="sectionName">The name of the configuran section, used to identify the block dependency.</param>
        public AddSateliteProviderCommandAttribute(string sectionName) 
            : base(CommonDesignTime.CommandTypeNames.AddSateliteProviderCommand)
        {
            if (string.IsNullOrEmpty(sectionName)) 
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "sectionName");

            this.sectionName = sectionName;
            this.CommandPlacement = CommandPlacement.ContextAdd;
            this.Replace = CommandReplacement.DefaultAddCommandReplacement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="defaultProviderConfigurationType"></param>
        /// <param name="defaultProviderConfigurationPropertyName"></param>
        /// <param name="sateliteProviderReferencePropertyName"></param>
        public AddSateliteProviderCommandAttribute(string sectionName, Type defaultProviderConfigurationType, string defaultProviderConfigurationPropertyName, string sateliteProviderReferencePropertyName)
            :this(sectionName)
        {
            this.defaultProviderConfigurationType = defaultProviderConfigurationType;
            this.defaultProviderConfigurationPropertyName = defaultProviderConfigurationPropertyName;
            this.sateliteProviderReferencePropertyName = sateliteProviderReferencePropertyName;
        }


        /// <summary>
        /// Gets the section name of the block dependency.
        /// </summary>
        public string SectionName
        {
            get { return sectionName; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Type DefaultProviderConfigurationType
        {
            get
            {
                return defaultProviderConfigurationType;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DefaultProviderConfigurationPropertyName
        {
            get
            {
                return defaultProviderConfigurationPropertyName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SateliteProviderReferencePropertyName
        {
            get
            {
                return sateliteProviderReferencePropertyName;
            }
        }
    }
}
