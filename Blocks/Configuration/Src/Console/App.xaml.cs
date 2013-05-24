//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Globalization;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Console
{
    /// <summary> 
    /// Interaction logic for App.xaml
    /// </summary>
    // demand for FullTrust required by the splash window invoked in the generated code.
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Unrestricted = true)]
    public partial class App : Application
    {
        private static int ExitWithFailureCode = 1;

        public App()
        {

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", MessageId = "0#")]
        protected override void OnStartup(StartupEventArgs e)
        {
            this.DispatcherUnhandledException += UnhandledException;

            CommandLineParameters = new CommandLineParameters(e.Args);
        }

        private void UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ConfigurationLogWriter.LogException(e.Exception);
            MessageBox.Show(string.Format(CultureInfo.CurrentCulture, Console.Properties.Resources.UnhandledExceptionMessageFormat, e.Exception.Message),
                            Console.Properties.Resources.UnhandledExceptionMessageTitle, MessageBoxButton.OK,
                            MessageBoxImage.Error);

            this.Shutdown(ExitWithFailureCode);
        }

        public static string StartingFileName
        {
            get
            {
                return CommandLineParameters.StartingConfigurationFileName;
            }
        }

        internal static CommandLineParameters CommandLineParameters { get; private set; }
    }
}
