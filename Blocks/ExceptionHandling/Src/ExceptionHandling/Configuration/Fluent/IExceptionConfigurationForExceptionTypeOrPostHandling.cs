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
    /// This interface supports the configuration of the Exception Handling Application Block.
    /// </summary>
    public interface IExceptionConfigurationForExceptionTypeOrPostHandling :
        IExceptionConfigurationGivenPolicyWithName,
        IExceptionConfigurationAddExceptionHandlers,
        IExceptionConfigurationThenDoPostHandlingAction
    {
    }
}
