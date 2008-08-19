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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// TODO
    /// </summary>
    public class SaveAsConfigurationApplicationNodeCommand : SaveConfigurationApplicationNodeCommand
    {
        string filePath;
        bool updateApplicationNode;
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="filePath"></param>
        public SaveAsConfigurationApplicationNodeCommand(IServiceProvider serviceProvider, string filePath)
            :this(serviceProvider, filePath, true)
        {
            this.filePath = filePath;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="filePath"></param>
        /// <param name="updateApplicationNode"></param>
        public SaveAsConfigurationApplicationNodeCommand(IServiceProvider serviceProvider, string filePath, bool updateApplicationNode)
            : base(serviceProvider)
        {
            this.filePath = filePath;
            this.updateApplicationNode = updateApplicationNode;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="clearErrorLog"></param>
        /// <param name="filePath"></param>
        public SaveAsConfigurationApplicationNodeCommand(IServiceProvider serviceProvider, bool clearErrorLog, string filePath)
            : this(serviceProvider, clearErrorLog, filePath, true)
        {
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="clearErrorLog"></param>
        /// <param name="filePath"></param>
        /// <param name="updateApplicationNode"></param>
        public SaveAsConfigurationApplicationNodeCommand(IServiceProvider serviceProvider, bool clearErrorLog, string filePath, bool updateApplicationNode)
            : base(serviceProvider, clearErrorLog)
        {
            this.updateApplicationNode = updateApplicationNode;
            this.filePath = filePath;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="node"></param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            ConfigurationApplicationNode applicationNode = ServiceHelper.GetCurrentRootNode(ServiceProvider);
            string previousConfigurationFile = applicationNode.ConfigurationFile;
            applicationNode.ConfigurationFile = filePath;
            try
            {
                base.ExecuteCore(node);
            }
            finally
            {
                if (!updateApplicationNode)
                {
                    applicationNode.ConfigurationFile = previousConfigurationFile;
                }
            }
        }
    }
}
