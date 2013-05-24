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

using System;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface extensions for configuring an exception type on a <see cref="ExceptionPolicy"/>
    /// </summary>
    public interface IExceptionConfigurationForExceptionType : IExceptionConfigurationGivenPolicyWithName
    {
        /// <summary>
        /// The <see cref="Exception"/> handled under the <see cref="ExceptionPolicy"/>.
        /// </summary>
        /// <param name="exceptionType">The type of <see cref="Exception"/> handled for this policy.</param>
        /// <returns></returns>
        IExceptionConfigurationAddExceptionHandlers ForExceptionType(Type exceptionType);

        /// <summary>
        /// The <see cref="Exception"/> handled under the <see cref="ExceptionPolicy"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Exception"/> handled for this policy.</typeparam>
        /// <returns></returns>
        IExceptionConfigurationAddExceptionHandlers ForExceptionType<T>() where T : Exception;

    }
}
