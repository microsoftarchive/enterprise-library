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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Defines an exception policy with a given name.
    /// </summary>
    public interface IExceptionConfigurationGivenPolicyWithName : IFluentInterface
    {
        /// <summary>
        /// Defines new policy with a given name.
        /// </summary>
        /// <param name="name">Name of policy</param>
        /// <returns></returns>
        IExceptionConfigurationForExceptionType GivenPolicyWithName(string name);
    }
}
