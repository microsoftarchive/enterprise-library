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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// Represents the process to build an <see cref="IExceptionHandler"/> described by an isntance <see cref="ExceptionHandlerData"/> configuration object.
    /// </summary>
    /// <remarks>
    /// This is used by the <see cref="ConfiguredObjectStrategy"/> when an instance of the <see cref="IExceptionHandler"/> class is requested to 
    /// a properly configured <see cref="IBuilder"/> instance.
    /// </remarks>
	public class ExceptionHandlerCustomFactory : AssemblerBasedObjectFactory<IExceptionHandler, ExceptionHandlerData>
	{
        /// <summary>
        /// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// </summary>
		public static ExceptionHandlerCustomFactory Instance = new ExceptionHandlerCustomFactory();
	}
}
