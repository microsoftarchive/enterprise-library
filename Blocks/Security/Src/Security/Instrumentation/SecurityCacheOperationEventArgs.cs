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
    /// Provides data for <see cref="SecurityCacheProviderInstrumentationProvider"/> events.
    /// </summary>
    public class SecurityCacheOperationEventArgs: EventArgs
    {
        private SecurityEntityType itemType;
        private IToken token;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityCacheOperationEventArgs"/> class.
        /// </summary>
        /// <param name="itemType">The type of item this cache operation applies to.</param>
        /// <param name="token">The token used on this cache operation.</param>
        public SecurityCacheOperationEventArgs(SecurityEntityType itemType, IToken token)
        {
            this.token = token;
            this.itemType = itemType;
        }

        /// <summary>
        /// Gets the type of item this cache operation applies to.
        /// </summary>
        public SecurityEntityType ItemType
        {
            get { return itemType; }
        }

        /// <summary>
        /// Gets the token used on this cache operation.
        /// </summary>
        public IToken Token
        {
            get { return token; }
        }
    }
}
