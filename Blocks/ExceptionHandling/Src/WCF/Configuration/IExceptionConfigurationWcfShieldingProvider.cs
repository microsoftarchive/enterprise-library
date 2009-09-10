//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// This interfaces supports the fluent configuration of exception shielding.
    /// </summary>
    public interface IExceptionConfigurationWcfShieldingProvider : IExceptionConfigurationForExceptionTypeOrPostHandling
    {
        /// <summary>
        /// Maps a property from the exception to the fault contract.
        /// </summary>
        /// <param name="name">Fault contract property to map to</param>
        /// <param name="source">Source property to map from.</param>
        /// <returns></returns>
        IExceptionConfigurationWcfShieldingProvider MapProperty(string name, string source);
    }
}
