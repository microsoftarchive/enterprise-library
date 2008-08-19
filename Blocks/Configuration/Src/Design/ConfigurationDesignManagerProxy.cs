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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	sealed class ConfigurationDesignManagerProxy : IConfigurationDesignManager, IEquatable<ConfigurationDesignManagerProxy>
    {
        private IConfigurationDesignManager manager;
		private List<Type> dependentTypes;
		private bool registered;		

		/// <summary>
		/// 
		/// </summary>
		/// <param name="managerType"></param>
        public ConfigurationDesignManagerProxy(Type managerType)
        {
            if (managerType == null) throw new ArgumentNullException("managerType");

			dependentTypes = new List<Type>();
            manager = Activator.CreateInstance(managerType) as IConfigurationDesignManager;
            if (this.manager == null)
            {
                throw new ArgumentException(string.Format(Resources.Culture, Resources.ExceptionNotAssignableType, managerType.AssemblyQualifiedName, typeof(IConfigurationDesignManager).AssemblyQualifiedName));
            }
        }

		public IConfigurationDesignManager ConfigurationDesignManager
        {
            get { return manager; }
        }

		public List<Type> DependentTypes
		{
			get { return dependentTypes;  }
		}

		public void Register(IServiceProvider serviceProvider)
        {
			if (registered) return;

            manager.Register(serviceProvider);
			registered = true;
        }

		public void Save(IServiceProvider serviceProvider)
        {
            manager.Save(serviceProvider);
        }

		public void Open(IServiceProvider serviceProvider)
        {
            manager.Open(serviceProvider);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <param name="dictionaryConfigurationSource"></param>
        public void BuildConfigurationSource(IServiceProvider serviceProvider, DictionaryConfigurationSource dictionaryConfigurationSource)
        {
            manager.BuildConfigurationSource(serviceProvider, dictionaryConfigurationSource);
        }

		public bool Equals(ConfigurationDesignManagerProxy other)
		{
			if (null == other) throw new ArgumentNullException("other");

			if (ConfigurationDesignManager.GetType() == other.ConfigurationDesignManager.GetType())
			{
				return true;
			}
			return false;
		}
	}
}