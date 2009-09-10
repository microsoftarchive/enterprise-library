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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport
{
    public abstract class ConfigurationDesignHost
    {
        ConfigurationApplicationNode appNode;
        IConfigurationUIHierarchy hierarchy;
        IServiceProvider serviceProvider;

        protected ConfigurationApplicationNode ApplicationNode
        {
            get { return appNode; }
        }

        protected IErrorLogService ErrorLogService
        {
            get { return (IErrorLogService)serviceProvider.GetService(typeof(IErrorLogService)); }
        }

        protected IConfigurationUIHierarchyService HiearchyService
        {
            get { return (IConfigurationUIHierarchyService)serviceProvider.GetService(typeof(IConfigurationUIHierarchyService)); }
        }

        protected IConfigurationUIHierarchy Hierarchy
        {
            get { return hierarchy; }
        }

        protected INodeCreationService NodeCreationService
        {
            get { return (INodeCreationService)serviceProvider.GetService(typeof(INodeCreationService)); }
        }

        protected IServiceProvider ServiceProvider
        {
            get { return serviceProvider; }
        }

        protected IUICommandService UICommandService
        {
            get { return (IUICommandService)serviceProvider.GetService(typeof(IUICommandService)); }
        }

        protected IUIService UIService
        {
            get { return (IUIService)serviceProvider.GetService(typeof(IUIService)); }
        }

        public virtual void AfterCleanup() {}
        public virtual void AfterSetup() {}
        public virtual void BeforeCleanup() {}
        public virtual void BeforeSetup() {}

        protected virtual void CleanupCore() {}

        protected virtual void InitializeCore() {}

        protected void SetDictionaryConfigurationSource(DictionaryConfigurationSource configurationSource)
        {
            ConfigurationSourceSectionNode configurationSourceSection = new ConfigurationSourceSectionNode();
            DictionarySourceElementNode configurationSourceNode = new DictionarySourceElementNode(configurationSource);
            ApplicationNode.AddNode(configurationSourceSection);

            configurationSourceSection.AddNode(configurationSourceNode);
            configurationSourceSection.SelectedSource = configurationSourceNode;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            BeforeCleanup();

            CleanupCore();
            hierarchy.Dispose();
            IDisposable disposableServiceProvider = serviceProvider as IDisposable;
            if (null != disposableServiceProvider) disposableServiceProvider.Dispose();

            AfterCleanup();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            BeforeSetup();

            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
            appNode = new ConfigurationApplicationNode(ConfigurationApplicationFile.FromCurrentAppDomain());
            serviceProvider = ServiceBuilder.Build();
            hierarchy = new ConfigurationUIHierarchy(appNode, serviceProvider);
            ServiceHelper.GetUIHierarchyService(ServiceProvider).SelectedHierarchy = Hierarchy;
            hierarchy.Load();
            InitializeCore();

            AfterSetup();
        }

        class DictionarySourceElementNode : ConfigurationSourceElementNode
        {
            DictionaryConfigurationSource source;

            public DictionarySourceElementNode(DictionaryConfigurationSource configurationSource)
                : base("dictionary source")
            {
                source = configurationSource;
            }

            public override IConfigurationSource ConfigurationSource
            {
                get { return source; }
            }

            public override ConfigurationSourceElement ConfigurationSourceElement
            {
                get { return null; }
            }
        }
    }
}
