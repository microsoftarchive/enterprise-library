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
    /// This interface supports the fluent configuration of the Exception Handling Application Block.
    /// </summary>
    public interface IExceptionConfigurationThenDoPostHandlingAction : IFluentInterface
    {
        /// <summary>
        /// End the current exception handling chain by doing nothing more.
        /// </summary>
        /// <returns></returns>
        IExceptionConfigurationForExceptionType ThenDoNothing();

        /// <summary>
        /// End the current exception handling chain by notifying the caller that an exception should be rethrown.
        /// </summary>
        /// <returns></returns>
        IExceptionConfigurationForExceptionType ThenNotifyRethrow();

        /// <summary>
        /// End the current exception handling chain by throwing a new exception.
        /// </summary>
        /// <returns></returns>
        IExceptionConfigurationForExceptionType ThenThrowNewException();
    }
}
