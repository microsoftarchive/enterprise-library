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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings that describe a <see cref="BinaryLogFormatter"/>.
    /// </summary>
    public class BinaryLogFormatterData : FormatterData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryLogFormatterData"/> class with default values.
        /// </summary>
        public BinaryLogFormatterData()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryLogFormatterData"/> class with a name.
        /// </summary>
        /// <param name="name">The name for the represented <see cref="BinaryLogFormatter"/>.</param>
        public BinaryLogFormatterData(string name)
            : base(name, typeof(BinaryLogFormatter))
        { }

        /// <summary>
        /// Returns the <see cref="TypeRegistration"/> entry for this data section.
        /// </summary>
        /// <returns>The type registration for this data section</returns>
        public override IEnumerable<TypeRegistration> GetRegistrations()
        {
            yield return
                new TypeRegistration<ILogFormatter>(() => new BinaryLogFormatter())
               {
                   Name = this.Name,
                   Lifetime = TypeRegistrationLifetime.Transient
               };
        }
    }
}
