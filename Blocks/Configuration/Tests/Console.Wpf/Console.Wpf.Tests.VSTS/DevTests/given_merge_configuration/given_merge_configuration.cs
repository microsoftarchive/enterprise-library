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
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using System.Configuration;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_merge_configuration
{

    public abstract class given_configuration_merge : ArrangeActAssert
    {
        protected EnvironmentMergeSection MergeSection;
        protected string TargetFile;

        protected override void Arrange()
        {
            TargetFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("{0}.config", Guid.NewGuid()));

            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(
                        new ExeConfigurationFileMap
                        {
                            ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "merge_environment.dconfig")
                        }, ConfigurationUserLevel.None);

            MergeSection = (EnvironmentMergeSection)configuration.GetSection(EnvironmentMergeSection.EnvironmentMergeData);
        }

        protected override void Teardown()
        {
            File.Delete(TargetFile);
        }

    }

    [TestClass]
    [DeploymentItem("merge_environment.dconfig")]
    [DeploymentItem("merge_main.config")]
    public class when_merging_configuration : given_configuration_merge
    {
        protected override void Act()
        {
            ConfigurationMerger configurationMerger = new ConfigurationMerger();
            configurationMerger.MergeConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "merge_main.config"), MergeSection, TargetFile);
        }


        [TestMethod]
        public void then_file_was_written_to()
        {
            Assert.IsTrue(File.Exists(TargetFile));
        }

        
        [TestMethod]
        public void then_file_has_overridden_values()
        {
            FileConfigurationSource source = new FileConfigurationSource(TargetFile);
            CacheManagerSettings settings = (CacheManagerSettings)source.GetSection(CacheManagerSettings.SectionName);
            CacheManagerData cacheManager = (CacheManagerData)settings.CacheManagers.Get("Cache Manager");

            Assert.AreEqual(1, cacheManager.MaximumElementsInCacheBeforeScavenging);
            Assert.AreEqual(1, cacheManager.NumberToRemoveWhenScavenging);
            Assert.AreEqual(1, cacheManager.ExpirationPollFrequencyInSeconds);
        }
    }

    [TestClass]
    [DeploymentItem("merge_environment.dconfig")]
    [DeploymentItem("merge_main.config")]
    public class when_merging_configuration_and_target_file_exists : given_configuration_merge
    {
        DateTime lastWriteTime;

        protected override void Act()
        {
            lastWriteTime = DateTime.Today.Subtract(TimeSpan.FromMinutes(5));
            
            File.Create(TargetFile).Dispose();
            File.SetLastWriteTime(TargetFile, lastWriteTime);

            ConfigurationMerger configurationMerger = new ConfigurationMerger();
            configurationMerger.MergeConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "merge_main.config"), MergeSection, TargetFile);
        }


        [TestMethod]
        public void then_file_was_written_to()
        {
            Assert.AreNotEqual(lastWriteTime, File.GetLastWriteTime(TargetFile));
        }
    }


    [TestClass]
    [DeploymentItem("merge_environment.dconfig")]
    [DeploymentItem("merge_main.config")]
    public class when_merging_configuration_and_source_file_is_readonly : given_configuration_merge
    {
        string sourceFile;
        protected override void Act()
        {
            sourceFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "merge_main.config");
            File.SetAttributes(sourceFile, FileAttributes.ReadOnly);

            ConfigurationMerger configurationMerger = new ConfigurationMerger();
            configurationMerger.MergeConfiguration(sourceFile, MergeSection, TargetFile);
        }

        [TestMethod]
        public void then_file_was_written_to()
        {
            Assert.IsTrue(File.Exists(TargetFile));
        }

        protected override void Teardown()
        {
            File.SetAttributes(sourceFile, FileAttributes.Normal);
            base.Teardown();
        }
    }

    [TestClass]
    [DeploymentItem("merge_environment.dconfig")]
    [DeploymentItem("merge_main.config")]
    public class when_merging_configuration_and_exception_occurs : given_configuration_merge
    {
        DateTime lastWriteTime;
        FileStream openFile;
        protected override void Act()
        {
            openFile = File.Create(TargetFile);
            lastWriteTime = File.GetLastWriteTime(TargetFile);

            try
            {
                ConfigurationMerger configurationMerger = new ConfigurationMerger();
                configurationMerger.MergeConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "merge_main.config"), MergeSection, TargetFile);
            }
            catch (IOException)
            {
            }
        }

        [TestMethod]
        public void then_file_was_not_written_to()
        {
            Assert.AreEqual(lastWriteTime, File.GetLastWriteTime(TargetFile));
        }

        protected override void Teardown()
        {
            openFile.Dispose();
            base.Teardown();
        }
    }
}
