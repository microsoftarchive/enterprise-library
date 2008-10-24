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

using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Formatters
{
    /// <summary>
    /// Represents an instance of <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.TextFormatterData"/>
    /// as an instrumentation class.
    /// </summary>
    [ManagementEntity]
    public partial class TextFormatterSetting : FormatterSetting
    {
        string template;

        /// <summary>
        /// Initialize a new instance of the <see cref="TextFormatterSetting"/> class with
        /// the formatter configuration, the name of the formatter, and the template to use.
        /// </summary>
        /// <param name="sourceElement">The configuration for the formatter.</param>
        /// <param name="name">The name of the formatter.</param>
        /// <param name="template">The template for the formatter.</param>
        public TextFormatterSetting(TextFormatterData sourceElement,
                                    string name,
                                    string template)
            : base(sourceElement, name)
        {
            this.template = template;
        }

        /// <summary>
        /// Gets the template for the represented configuration element.
        /// </summary>
        [ManagementConfiguration]
        public string Template
        {
            get { return template; }
            set { template = value; }
        }

        /// <summary>
        /// Returns the <see cref="TextFormatterSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="TextFormatterSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static TextFormatterSetting BindInstance(string ApplicationName,
                                                        string SectionName,
                                                        string Name)
        {
            return BindInstance<TextFormatterSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="TextFormatterSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<TextFormatterSetting> GetInstances()
        {
            return GetInstances<TextFormatterSetting>();
        }

        /// <summary>
        /// Saves the changes on the <see cref="TextFormatterSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return TextFormatterDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
