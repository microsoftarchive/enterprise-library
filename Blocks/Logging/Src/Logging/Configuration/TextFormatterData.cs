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

using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings for a <see cref="TextFormatter"/>.
    /// </summary>
    public class TextFormatterData : FormatterData
    {
        private const string templateProperty = "template";

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFormatterData"/> class with default values.
        /// </summary>
        public TextFormatterData()
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="TextFormatterData"/> class with a template.
        /// </summary>
        /// <param name="templateData">
        /// Template containing tokens to replace.
        /// </param>
        public TextFormatterData(string templateData)
            : this("unnamed", templateData)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="TextFormatterData"/> class with a name and template.
        /// </summary>
        /// <param name="name">
        /// The name of the formatter.
        /// </param>
        /// <param name="templateData">
        /// Template containing tokens to replace.
        /// </param>
        public TextFormatterData(string name, string templateData)
            : this(name, typeof(TextFormatter), templateData)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="TextFormatterData"/> class with a name and template.
        /// </summary>
        /// <param name="name">
        /// The name of the formatter.
        /// </param>
        /// <param name="formatterType">
        /// The type of the formatter.
        /// </param>
        /// <param name="templateData">
        /// Template containing tokens to replace.
        /// </param>
        private TextFormatterData(string name, Type formatterType, string templateData)
            : base(name, formatterType)
        {
            this.Template = templateData;
        }


        /// <summary>
        /// Gets or sets the template containing tokens to replace.
        /// </summary>
        [ConfigurationProperty(templateProperty, IsRequired = true)]
        public string Template
        {
            get
            {
                return (string)this[templateProperty];
            }
            set
            {
                this[templateProperty] = value;
            }
        }

        /// <summary>
        /// Returns the <see cref="TypeRegistration"/> entry for this data section.
        /// </summary>
        /// <returns>The type registration for this data section</returns>
        public override IEnumerable<TypeRegistration> GetRegistrations()
        {
            yield return new TypeRegistration<ILogFormatter>(
               () => new TextFormatter(this.Template))
            {
                Name = this.Name,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }
    }
}
