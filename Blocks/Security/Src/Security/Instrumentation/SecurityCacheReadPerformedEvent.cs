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
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation
{

    /// <summary>
    /// Represents the WMI event fired when an item is read from a <see cref="SecurityCacheProvider"/>.
    /// </summary>
    public class SecurityCacheReadPerformedEvent: SecurityEvent
    {
        private string entityType;
        private string tokenUsed;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityCacheReadPerformedEvent"/> class.
        /// </summary>
        /// <param name="instanceName">The name of the <see cref="SecurityCacheProvider"/> instance the item is read from.</param>
        /// <param name="entityType">The name of the <see cref="SecurityEntityType"/> that is read from the cache.</param>
        /// <param name="tokenUsed">The contents of the token that is used to read the item from the cache.</param>
        public SecurityCacheReadPerformedEvent(string instanceName, string entityType, string tokenUsed)
			: base(instanceName)
        {
            this.entityType = entityType;
            this.tokenUsed = tokenUsed;
        }

        /// <summary>
        /// Gets the name of the <see cref="SecurityEntityType"/> that is read from the cache.
        /// </summary>
        public string EntityType
        {
            get { return entityType; }
        }

        /// <summary>
        /// Gets the contents of the token that is used to read the item from the cache.
        /// </summary>
        public string TokenUsed
        {
            get { return tokenUsed; }
        }
    }
}
