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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Indicates the <see cref="IConfigurationDesignManager"/> defined in an assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple=true)]
    public sealed class ConfigurationDesignManagerAttribute : Attribute
    {
        private readonly Type configurationDesignManager;
		private readonly Type dependentConfigurationDesignManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationDesignManagerAttribute"/> class with a <see cref="Type"/> implementing <see cref="IConfigurationDesignManager"/>.
        /// </summary>
        /// <param name="configurationDesignManager">
        /// A <see cref="Type"/> implementing <see cref="IConfigurationDesignManager"/>.
        /// </param>        
        public ConfigurationDesignManagerAttribute(Type configurationDesignManager) : this(configurationDesignManager, null)
        {            
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationDesignManagerAttribute"/> class with a <see cref="Type"/> implementing <see cref="IConfigurationDesignManager"/> and a dependent <see cref="IConfigurationDesignManager"/>.
		/// </summary>
		/// <param name="configurationDesignManager">
		/// A <see cref="Type"/> implementing <see cref="IConfigurationDesignManager"/>.
		/// </param> 
		/// <param name="dependentConfigurationDesignManager">
		/// The <see cref="Type"/> of the dependent <see cref="IConfigurationDesignManager"/>.
		/// </param>
		public ConfigurationDesignManagerAttribute(Type configurationDesignManager, Type dependentConfigurationDesignManager)
		{
			this.configurationDesignManager = configurationDesignManager;
			this.dependentConfigurationDesignManager = dependentConfigurationDesignManager;
		}

        /// <summary>
        /// Gets the <see cref="Type"/> implementing <see cref="IConfigurationDesignManager"/>.
        /// </summary>
        /// <value>
        /// The <see cref="Type"/> implementing <see cref="IConfigurationDesignManager"/>
        /// </value>
        public Type ConfigurationDesignManager
        {
            get { return configurationDesignManager; }
        }


		/// <summary>
		/// Gets the <see cref="Type"/> of the dependent <see cref="IConfigurationDesignManager"/>.
		/// </summary>
		/// <value>
		/// The <see cref="Type"/> of the dependent <see cref="IConfigurationDesignManager"/>.
		/// </value>
		public Type DependentConfigurationDesignManager
		{
			get { return dependentConfigurationDesignManager;  }
		}
    }
}