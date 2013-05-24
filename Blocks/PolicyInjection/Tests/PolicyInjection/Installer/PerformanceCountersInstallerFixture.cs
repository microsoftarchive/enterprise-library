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
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Installers;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.Installer
{
    [TestClass]
    public class PerformanceCountersInstallerFixture
    {
        const string firstCategory = "PerformanceCountersInstallFixture First Perfcounter category";

        const string secondCategory =
            "PerformanceCountersInstallFixture second perfcounter category";

        [TestInitialize]
        [TestCleanup]
        public void CleanupCategories()
        {
            DeleteCounterCategory(firstCategory);
            DeleteCounterCategory(secondCategory);
        }

        [TestMethod]
        public void ShouldInstallFirstCategory()
        {
            PerformanceCountersInstaller installer = GetCommandLineConfiguredInstaller(firstCategory);
            DoCommitInstall(installer);

            PerformanceCounterCategory category = new PerformanceCounterCategory(firstCategory);

            AssertCategoryIsCorrect(category);
        }

        [TestMethod]
        public void ShouldInstallAndUninstallFirstCategory()
        {
            PerformanceCountersInstaller installer = GetCommandLineConfiguredInstaller(firstCategory);
            DoCommitInstall(installer);

            Assert.IsTrue(PerformanceCounterCategory.Exists(firstCategory));

            PerformanceCountersInstaller uninstaller = GetCommandLineConfiguredInstaller(firstCategory);
            uninstaller.Uninstall(null);

            Assert.IsFalse(PerformanceCounterCategory.Exists(firstCategory));
        }

        [TestMethod]
        public void ShouldInstallFirstAndSecondCategories()
        {
            PerformanceCountersInstaller installer = GetCommandLineConfiguredInstaller(firstCategory);
            DoCommitInstall(installer);

            PerformanceCountersInstaller installer2 = GetCommandLineConfiguredInstaller(secondCategory);
            DoCommitInstall(installer2);

            Assert.IsTrue(PerformanceCounterCategory.Exists(firstCategory));
            Assert.IsTrue(PerformanceCounterCategory.Exists(secondCategory));

            AssertCategoryIsCorrect(new PerformanceCounterCategory(secondCategory), secondCategory);
        }

        [TestMethod]
        public void ShouldBeAbleToRollbackInstallation()
        {
            PerformanceCountersInstaller installer = GetCommandLineConfiguredInstaller(firstCategory);
            Hashtable savedState = new Hashtable();
            try
            {
                installer.Install(savedState);
            }
            catch (SecurityException ex)
            {
                Assert.Inconclusive("In order to run the tests, please run Visual Studio as Administrator.\r\n{0}", ex.ToString());
            }
            installer.Rollback(savedState);

            Assert.IsFalse(PerformanceCounterCategory.Exists(secondCategory));
        }

        [TestMethod]
        public void ShouldBeAbleToSetCategoryViaConstructor()
        {
            PerformanceCountersInstaller installer = new PerformanceCountersInstaller(secondCategory);
            CommitInstall(installer);

            Assert.IsFalse(PerformanceCounterCategory.Exists(firstCategory));
            Assert.IsTrue(PerformanceCounterCategory.Exists(secondCategory));
        }

        [TestMethod]
        public void CategorySetInCodeOverridesCategorySetOnCommandLine()
        {
            PerformanceCountersInstaller installer = new PerformanceCountersInstaller(secondCategory);
            CommitInstall(installer, string.Format("/category={0}", firstCategory));

            Assert.IsFalse(PerformanceCounterCategory.Exists(firstCategory));
            Assert.IsTrue(PerformanceCounterCategory.Exists(secondCategory));
        }

        [TestMethod]
        public void CanCreateMultipleCategoriesWithOneInstaller()
        {
            PerformanceCountersInstaller installer =
                new PerformanceCountersInstaller(firstCategory, secondCategory);
            CommitInstall(installer);

            Assert.IsTrue(PerformanceCounterCategory.Exists(firstCategory));
            Assert.IsTrue(PerformanceCounterCategory.Exists(secondCategory));

            AssertCategoryIsCorrect(new PerformanceCounterCategory(firstCategory), firstCategory);
            AssertCategoryIsCorrect(new PerformanceCounterCategory(secondCategory), secondCategory);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldThrowIfNoCategories()
        {
            PerformanceCountersInstaller installer = new PerformanceCountersInstaller();
            CommitInstall(installer);
        }

        [TestMethod]
        public void CanCreateMultipleCategoriesFromTheCommandLine()
        {
            PerformanceCountersInstaller installer = new PerformanceCountersInstaller();
            CommitInstall(installer, string.Format("/category={0};{1}", firstCategory, secondCategory));

            Assert.IsTrue(PerformanceCounterCategory.Exists(firstCategory));
            Assert.IsTrue(PerformanceCounterCategory.Exists(secondCategory));
        }

        [TestMethod]
        public void CanCreateCategoriesFromConfiguration()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            PolicyData policyData = new PolicyData("Perfmon policy");
            //policyData.MatchingRules.Add(new TagAttributeMatchingRuleData("Match By Tag", "Perfmon"));
            PerformanceCounterCallHandlerData counterData = new PerformanceCounterCallHandlerData("{type}.{method}");
            counterData.CategoryName = firstCategory;
            policyData.Handlers.Add(counterData);
            settings.Policies.Add(policyData);

            DictionaryConfigurationSource configSource = new DictionaryConfigurationSource();
            configSource.Add(PolicyInjectionSettings.SectionName, settings);

            PerformanceCountersInstaller installer = new PerformanceCountersInstaller(configSource);
            CommitInstall(installer);

            Assert.IsTrue(PerformanceCounterCategory.Exists(firstCategory));
            Assert.IsFalse(PerformanceCounterCategory.Exists(secondCategory));
        }

        static void CommitInstall(System.Configuration.Install.Installer installer)
        {
            installer.Context = new InstallContext();
            DoCommitInstall(installer);
        }

        static void CommitInstall(System.Configuration.Install.Installer installer,
                                  params string[] args)
        {
            installer.Context = new InstallContext(null, args);
            DoCommitInstall(installer);
        }

        static void DoCommitInstall(System.Configuration.Install.Installer installer)
        {
            Hashtable savedData = new Hashtable();
            try
            {
                installer.Install(savedData);
            }
            catch (SecurityException ex)
            {
                Assert.Inconclusive("In order to run the tests, please run Visual Studio as Administrator.\r\n{0}", ex.ToString());
            }

            installer.Commit(savedData);
        }

        static PerformanceCountersInstaller GetCommandLineConfiguredInstaller(string category)
        {
            PerformanceCountersInstaller installer = new PerformanceCountersInstaller();
            string[] args = new string[] { string.Format("/category={0}", category) };
            InstallContext context = new InstallContext(null, args);
            installer.Context = context;
            return installer;
        }

        static void AssertCategoryIsCorrect(PerformanceCounterCategory category)
        {
            AssertCategoryIsCorrect(category, firstCategory);
        }

        static void AssertCategoryIsCorrect(PerformanceCounterCategory category,
                                            string categoryName)
        {
            Assert.IsNotNull(category);
            Assert.AreEqual(categoryName, category.CategoryName);
            Assert.AreEqual(PerformanceCounterCategoryType.MultiInstance, category.CategoryType);
            Assert.IsTrue(
                category.CounterExists(PerformanceCounterCallHandler.NumberOfCallsCounterName));
            Assert.IsTrue(
                category.CounterExists(PerformanceCounterCallHandler.CallsPerSecondCounterName));
            Assert.IsTrue(
                category.CounterExists(PerformanceCounterCallHandler.AverageCallDurationCounterName));
            Assert.IsTrue(
                category.CounterExists(
                    PerformanceCounterCallHandler.AverageCallDurationBaseCounterName));
            Assert.IsTrue(
                category.CounterExists(PerformanceCounterCallHandler.TotalExceptionsCounterName));
            Assert.IsTrue(
                category.CounterExists(PerformanceCounterCallHandler.ExceptionsPerSecondCounterName));
        }

        #region Perfcounter helper functions

        void DeleteCounterCategory(string categoryName)
        {
            try
            {
                PerformanceCounterCategory.Delete(categoryName);
            }
            catch (Win32Exception) {}
            catch (InvalidOperationException) {}
        }

        #endregion
    }
}
