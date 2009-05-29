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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISecurityCacheProviderInstrumentationProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="token"></param>
        void FireSecurityCacheReadPerformed(SecurityEntityType itemType, IToken token);
    }
}
