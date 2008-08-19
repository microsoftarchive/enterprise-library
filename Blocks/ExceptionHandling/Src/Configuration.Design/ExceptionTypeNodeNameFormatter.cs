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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
    /// <summary>
    /// Used to create a friendly name for ExcpetionType nodes loaded from configuration.
    /// </summary>
    public class ExceptionTypeNodeNameFormatter : TypeNodeNameFormatter
    {
        /// <summary>
        /// Creates a friendly name based on a <see cref="ExceptionTypeData"/> instance, which can be used as a displayname within a graphical tool.
        /// </summary>
        /// <param name="exceptionTypeConfiguration">The configuration this name is based on.</param>
        /// <returns>A friendly name that can be used for the ExceptionTypeNode.</returns>
        public string CreateName(ExceptionTypeData exceptionTypeConfiguration)
        {
            if (exceptionTypeConfiguration == null) 
                throw new ArgumentNullException("exceptionTypeConfiguration");

            return CreateName(exceptionTypeConfiguration.TypeName);
        }
    }
}