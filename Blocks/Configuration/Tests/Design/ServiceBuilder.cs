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
using System.Text;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
	class ServiceBuilder
	{
		public static ServiceContainer Build()
		{
			ServiceContainer container = new ServiceContainer();
			NodeNameCreationService nodeNameCreationService = new NodeNameCreationService();
            ConfigurationUIHierarchyService configurationUIHierarchy = new ConfigurationUIHierarchyService();
			container.AddService(typeof(INodeNameCreationService), nodeNameCreationService);
            container.AddService(typeof(IConfigurationUIHierarchyService), configurationUIHierarchy);
			container.AddService(typeof(IUIService), new MockUIService());
			container.AddService(typeof(IErrorLogService), new ErrorLogService());
			container.AddService(typeof(INodeCreationService), new NodeCreationService());
            container.AddService(typeof(IUICommandService), new UICommandService(configurationUIHierarchy));
            container.AddService(typeof(IStorageService), new StorageService());
            container.AddService(typeof(IPluginDirectoryProvider), new AppDomainBasePluginDirectoryProvider());
			return container;
		}

        class AppDomainBasePluginDirectoryProvider : IPluginDirectoryProvider
        {
            #region IPluginDirectoryProvider Members

            /// <summary>
            /// TODO: ADD COMMENT
            /// </summary>
            public string PluginDirectory
            {
                get { return AppDomain.CurrentDomain.BaseDirectory; }
            }

            #endregion
        }
	}
}
