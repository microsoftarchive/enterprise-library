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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
    public abstract class Given_HierarchicalConfigurationSourceWithEventListener : Given_HierarchicalConfigurationSourceHandler
    {
        protected ConfigurationSourceEventListener LocalSourceEventListener;
        protected SectionListener DummySectionListener;

        protected override ConfigurationSection Arrange_GetParentSourceSection()
        {
            return new DummySectionWithCollections { Name = "parent", Value = 12 };
        }

        protected override ConfigurationSection Arrange_GetLocalSourceSection()
        {
            return new DummySectionWithCollections { Name = "local" };
        }

        protected override void Arrange()
        {
            base.Arrange();

            LocalSourceEventListener = new ConfigurationSourceEventListener(base.LocalSource);
            DummySectionListener = LocalSourceEventListener.ListenForSectionChanges(base.SectionName);
        }

        protected class ConfigurationSourceEventListener
        {
            public int ConfigurationSourceChangedRaiseCount;
            public ConfigurationSourceChangedEventArgs LastConfigurationSourceChangedEventArgs;

            IConfigurationSource configurationSourcetoListenTo;

            public ConfigurationSourceEventListener(IConfigurationSource configurationSourcetoListenTo)
            {
                this.configurationSourcetoListenTo = configurationSourcetoListenTo;
                configurationSourcetoListenTo.SourceChanged += new EventHandler<ConfigurationSourceChangedEventArgs>(configurationSourcetoListenTo_SourceChanged);
            }

            void configurationSourcetoListenTo_SourceChanged(object sender, ConfigurationSourceChangedEventArgs e)
            {
                ConfigurationSourceChangedRaiseCount++;
                LastConfigurationSourceChangedEventArgs = e;
            }

            Dictionary<string, SectionListener> sectionListeners = new Dictionary<string, SectionListener>();
            public SectionListener ListenForSectionChanges(string sectionName)
            {
                sectionListeners.Add(sectionName, new SectionListener());
                this.configurationSourcetoListenTo.AddSectionChangeHandler(sectionName, (sender, args) =>
                {
                    sectionListeners[args.SectionName].SectionChangedRaiseCount++;
                });

                return sectionListeners[sectionName];
            }
        }

        protected class SectionListener
        {
            public int SectionChangedRaiseCount = 0;
        }
    }

    [TestClass]
    public class When_ParentConfigurationSourceSectionChanges : Given_HierarchicalConfigurationSourceWithEventListener
    {
        protected override void Act()
        {
            //wire up
            var section = base.GetMergedSection();

            ParentSource.DoSourceSectionChanged(base.SectionName);
        }

        [TestMethod]
        public void Then_LocalSourceFiresSectionChangedEvent()
        {
            Assert.AreEqual(1, DummySectionListener.SectionChangedRaiseCount);
        }
    }


    [TestClass]
    public class When_ParentConfigurationSourceChanges : Given_HierarchicalConfigurationSourceWithEventListener
    {
        protected override void Act()
        {
            //wire up
            var section = base.GetMergedSection();
            ParentSource.Remove(SectionName);
            ParentSource.Add(SectionName, new DummySectionWithCollections { Name = "parent", Value = 33 });
            ParentSource.DoSourceChanged(new []{base.SectionName});
        }

        [TestMethod]
        public void Then_LocalSourceFiresSectionChangedEvent()
        {
            Assert.AreEqual(1, LocalSourceEventListener.ConfigurationSourceChangedRaiseCount);
        }

        [TestMethod]
        public void Then_LocalSourceReflectChangesInParent()
        {
            var section = GetMergedSection();
            Assert.AreEqual(33, section.Value);
        }

    }
}
