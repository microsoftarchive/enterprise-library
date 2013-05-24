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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings that describe a <see cref="BinaryLogFormatter"/>.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "BinaryLogFormatterDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "BinaryLogFormatterDataDisplayName")]
    public class BinaryLogFormatterData : FormatterData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryLogFormatterData"/> class with default values.
        /// </summary>
        public BinaryLogFormatterData() { Type = typeof(BinaryLogFormatter); }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryLogFormatterData"/> class with a name.
        /// </summary>
        /// <param name="name">The name for the represented <see cref="BinaryLogFormatter"/>.</param>
        public BinaryLogFormatterData(string name)
            : base(name, typeof(BinaryLogFormatter))
        { }

        /// <summary>
        /// Builds the <see cref="ILogFormatter" /> object represented by this configuration object.
        /// </summary>
        /// <returns>
        /// A formatter.
        /// </returns>
        public override ILogFormatter BuildFormatter()
        {
            return new BinaryLogFormatter();
        }
    }
}
