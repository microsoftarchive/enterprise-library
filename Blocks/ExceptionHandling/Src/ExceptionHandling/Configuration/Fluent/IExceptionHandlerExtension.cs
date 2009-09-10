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

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Used to provide context to extensions of the Exception Handling fluent configuration interface.
    /// </summary>
    public interface IExceptionHandlerExtension : IFluentInterface
    {
        /// <summary>
        /// Retrieves data about the currently built up ExceptionTypeData.  Exception handler configuration extensions will use this to 
        /// add their handler information to the exception.
        /// </summary>
        /// <seealso cref="ReplaceHandler"/>
        /// <seealso cref="ReplaceWithHandlerLoggingConfigurationSourceBuilderExtensions"/>
        /// 
        /// <seealso cref="WrapHandler"/>
        /// <seealso cref="WrapWithHandlerLoggingConfigurationSourceBuilderExtensions"/>
        ExceptionTypeData CurrentExceptionTypeData { get; }
    }
}
