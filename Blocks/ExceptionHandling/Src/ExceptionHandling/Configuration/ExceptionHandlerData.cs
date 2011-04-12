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

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    partial class ExceptionHandlerData
    {
        /// <summary>
        /// Builds a name with the supplied prefix. 
        /// </summary>
        /// <param name="prefix">Prefix to use when building a name</param>
        /// <returns>A name as string</returns>
        protected string BuildName(string prefix)
        {
            return prefix + "." + Name;
        }
    }
}
