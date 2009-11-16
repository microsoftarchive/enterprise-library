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
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary/>
    public static class ExceptionHandlingDesignTime
    {
        /// <summary/>
        public static class ViewModelTypeNames
        {
            ///<summary>
            ///</summary>
            public const string ExceptionHandlingSectionViewModel =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.ExceptionHandlingSectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

        }
        /// <summary/>
        public static class CommandTypeNames
        {
            /// <summary/>
            public const string AddExceptionHandlingBlockCommand = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.AddExceptionHandlingBlockCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }
    }
}
