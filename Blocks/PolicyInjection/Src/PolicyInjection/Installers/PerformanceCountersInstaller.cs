//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Security;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Installers
{
    /// <summary>
    /// An <see cref="PerformanceCounterCallHandler"/> class that can be used by installutil.exe to install
    /// the performance counter categories updated by the <see cref="Installer"/>.
    /// </summary>
    [RunInstaller(true)]
    [SecurityCritical]
    public class PerformanceCountersInstaller : Installer
    {
        private List<string> categoryNames;

        /// <summary>
        /// Create the installer with an empty list of categories.
        /// </summary>
        public PerformanceCountersInstaller()
        {
            categoryNames = new List<string>();
        }

        /// <summary>
        /// Create the installer class with the given list of categories.
        /// </summary>
        /// <param name="categoryNames">The set of categories.</param>
        public PerformanceCountersInstaller(params string[] categoryNames)
        {
            this.categoryNames = new List<string>(categoryNames);
        }

        /// <summary>
        /// Create the installer class, reading the categories from a policy set
        /// configured in the given <paramref name="configurationSource"/>.
        /// </summary>
        /// <param name="configurationSource">Configuration source containing the policy set.</param>
        public PerformanceCountersInstaller(IConfigurationSource configurationSource)
        {
            categoryNames = new List<string>();

            PolicyInjectionSettings settings =
                configurationSource.GetSection(PolicyInjectionSettings.SectionName) as PolicyInjectionSettings;

            if (settings != null)
            {
                foreach (PolicyData policyData in settings.Policies)
                {
                    foreach (CallHandlerData handlerData in policyData.Handlers)
                    {
                        PerformanceCounterCallHandlerData perfHandlerData =
                            handlerData as PerformanceCounterCallHandlerData;
                        if (perfHandlerData != null)
                        {
                            categoryNames.Add(perfHandlerData.CategoryName);
                        }
                    }
                }
            }
        }

        ///<summary>
        ///Raises the <see cref="E:System.Configuration.Install.Installer.BeforeInstall"></see> event.
        ///</summary>
        ///
        ///<param name="savedState">An <see cref="T:System.Collections.IDictionary"></see> that contains the state of the computer before the installers in the <see cref="P:System.Configuration.Install.Installer.Installers"></see> property are installed. This <see cref="T:System.Collections.IDictionary"></see> object should be empty at this point. </param>
        [SecurityCritical]
        protected override void OnBeforeInstall(IDictionary savedState)
        {
            GetCategoryFromContext();
            CreateInstallers();
            base.OnBeforeInstall(savedState);
        }

        private void GetCategoryFromContext()
        {
            if (categoryNames.Count == 0)
            {
                string categoryName = Context.Parameters["category"];

                if (categoryName != null)
                {
                    categoryNames = new List<string>(categoryName.Split(';'));
                }
                else
                {
                    throw new InvalidOperationException(Resources.NoCategoryErrorMessage);
                }
            }
        }

        private void CreateInstallers()
        {
            Installers.Clear();
            foreach (string categoryName in categoryNames)
            {
                PerformanceCounterInstaller installer = new PerformanceCounterInstaller();
                installer.CategoryName = categoryName;
                installer.CategoryHelp = Resources.PerformanceCounterCategoryHelp;
                installer.CategoryType = PerformanceCounterCategoryType.MultiInstance;

                installer.Counters.Add(GetNumberOfCallsCreationData());
                installer.Counters.Add(GetCallsPerSecondCreationData());
                installer.Counters.Add(GetNumberOfExceptionsCreationData());
                installer.Counters.Add(GetExceptionsPerSecondCreationData());
                installer.Counters.Add(GetAverageCallDurationCreationData());
                installer.Counters.Add(GetAverageCallDurationBaseCreationData());

                Installers.Add(installer);
            }
        }

        ///<summary>
        ///Raises the <see cref="E:System.Configuration.Install.Installer.BeforeUninstall"></see> event.
        ///</summary>
        ///
        ///<param name="savedState">An <see cref="T:System.Collections.IDictionary"></see> that contains the state of the computer before the installers in the <see cref="P:System.Configuration.Install.Installer.Installers"></see> property uninstall their installations. </param>
        [SecurityCritical]
        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            GetCategoryFromContext();
            CreateInstallers();
            base.OnBeforeUninstall(savedState);
        }

        #region Get Creation Data for counters

        private CounterCreationData GetNumberOfCallsCreationData()
        {
            return new CounterCreationData(
                PerformanceCounterCallHandler.NumberOfCallsCounterName,
                Resources.NumberOfCallsCounterHelp,
                PerformanceCounterType.NumberOfItems32);

        }

        private CounterCreationData GetCallsPerSecondCreationData()
        {
            return new CounterCreationData(
                PerformanceCounterCallHandler.CallsPerSecondCounterName,
                Resources.CallsPerSecondCounterHelp,
                PerformanceCounterType.RateOfCountsPerSecond32);
        }

        private CounterCreationData GetNumberOfExceptionsCreationData()
        {
            return new CounterCreationData(
                PerformanceCounterCallHandler.TotalExceptionsCounterName,
                Resources.NumberOfExceptionsCounterHelp,
                PerformanceCounterType.NumberOfItems32);
        }

        private CounterCreationData GetExceptionsPerSecondCreationData()
        {
            return new CounterCreationData(
                PerformanceCounterCallHandler.ExceptionsPerSecondCounterName,
                Resources.ExceptionsPerSecondCounterHelp,
                PerformanceCounterType.RateOfCountsPerSecond32);
        }

        private CounterCreationData GetAverageCallDurationCreationData()
        {
            return new CounterCreationData(
                PerformanceCounterCallHandler.AverageCallDurationCounterName,
                Resources.AverageCallDurationCounterHelp,
                PerformanceCounterType.AverageTimer32);
        }

        private CounterCreationData GetAverageCallDurationBaseCreationData()
        {
            return new CounterCreationData(
                PerformanceCounterCallHandler.AverageCallDurationBaseCounterName,
                Resources.AverageCallDurationBaseCounterHelp,
                PerformanceCounterType.AverageBase);
        }

        #endregion
    }
}
