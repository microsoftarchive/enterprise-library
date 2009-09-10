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
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.ComponentModel.Design.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// A utility class to help with common service related activities.
	/// </summary>
    public static class ServiceHelper
    {
		/// <summary>
		/// Log an error to the <see cref="IErrorLogService"/>.
		/// </summary>
		/// <param name="serviceProvider">
		/// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </param>
		/// <param name="node">The configuration node associated with the error.</param>
		/// <param name="e">The <see cref="Exception"/> to log.</param>
		public static void LogError(IServiceProvider serviceProvider, ConfigurationNode node, Exception e)
		{
			Exception exception = e;
			while (exception != null)
			{
				LogError(serviceProvider, new ConfigurationError(node, exception.Message));
				exception = exception.InnerException;
			}
		}

		/// <summary>
		/// Log a <see cref="ValidationError"/> to the <see cref="IErrorLogService"/>.
		/// </summary>
		/// <param name="serviceProvider">
		/// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </param>
		/// <param name="error">
		/// The <see cref="ValidationError"/> to log.
		/// </param>
		public static void LogError(IServiceProvider serviceProvider, ValidationError error)
		{
			if (null == serviceProvider) throw new ArgumentNullException("serviceProvider");

			IErrorLogService errorLogService = GetErrorService(serviceProvider);
			errorLogService.LogError(error);
		}

		/// <summary>
		/// Log a <see cref="ConfigurationError"/> to the <see cref="IErrorLogService"/>.
		/// </summary>
		/// <param name="serviceProvider">
		/// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </param>
		/// <param name="error">
		/// The <see cref="ConfigurationError"/> to log.
		/// </param>
		public static void LogError(IServiceProvider serviceProvider, ConfigurationError error)
		{
			if (null == serviceProvider) throw new ArgumentNullException("serviceProvider");

			IErrorLogService errorLogService = GetErrorService(serviceProvider);
			errorLogService.LogError(error);
		}

		/// <summary>
		/// Display the errors from the <see cref="IErrorLogService"/> in the user interface.
		/// </summary>
		/// <param name="serviceProvider">
		/// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </param>
		public static void DisplayErrors(IServiceProvider serviceProvider)
		{
			if (null == serviceProvider) throw new ArgumentNullException("serviceProvider");

			IUIService uiService = GetUIService(serviceProvider);
			uiService.DisplayErrorLog(GetErrorService(serviceProvider));
		}

		/// <summary>
		/// Gets the registered <see cref="IUIService"/>.
		/// </summary>
		/// <param name="serviceProvider">
		/// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </param>
		/// <returns>The registered <see cref="IUIService"/>.</returns>
		public static IUIService GetUIService(IServiceProvider serviceProvider)
		{
			if (null == serviceProvider) throw new ArgumentNullException("serviceProvider");

			IUIService uiService = serviceProvider.GetService(typeof(IUIService)) as IUIService;
			return uiService;
		}

		/// <summary>
		/// Gets the registered <see cref="IConfigurationUIHierarchyService"/>.
		/// </summary>
		/// <param name="serviceProvider">
		/// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </param>
		/// <returns>The registered <see cref="IConfigurationUIHierarchyService"/>.</returns>
        public static IConfigurationUIHierarchyService GetUIHierarchyService(IServiceProvider serviceProvider)
        {
			if (null == serviceProvider) throw new ArgumentNullException("serviceProvider");

            IConfigurationUIHierarchyService hierarchyService = serviceProvider.GetService(typeof(IConfigurationUIHierarchyService)) as IConfigurationUIHierarchyService;            
            return hierarchyService;
        }

		/// <summary>
		/// Gets the current selected <see cref="IConfigurationUIHierarchy"/>.
		/// </summary>
		/// <param name="serviceProvider">
		/// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </param>
		/// <returns>The current selected <see cref="IConfigurationUIHierarchy"/>.</returns>
        public static IConfigurationUIHierarchy GetCurrentHierarchy(IServiceProvider serviceProvider)
        {
			if (null == serviceProvider) throw new ArgumentNullException("serviceProvider");

            IConfigurationUIHierarchyService uiHierarchyService = GetUIHierarchyService(serviceProvider);
            return uiHierarchyService.SelectedHierarchy;
        }		

        /// <summary>
		/// Gets the current selected <see cref="IConfigurationUIHierarchy"/>'s <see cref="IConfigurationSource"/>.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
		/// <returns>The current selected <see cref="IConfigurationUIHierarchy"/>'s <see cref="IConfigurationSource"/>.</returns>
		public static IConfigurationSource GetCurrentConfigurationSource(IServiceProvider serviceProvider)
		{
			if (null == serviceProvider) throw new ArgumentNullException("serviceProvider");

			return GetCurrentHierarchy(serviceProvider).ConfigurationSource;
		}

        /// <summary>
        /// Gets the registered <see cref="INodeNameCreationService"/>.
        /// </summary>
		/// <param name="serviceProvider">
		/// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </param>
        /// <returns>The registered <see cref="INodeNameCreationService"/>.</returns>
		public static INodeNameCreationService GetNameCreationService(IServiceProvider serviceProvider)
		{
			if (null == serviceProvider) throw new ArgumentNullException("serviceProvider");

			INodeNameCreationService nameCreationService = serviceProvider.GetService(typeof(INodeNameCreationService)) as INodeNameCreationService;
			return nameCreationService;
		}

		/// <summary>
		/// Gets the registered <see cref="IStorageService"/>.
		/// </summary>
		/// <param name="serviceProvider">
		/// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </param>
		/// <returns>The registered <see cref="IStorageService"/>.</returns>
		public static IStorageService GetCurrentStorageService(IServiceProvider serviceProvider)
		{
			if (null == serviceProvider) throw new ArgumentNullException("serviceProvider");

			return GetCurrentHierarchy(serviceProvider).StorageService;
		}

		/// <summary>
		/// Gets the current selected <see cref="IConfigurationUIHierarchy"/>'s root <see cref="ConfigurationApplicationNode"/>.
		/// </summary>
		/// <param name="serviceProvider">
		/// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </param>
		/// <returns>The current selected <see cref="IConfigurationUIHierarchy"/>'s root <see cref="ConfigurationApplicationNode"/>.</returns>
		public static ConfigurationApplicationNode GetCurrentRootNode(IServiceProvider serviceProvider)
		{
			if (null == serviceProvider) throw new ArgumentNullException("serviceProvider");

			return GetCurrentHierarchy(serviceProvider).RootNode;
		}

		/// <summary>
		/// Gets the file associated with the currently selected root node.
		/// </summary>
		/// <param name="serviceProvider">
		/// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </param>
		/// <returns>The file associated with the currently selected root node.</returns>
		public static string GetApplicationConfigurationFile(IServiceProvider serviceProvider)
		{
			if (null == serviceProvider) throw new ArgumentNullException("serviceProvider");

			return GetCurrentRootNode(serviceProvider).ConfigurationFile;
		}

		/// <summary>
		/// Gets the registered <see cref="IUICommandService"/>.
		/// </summary>
		/// <param name="serviceProvider">
		/// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </param>
		/// <returns>The registered <see cref="IUICommandService"/>.</returns>
		public static IUICommandService GetUICommandService(IServiceProvider serviceProvider)
		{
			if (null == serviceProvider) throw new ArgumentNullException("serviceProvider");

			IUICommandService menuService = serviceProvider.GetService(typeof(IUICommandService)) as IUICommandService;
			return menuService;
		}

		/// <summary>
		/// Gets the registered <see cref="IErrorLogService"/>.
		/// </summary>
		/// <param name="serviceProvider">
		/// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </param>
		/// <returns>The registered <see cref="IErrorLogService"/>.</returns>
		public static IErrorLogService GetErrorService(IServiceProvider serviceProvider)
		{
			if (null == serviceProvider) throw new ArgumentNullException("serviceProvider");

			IErrorLogService errorLogService = serviceProvider.GetService(typeof(IErrorLogService)) as IErrorLogService;
			return errorLogService;
		}

		/// <summary>
		/// Gets the registered <see cref="INodeCreationService"/>.
		/// </summary>
		/// <param name="serviceProvider">
		/// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </param>
		/// <returns>The registered <see cref="INodeCreationService"/>.</returns>
        public static INodeCreationService GetNodeCreationService(IServiceProvider serviceProvider)
		{
			if (null == serviceProvider) throw new ArgumentNullException("serviceProvider");

			INodeCreationService nodeCreationService = serviceProvider.GetService(typeof(INodeCreationService)) as INodeCreationService;
			return nodeCreationService;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IPluginDirectoryProvider GetPluginDirectoryService(IServiceProvider serviceProvider)
        {
            if (null == serviceProvider) throw new ArgumentNullException("serviceProvider");

            return (IPluginDirectoryProvider) serviceProvider.GetService(typeof(IPluginDirectoryProvider));
        }
    }
}
