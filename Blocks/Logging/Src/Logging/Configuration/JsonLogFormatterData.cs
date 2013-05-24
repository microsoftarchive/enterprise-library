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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings that describe a <see cref="JsonLogFormatter"/> object.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "JsonLogFormatterDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "JsonLogFormatterDataDisplayName")]
    public class JsonLogFormatterData : FormatterData
    {
        const string FormattingProperty = "formatting";

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLogFormatterData"/> class with default values.
        /// </summary>
        public JsonLogFormatterData() { Type = typeof(JsonLogFormatter); }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLogFormatterData"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name for the represented <see cref="JsonLogFormatter"/> object.</param>
        public JsonLogFormatterData(string name)
            : base(name, typeof(JsonLogFormatter))
        { }

        /// <summary>
        /// Gets or sets the template that contains the tokens to replace.
        /// </summary>
        [ConfigurationProperty(FormattingProperty)]
        [ResourceDescription(typeof(DesignResources), "JsonLogFormatterDataFormattingDescription")]
        [ResourceDisplayName(typeof(DesignResources), "JsonLogFormatterDataFormattingDisplayName")]
        public JsonFormatting Formatting
        {
            get { return (JsonFormatting)this[FormattingProperty]; }
            set { this[FormattingProperty] = value; }
        }

        /// <summary>
        /// Builds the <see cref="ILogFormatter" /> object represented by this configuration object.
        /// </summary>
        /// <returns>
        /// A formatter.
        /// </returns>
        public override ILogFormatter BuildFormatter()
        {
            return new JsonLogFormatter(this.Formatting);
        }
    }
}
