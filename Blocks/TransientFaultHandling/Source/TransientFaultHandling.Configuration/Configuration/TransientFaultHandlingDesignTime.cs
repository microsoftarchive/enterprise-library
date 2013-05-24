#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Transient Fault Handling Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Configuration
{
    internal static class TransientFaultHandlingDesignTime
    {
        public static class ViewModelTypeNames
        {
            public const string RetryPolicyConfigurationSettingsViewModel =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.RetryPolicyConfigurationSettingsViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string DefaultElementConfigurationProperty =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.DefaultElementConfigurationProperty, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string TimeSpanElementConfigurationProperty =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.TimeSpanElementConfigurationProperty, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
        }

        public static class CommandTypeNames
        {
            public const string WellKnownRetryStrategyElementCollectionCommand = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.WellKnownRetryStrategyElementCollectionCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string AddTransientFaultHandlingBlockCommand = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.AddTransientFaultHandlingBlockCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
        }

        /// <summary>
        /// Provides common editor types used by the design-time infrastructure.
        /// </summary>
        public static class EditorTypes
        {
            /// <summary>
            /// Type name of the TimeSpanEditor class, which is declared in the Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime assembly.
            /// </summary>
            public const string TimeSpanEditor = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors.TimeSpanEditorControl, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            /// <summary>
            /// Type name of the FrameworkElement, which is declared in the PresentationFramework assembly.
            /// </summary>
            public const string FrameworkElement = "System.Windows.FrameworkElement, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";
        }

        public static class ValidatorTypes
        {
            public const string NameValueCollectionValidator = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.NameValueCollectionValidator, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string ExponentialBackoffValidator = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.BlockSpecifics.ExponentialBackoffValidator, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
        }
    }
}
