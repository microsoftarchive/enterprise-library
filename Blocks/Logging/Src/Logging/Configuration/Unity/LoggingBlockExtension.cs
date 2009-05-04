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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity
{
	/// <summary>
	/// Container extension to the policies required to create the Logging Application Block's
	/// objects described in the configuration file.
	/// </summary>
	public class LoggingBlockExtension : EnterpriseLibraryBlockExtension
	{
		/// <summary>
		/// Adds the policies describing the Logging Application Block's objects.
		/// </summary>
		protected override void Initialize()
		{
			LoggingSettings settings = (LoggingSettings)this.ConfigurationSource.GetSection(LoggingSettings.SectionName);
            InstrumentationConfigurationSection instrumentationSettings = (InstrumentationConfigurationSection)this.ConfigurationSource.GetSection(InstrumentationConfigurationSection.SectionName);

			if (settings == null)
			{
				// skip if no settings are available
				return;
			}

            UnityContainerConfigurator configurator = new UnityContainerConfigurator(Container);
            configurator.RegisterAll(settings.CreateRegistrations());
            
          

            CreateTraceManagerPolicies(
                Context.Policies,
                instrumentationSettings);
		}

        private static void CreateTraceManagerPolicies(IPolicyList policyList,
            InstrumentationConfigurationSection instrumentationSettings
            )
        {
            new PolicyBuilder<TraceManager, InstrumentationConfigurationSection>(
                null,
                instrumentationSettings,
                c => new TraceManager(
                        Resolve.Reference<LogWriter>(null),
                        new TracerInstrumentationListener(GetPerformanceCountersEnabled(c)))
                    ).AddPoliciesToPolicyList(policyList);
        }

    

        private static bool GetPerformanceCountersEnabled(InstrumentationConfigurationSection instrumentationSettings)
        {
            if (instrumentationSettings != null)
            {
                return instrumentationSettings.PerformanceCountersEnabled;
            }

            return false;
        }
	}
}
