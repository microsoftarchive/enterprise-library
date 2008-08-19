//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// Defines the contract for an ExceptionHandler.  An ExceptionHandler contains specific handling
    /// logic (i.e. logging the exception, replacing the exception, etc.) that is executed in a chain of multiple
    /// ExceptionHandlers.  A chain of one or more ExceptionHandlers is executed based on the exception type being 
    /// handled, as well as the <see cref="ExceptionPolicy"/>.  <seealso cref="ExceptionPolicy.HandleException(Exception,String)"/>
    /// </summary>    
	public interface IExceptionHandler
    {
		/// <summary>
        /// <para>When implemented by a class, handles an <see cref="Exception"/>.</para>
        /// </summary>
        /// <param name="exception"><para>The exception to handle.</para></param>        
        /// <param name="handlingInstanceId">
        /// <para>The unique ID attached to the handling chain for this handling instance.</para>
        /// </param>
        /// <returns><para>Modified exception to pass to the next exceptionHandlerData in the chain.</para></returns>
        Exception HandleException(Exception exception, Guid handlingInstanceId);
    }
}