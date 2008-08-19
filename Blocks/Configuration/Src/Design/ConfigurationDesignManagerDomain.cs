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
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    sealed class ConfigurationDesignManagerDomain
    {
        private IServiceProvider serviceProvider;		
        internal List<ConfigurationDesignManagerProxy> managers;

        internal ConfigurationDesignManagerDomain(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
			this.managers = new List<ConfigurationDesignManagerProxy>();
        }

        public void Load(Assembly assembly)
        {
            object[] typeAttributes = assembly.GetCustomAttributes(typeof(ConfigurationDesignManagerAttribute), true);
            foreach (ConfigurationDesignManagerAttribute typeAttribute in typeAttributes)
            {
                Load(typeAttribute);
            }
        }

		public void Open()
        {
			List<ConfigurationDesignManagerProxy> alreadyOpened = new List<ConfigurationDesignManagerProxy>();

			foreach (ConfigurationDesignManagerProxy manager in managers)
            {
				if (alreadyOpened.Contains(manager)) continue;
				if (manager.DependentTypes.Count > 0) LoadDependentTypes(alreadyOpened, manager);
                manager.Open(serviceProvider);
				alreadyOpened.Add(manager);
            }
        }


		public void Save()
        {
			List<ConfigurationDesignManagerProxy> alreadySaved = new List<ConfigurationDesignManagerProxy>();

			foreach (ConfigurationDesignManagerProxy manager in managers)
            {
				foreach (Type t in manager.DependentTypes)
				{
					ConfigurationDesignManagerProxy managerProxy = FindProxy(t, manager);
					if (alreadySaved.Contains(managerProxy)) continue;
					managerProxy.Save(serviceProvider);
					alreadySaved.Add(managerProxy);
				}
				if (alreadySaved.Contains(manager)) continue;
                manager.Save(serviceProvider);
				alreadySaved.Add(manager);
            }
        }

        public IConfigurationSource BuildConfigurationSource()
        {
			List<ConfigurationDesignManagerProxy> alreadyBuilt = new List<ConfigurationDesignManagerProxy>();

            DictionaryConfigurationSource dictionary = new DictionaryConfigurationSource();
			foreach (ConfigurationDesignManagerProxy manager in managers)
            {
				foreach (Type t in manager.DependentTypes)
				{
					ConfigurationDesignManagerProxy managerProxy = FindProxy(t, manager);
					if (alreadyBuilt.Contains(managerProxy)) continue;
					managerProxy.BuildConfigurationSource(serviceProvider, dictionary);
					alreadyBuilt.Add(managerProxy);
				}
				if (alreadyBuilt.Contains(manager)) continue;
                manager.BuildConfigurationSource(serviceProvider, dictionary);
				alreadyBuilt.Add(manager);
            }
            return dictionary;
        }

        public void Register()
        {
            foreach (ConfigurationDesignManagerProxy manager in managers)
            {
				foreach (Type t in manager.DependentTypes)
				{
					ConfigurationDesignManagerProxy managerProxy = FindProxy(t, manager);					
					managerProxy.Register(serviceProvider);					
				}				
				manager.Register(serviceProvider);				
            }
        }

		private void LoadDependentTypes(List<ConfigurationDesignManagerProxy> alreadyOpened, ConfigurationDesignManagerProxy manager)
		{
			foreach (Type t in manager.DependentTypes)
			{
				ConfigurationDesignManagerProxy managerProxy = FindProxy(t, manager);
				if (alreadyOpened.Contains(managerProxy)) continue;
				if (manager.DependentTypes.Count > 0) LoadDependentTypes(alreadyOpened, managerProxy);
				managerProxy.Open(serviceProvider);
				alreadyOpened.Add(managerProxy);
			}
		}

		private ConfigurationDesignManagerProxy FindProxy(Type t, ConfigurationDesignManagerProxy manager)
		{
			ConfigurationDesignManagerProxy managerProxy = managers.Find(delegate(ConfigurationDesignManagerProxy proxy)
																			   {
																				   if (proxy.ConfigurationDesignManager.GetType() == t)
																				   {
																					   return true;
																				   }
																				   return false;
																			   });
			if (null == managerProxy) throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture, Resources.ExceptionManagerDependencyNotFound, manager.ConfigurationDesignManager.GetType().Name, t.Name));
			return managerProxy;
		}

        /// <devdoc>
        /// Loads the IConfigurationDesignManager object from the specified ConfigurationDesignManagerAttribute.
        /// </devdoc>
        internal void Load(ConfigurationDesignManagerAttribute typeAttribute)
        {
            this.Load(typeAttribute.ConfigurationDesignManager, typeAttribute.DependentConfigurationDesignManager);
        }

        /// <devdoc>
        /// Loads the IConfigurationDesignManager object from the specified Type.
        /// </devdoc>
        internal void Load(Type type, Type dependentType)
        {
            ConfigurationDesignManagerProxy foundProxy = managers.Find(delegate(ConfigurationDesignManagerProxy proxyFromList) { return proxyFromList.ConfigurationDesignManager.GetType() == type; });

            ConfigurationDesignManagerProxy proxy = (foundProxy == null) ? new ConfigurationDesignManagerProxy(type) : foundProxy;
			if (null != dependentType) proxy.DependentTypes.Add(dependentType);
            Load(proxy);
        }

        internal void Load(ConfigurationDesignManagerProxy proxy)
        {
            if (!managers.Contains(proxy))
            {
                // source manager has to be first
                if (proxy.ConfigurationDesignManager.GetType().Equals(typeof(ConfigurationSourceConfigurationDesignManager)))
                {
                    this.managers.Insert(0, proxy);
                }
                else
                {
                    this.managers.Add(proxy);    
                }                
            }            
        }

        /// <devdoc>
        /// Loads all of the assemblies located at the specified path and searches them for ConfigurationDesignManagerAttribute attributes.
        /// </devdoc>
        internal void LoadFrom(string path)
        {
            string[] files = Directory.GetFiles(path, "*.dll");

            foreach (string file in files)
            {
                Assembly assembly = null;

               
                try
                {
                    AssemblyName assemblyName = AssemblyName.GetAssemblyName(file);
                    assembly = Assembly.Load(assemblyName);
                }
                catch
                {
                }
                if (assembly != null)
                {
                    this.Load(assembly);
                }
            }
        }
		
        /// <devdoc>
        /// Loads all of the assemblies located at the current AppDomain.BaseDirectory and searches them for ConfigurationDesignManagerAttribute attributes.
        /// </devdoc>
        internal void Load()
        {
            IPluginDirectoryProvider pluginDirectoryProvider = ServiceHelper.GetPluginDirectoryService(serviceProvider);
            LoadFrom(pluginDirectoryProvider.PluginDirectory);
        }
    }
}