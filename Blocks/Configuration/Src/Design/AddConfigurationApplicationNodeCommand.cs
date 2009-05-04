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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
	/// Represents a command for adding a new <see cref="ConfigurationApplicationNode"/>.
    /// </summary>
    public class AddConfigurationApplicationNodeCommand : ConfigurationNodeCommand
    {
        private readonly ConfigurationApplicationFile applicationData;

        /// <summary>
		/// Initialize a new instance of the <see cref="AddConfigurationApplicationNodeCommand"/> class with an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        public AddConfigurationApplicationNodeCommand(IServiceProvider serviceProvider) : this(serviceProvider, true)
        {
        }

        /// <summary>
		/// Initialize a new instance of the <see cref="AddConfigurationApplicationNodeCommand"/> class with an <see cref="IServiceProvider"/> and if the error service should be cleared after the command executes.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        /// <param name="clearErrorService">
        /// Determines if all the messages in the <see cref="IErrorLogService"/> should be cleared when the command has executed.
        /// </param>
        public AddConfigurationApplicationNodeCommand(IServiceProvider serviceProvider, bool clearErrorService) : base(serviceProvider, clearErrorService)
        {
            applicationData = new ConfigurationApplicationFile();
        }        

        /// <summary>
		/// Creates a new <see cref="ConfigurationApplicationNode"/> object and adds it to the solution.
        /// </summary>
        /// <param name="node">
		/// The <see cref="ConfigurationApplicationNode"/> is the root of an <see cref="IConfigurationUIHierarchy"/> so the <paramref name="node"/> is not used, so passing <see langword="null"/> is expected.
        /// </param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
		protected override void ExecuteCore(ConfigurationNode node)
        {
            try
            {
                UIService.BeginUpdate();
				ConfigurationApplicationNode appNode = new ConfigurationApplicationNode(applicationData);
                IConfigurationUIHierarchy hierarchy = new ConfigurationUIHierarchy(appNode, ServiceProvider);
                ConfigurationUIHierarchyService.AddHierarchy(hierarchy);
				hierarchy.Load();
                UIService.SetUIDirty(hierarchy);
            }
            finally
            {
                UIService.EndUpdate();
            }
        }
    }
}
