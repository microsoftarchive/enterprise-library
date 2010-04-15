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
    
    internal static class ExceptionHandlingDesignTime
    {
        public static class ViewModelTypeNames
        {
            public const string ExceptionHandlingSectionViewModel =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.ExceptionHandlingSectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string ExceptionPolicyDataViewModel =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.ExceptionPolicyDataViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string ExceptionTypeDataViewModel =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.ExceptionTypeDataViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string ExceptionHandlerDataViewModel =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.ExceptionHandlerDataViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
        }

        public static class CommandTypeNames
        {
            public const string AddExceptionPolicyCommand = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.AddExceptionPolicyCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string AddExceptionHandlingBlockCommand = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.AddExceptionHandlingBlockCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string AddExceptionTypeCommand = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.ExceptionTypeAddCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
        }

        public static class ValidatorTypes
        {
            public const string NameValueCollectionValidator = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.NameValueCollectionValidator, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
        }
    }
}
