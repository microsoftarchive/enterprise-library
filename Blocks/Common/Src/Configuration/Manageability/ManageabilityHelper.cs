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

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    /// <summary>
    /// Represents a manageability configuration helper.
    /// </summary>
    public class ManageabilityHelper : IManageabilityHelper
    {
        readonly string applicationName;
        readonly IDictionary<string, ConfigurationSectionManageabilityProvider> manageabilityProviders;
        readonly bool readGroupPolicies;
        readonly IRegistryAccessor registryAccessor;

        ///<summary>
        /// Initialize a new instance of a <see cref="ManageabilityHelper"/> class.
        ///</summary>
        ///<param name="manageabilityProviders">The manageability propvodiers.</param>
        ///<param name="readGroupPolicies">true to read Group Policies; otherwise, false.</param>
        ///<param name="applicationName">The application name.</param>
        public ManageabilityHelper(IDictionary<string, ConfigurationSectionManageabilityProvider> manageabilityProviders,
                                   bool readGroupPolicies,
                                   string applicationName)
            : this(manageabilityProviders,
                   readGroupPolicies,
                   new RegistryAccessor(),
                   applicationName) { }

        ///<summary>
        /// Initialize a new instance of the <see cref="ManageabilityHelper"/> class.
        ///</summary>
        ///<param name="manageabilityProviders">The manageability providers.</param>
        ///<param name="readGroupPolicies">true to read Group Policies; otherwise, false.</param>
        ///<param name="registryAccessor">A registry accessor.</param>
        ///<param name="applicationName">The application name.</param>
        public ManageabilityHelper(IDictionary<string, ConfigurationSectionManageabilityProvider> manageabilityProviders,
                                   bool readGroupPolicies,
                                   IRegistryAccessor registryAccessor,
                                   string applicationName)
        {
            this.manageabilityProviders = manageabilityProviders;
            this.readGroupPolicies = readGroupPolicies;
            this.registryAccessor = registryAccessor;
            this.applicationName = applicationName;
        }

        /// <summary>
        /// Gets the manageability providers.
        /// </summary>
        /// <value>
        /// The manageability providers.
        /// </value>
        public IDictionary<string, ConfigurationSectionManageabilityProvider> ManageabilityProviders
        {
            get { return manageabilityProviders; }
        }

        /// <summary>
        /// Builds the section key name.
        /// </summary>
        /// <param name="applicationName">
        /// The application name.
        /// </param>
        /// <param name="sectionName">
        /// The section name.
        /// </param>
        /// <returns>
        /// The section key name.
        /// </returns>
        public static string BuildSectionKeyName(String applicationName,
                                                   String sectionName)
        {
            return Path.Combine(Path.Combine(@"Software\Policies\", applicationName), sectionName);
        }

        void DoUpdateConfigurationSectionManageability(IConfigurationAccessor configurationAccessor,
                                                       String sectionName)
        {
            ConfigurationSectionManageabilityProvider manageabilityProvider = manageabilityProviders[sectionName];

            ConfigurationSection section = configurationAccessor.GetSection(sectionName);
            if (section != null)
            {
                IRegistryKey machineKey = null;
                IRegistryKey userKey = null;

                try
                {
                    LoadPolicyRegistryKeys(sectionName, out machineKey, out userKey);

                    if (!manageabilityProvider
                        .OverrideWithGroupPolicies(section, readGroupPolicies, machineKey, userKey))
                    {
                        configurationAccessor.RemoveSection(sectionName);
                    }
                }
                catch (Exception e)
                {
                    ManageabilityExtensionsLogger.LogException(e, Resources.ExceptionUnexpectedErrorProcessingSection);
                }
                finally
                {
                    ReleasePolicyRegistryKeys(machineKey, userKey);
                }
            }
        }

        void LoadPolicyRegistryKeys(String sectionName,
                                    out IRegistryKey machineKey,
                                    out IRegistryKey userKey)
        {
            if (readGroupPolicies)
            {
                String sectionKeyName = BuildSectionKeyName(applicationName, sectionName);
                machineKey = registryAccessor.LocalMachine.OpenSubKey(sectionKeyName);
                userKey = registryAccessor.CurrentUser.OpenSubKey(sectionKeyName);
            }
            else
            {
                machineKey = null;
                userKey = null;
            }
        }

        static void ReleasePolicyRegistryKeys(IRegistryKey machineKey,
                                              IRegistryKey userKey)
        {
            if (machineKey != null)
            {
                try
                {
                    machineKey.Close();
                }
                catch (Exception) { }
            }

            if (userKey != null)
            {
                try
                {
                    userKey.Close();
                }
                catch (Exception) { }
            }
        }

        /// <summary>
        /// Updates configuration management from the given configuration.
        /// </summary>
        /// <param name="configurationAccessor">
        /// The accessor for the configuration.
        /// </param>
        public void UpdateConfigurationManageability(IConfigurationAccessor configurationAccessor)
        {
            using (new GroupPolicyLock())
            {
                foreach (String sectionName in manageabilityProviders.Keys)
                {
                    DoUpdateConfigurationSectionManageability(configurationAccessor, sectionName);
                }
            }
        }

        /// <summary>
        /// Updates configuration management from the given configuration in the given section.
        /// </summary>
        /// <param name="configurationAccessor">
        /// The accessor for the configuration.
        /// </param>
        /// <param name="sectionName">
        /// The section to update.
        /// </param>
        public void UpdateConfigurationSectionManageability(IConfigurationAccessor configurationAccessor,
                                                            string sectionName)
        {
            using (new GroupPolicyLock())
            {
                DoUpdateConfigurationSectionManageability(configurationAccessor, sectionName);
            }
        }

        class GroupPolicyLock : IDisposable
        {
            IntPtr machineCriticalSectionHandle;
            IntPtr userCriticalSectionHandle;

            public GroupPolicyLock()
            {
                // lock policy processing, user first
                // see http://msdn.microsoft.com/library/default.asp?url=/library/en-us/policy/policy/entercriticalpolicysection.asp for details

                userCriticalSectionHandle = NativeMethods.EnterCriticalPolicySection(false);
                if (IntPtr.Zero == userCriticalSectionHandle)
                {
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }

                machineCriticalSectionHandle = NativeMethods.EnterCriticalPolicySection(true);
                if (IntPtr.Zero == machineCriticalSectionHandle)
                {
                    // save the current call's error
                    int hr = Marshal.GetHRForLastWin32Error();

                    // release the user policy section first - don't check for errors as an exception will be thrown
                    NativeMethods.LeaveCriticalPolicySection(userCriticalSectionHandle);

                    Marshal.ThrowExceptionForHR(hr);
                }
            }

            void IDisposable.Dispose()
            {
                // release locks in the reverse order
                // handles shouldn't be null here, as the constructor should have thrown if they were
                // exceptions are not thrown here; critical section locks will be timed out by the O.S.
                NativeMethods.LeaveCriticalPolicySection(machineCriticalSectionHandle);
                NativeMethods.LeaveCriticalPolicySection(userCriticalSectionHandle);
            }
        }
    }
}
