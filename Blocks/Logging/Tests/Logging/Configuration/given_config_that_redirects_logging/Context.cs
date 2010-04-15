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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests.ConfigFiles;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration.given_config_that_redirects_logging
{
    // Set up context for these tests.
    // This isn't really a logging block test, it's testing redirected
    // sections. However, it's a scenario that's most easily popped with the
    // logging block. It's here because almost none of the common DLL tests references
    // other blocks, and that's a good thing.
    public abstract class Context : ArrangeActAssert
    {
        protected LogWriter LogWriter { get; private set; }
        protected IConfigurationSource RootConfiguration { get; private set; }
        protected const string LogFileName = "redirectedLogging.log";
        protected const string SecondSourceName = "Second Source";

        private IUnityContainer container;

        private readonly string[] resourceFiles = {
            "ConfigThatRedirectsLoggingSection.config",
            "ConfigRedirectedTo.config"
        };

        protected override void Arrange()
        {
            base.Arrange();

            ResetDiskContents();
            DumpResourcesToDisk();
            InitializeContainer();
            LogWriter = container.Resolve<LogWriter>();
        }

        protected override void Teardown()
        {
            if(container != null)
            {
                container.Dispose();
            }
            RootConfiguration.Dispose();
            container = null;
            RootConfiguration = null;
            base.Teardown();
        }

        protected void CloseLogFile()
        {
            if (container != null)
            {
                container.Dispose();
                container = null;
            }
        }

        private void ResetDiskContents()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;

            var filesToDelete = resourceFiles.Concat(new[] {LogFileName})
                .Select(fn => Path.Combine(dir, fn));

            foreach(var file in filesToDelete)
            {
                if(File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }

        private void DumpResourcesToDisk()
        {
            var resourceDumper = new ResourceHelper<ConfigFileLocator>();
            foreach(var resourceFile in resourceFiles)
            {
                resourceDumper.DumpResourceFileToDisk(resourceFile);
            }
        }

        private void InitializeContainer()
        {
            RootConfiguration = new FileConfigurationSource(resourceFiles[0], true, 500);
            container = new UnityContainer();
            EnterpriseLibraryContainer.ConfigureContainer(new UnityContainerConfigurator(container), RootConfiguration);
        }

        protected void CopyData<TItem>(IEnumerable<TItem> sourceCollection, Action<TItem> addAction)
        {
            foreach(var item in sourceCollection)
            {
                addAction(item);
            }
        }
    }
}
