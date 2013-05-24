//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================


namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    internal static class LoggingDesignTime
    {
        internal static class ViewModelTypeNames
        {
            public const string LogggingSectionViewModel =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.LoggingSectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string SystemDiagnosticsTraceListenerDataViewModel = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.SystemDiagnosticsTraceListenerDataViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string CustomTraceListenerDataViewModel = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.CustomTraceListenerDataViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string TraceListenerReferenceViewModel =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.TraceListenerReferenceViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string TraceListenerElementCollectionViewModel =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.TraceListenerElementCollectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string TypeNameElementProperty =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.ElementProperty, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string EmailTraceListenerPropertyViewModel =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.Logging.EmailTraceListenerPasswordProperty, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string SourceLevelsProperty =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.Logging.SourceLevelsProperty, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string TimeSpanElementConfigurationProperty =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.TimeSpanElementConfigurationProperty, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
        }

        internal static class EditorTypeNames
        {
            public const string OverridenTraceListenerCollectionEditor =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.OverriddenTraceListenerCollectionEditor, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
        }

        internal static class CommandTypeNames
        {
            public const string AddLoggingBlockCommand = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.AddLoggingBlockCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
        }

        internal static class ValidatorTypes
        {
            public const string EmailTraceListenerAuthenticationValidator = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.Logging.EmailTraceListenerAuthenticationValidator, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string LogPriorityMinMaxValidatorType = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.Logging.LogPriorityMinMaxValidator, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string NameValueCollectionValidator = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.NameValueCollectionValidator, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
        }
    }
}
