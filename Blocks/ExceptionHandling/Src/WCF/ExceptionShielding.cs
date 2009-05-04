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
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF
{
    /// <summary>
    /// Constants definitions for the exception shielding classes.
    /// </summary>
    public class ExceptionShielding
    {
        /// <summary>
        /// The default exception policy name.
        /// </summary>
        public const string DefaultExceptionPolicy = "WCF Exception Shielding";

        /// <summary>
        /// The fault action.
        /// </summary>
        public const string FaultAction = "http://www.microsoft.com/practices/servicefactory/2006/01/wcf/exceptionShielding/fault";

        /// <summary>
        /// The handlingInstanceId Guid value that will be set to the specified property of the fault contract.
        /// Usage: MyProperty = "{Guid}"
        /// </summary>
        public const string HandlingInstanceIdPropertyMappingName = "Guid";
    }
}
