#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Enterprise Library Quick Start
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace SlabReconfigurationWebRole
{
    partial class MvcApplication
    {
        internal const string AzureLoggingConfigurationSettingName = "azureLoggingConnectionString";
        internal const string AzureLoggingVerbositySettingName = "azureLoggingVerbosity";

        private ObservableEventListener listener;

        public MvcApplication()
        {
        }

        partial void InitializeDiagnostics()
        {
            var connectionString = RoleEnvironment.GetConfigurationSettingValue(AzureLoggingConfigurationSettingName);

            this.listener = new ObservableEventListener();
            this.listener.LogToWindowsAzureTable(RoleEnvironment.CurrentRoleInstance.Id, connectionString, bufferingInterval: TimeSpan.FromSeconds(30));

            this.SetupListener();
            RoleEnvironment.Changed += RoleEnvironment_Changed;
        }

        void RoleEnvironment_Changed(object sender, RoleEnvironmentChangedEventArgs e)
        {
            // Using WAD for tracing how the verbosity is updated
            Trace.TraceInformation("Updating instance");

            foreach (var settingChange in e.Changes.OfType<RoleEnvironmentConfigurationSettingChange>())
            {
                if (string.Equals(settingChange.ConfigurationSettingName, AzureLoggingVerbositySettingName, StringComparison.Ordinal))
                {
                    Trace.TraceInformation("Setting up listener after change");

                    this.SetupListener();
                }
            }
        }

        private void SetupListener()
        {
            EventLevel level;
            if (Enum.TryParse<EventLevel>(RoleEnvironment.GetConfigurationSettingValue(AzureLoggingVerbositySettingName), out level))
            {
                Trace.TraceInformation("Updating verbosity to {0}", level);

                this.listener.EnableEvents(SlabReconfigurationWebRole.Events.QuickStartEventSource.Log, level);
            }
            else
            {
                Trace.TraceWarning("Invalid verbosity configuration");
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            if (this.listener != null)
            {
                this.listener.Dispose();
            }
        }
    }
}