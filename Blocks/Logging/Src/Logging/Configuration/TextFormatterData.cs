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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings for a <see cref="TextFormatter"/>.
    /// </summary>
    public partial class TextFormatterData : FormatterData
    {
        /// <summary>
        /// 
        /// </summary>
        public const string DefaultTemplate = "Timestamp: {timestamp}{newline}\nMessage: {message}{newline}\nCategory: {category}{newline}\nPriority: {priority}{newline}\nEventId: {eventid}{newline}\nSeverity: {severity}{newline}\nTitle:{title}{newline}\nMachine: {localMachine}{newline}\nApp Domain: {localAppDomain}{newline}\nProcessId: {localProcessId}{newline}\nProcess Name: {localProcessName}{newline}\nThread Name: {threadName}{newline}\nWin32 ThreadId:{win32ThreadId}{newline}\nExtended Properties: {dictionary({key} - {value}{newline})}";

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
