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
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// This interface provides the extension point for handlers that provide a fluent configuration interface.
    /// </summary>
    /// <remarks>
    /// Handlers providing a fluent interface should provide extension methods to this interface.
    /// <example>
    ///  public static class ReplaceWithHandlerLoggingConfigurationSourceBuilderExtensions
    ///  {
    ///     public static IExceptionConfigurationReplaceWithProvider ReplaceWith(this IExceptionConfigurationAddExceptionHandlers context, Type replacingExceptionType)
    ///     { }
    ///  }
    /// </example>
    /// 
    /// The context implementer offers additional interfaces that are useful in continuing the configuration of Exception Handling (<see cref="IExceptionConfigurationForExceptionTypeOrPostHandling"/>
    /// or in adding your custom handler information to the currently building exception type (<see cref="IExceptionHandlerExtension"/>).  In lieu of casting to these
    /// interfaces directly, consider using the <see cref="ExceptionHandlerConfigurationExtension"/> as a base class for your custom handler builder.
    /// </remarks>
    public interface IExceptionConfigurationAddExceptionHandlers : IExceptionConfigurationThenDoPostHandlingAction
    {
    }
}
