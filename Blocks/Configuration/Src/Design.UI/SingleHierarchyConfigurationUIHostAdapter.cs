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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.HostAdapter;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class SingleHierarchyConfigurationUIHostAdapter : ISingleHierarchyConfigurationUIHostAdapter
    {
        ConfigurationEditor configurationEditor;
        bool loading = false;
        IServiceProvider serviceProvider;

        /// <summary>
        /// 
        /// </summary>
        public SingleHierarchyConfigurationUIHostAdapter(HostAdapterConfiguration hostConfiguration)
        {
            ServiceContainer container = new ServiceContainer();
            NodeNameCreationService nodeNameCreationService = new NodeNameCreationService();
            ConfigurationUIHierarchyService configurationUIHierarchy = new ConfigurationUIHierarchyService();
            configurationUIHierarchy.HierarchyRemoved += new EventHandler<HierarchyRemovedEventArgs>(configurationUIHierarchyRemoved);
            ComponentChangeService componentChangeService = new ComponentChangeService();

            componentChangeService.ComponentChanged += new ComponentChangedEventHandler(OnComponentChanged);

            container.AddService(typeof(IComponentChangeService), componentChangeService);
            container.AddService(typeof(INodeNameCreationService), nodeNameCreationService);
            container.AddService(typeof(IConfigurationUIHierarchyService), configurationUIHierarchy);
            container.AddService(typeof(IErrorLogService), new ErrorLogService());
            container.AddService(typeof(INodeCreationService), new NodeCreationService());
            container.AddService(typeof(IUICommandService), new UICommandService(configurationUIHierarchy));
            container.AddService(typeof(IPluginDirectoryProvider), new PluginDirectoryProvider(hostConfiguration.PluginDirectory));

            configurationEditor = new ConfigurationEditor(this, container);
            container.AddService(typeof(IUIService), configurationEditor);

            serviceProvider = container;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UserControl EditorControl
        {
            get { return configurationEditor; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDirty
        {
            get
            {
                IUIService uiService = ServiceHelper.GetUIService(serviceProvider);
                IConfigurationUIHierarchyService uiHierarchyService = ServiceHelper.GetUIHierarchyService(serviceProvider);
                foreach (IConfigurationUIHierarchy hierarchy in uiHierarchyService.GetAllHierarchies())
                {
                    if (uiService.IsDirty(hierarchy))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        void configurationUIHierarchyRemoved(object sender,
                                             HierarchyRemovedEventArgs e)
        {
            if (!loading)
            {
                if (configurationEditor != null && configurationEditor.MainHierarchy.Id == e.UIHierarchy.Id)
                {
                    DoDocumentClosed();
                }
            }
        }

        /// <summary>
        /// Event fired when a document is closed.
        /// </summary>
        public event EventHandler<EventArgs> DocumentClosed;

        internal void DoDocumentClosed()
        {
            if (DocumentClosed != null)
            {
                DocumentClosed(this, EventArgs.Empty);
            }
        }

        internal void DoSelectionChanged(IComponent selectedComponent)
        {
            if (SelectionChanged != null)
            {
                SelectionChanged(this, new SelectionChangedEventArgs(selectedComponent));
            }
        }

        internal void DoStatusTextChanged(string statusText)
        {
            if (StatusTextChanged != null)
            {
                StatusTextChanged(this, new StatusTextChangedEventArgs(statusText));
            }
        }

        internal void DoTasksChanged(List<Task> tasks)
        {
            if (TasksChanged != null)
            {
                TasksChanged(this, new TasksChangedEventArgs(tasks));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            return serviceProvider.GetService(serviceType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurationFile"></param>
        public void Load(string configurationFile)
        {
            loading = true;
            try
            {
                FileConfigurationSource.ResetImplementation(configurationFile, false);

                IConfigurationUIHierarchyService hierarchyService = (IConfigurationUIHierarchyService)serviceProvider.GetService(typeof(IConfigurationUIHierarchyService));

                foreach (ConfigurationUIHierarchy hierarchy in hierarchyService.GetAllHierarchies())
                {
                    hierarchyService.RemoveHierarchy(hierarchy);
                }

                ConfigurationApplicationFile data = new ConfigurationApplicationFile(Path.GetDirectoryName(configurationFile), configurationFile);
                ConfigurationUIHierarchy newhierarchy = new ConfigurationUIHierarchy(new ConfigurationApplicationNode(data), serviceProvider);
                hierarchyService.AddHierarchy(newhierarchy);
                hierarchyService.SelectedHierarchy = newhierarchy;

                configurationEditor.SetMainHierarchy(newhierarchy);
                newhierarchy.Open();

				IErrorLogService errorLogService = GetService(typeof(IErrorLogService)) as IErrorLogService;
				if (errorLogService != null && errorLogService.ConfigurationErrorCount > 0)
				{
					configurationEditor.DisplayErrorLog(errorLogService);
				}

                configurationEditor.ClearDirtyState();
            }
            finally
            {
                loading = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        public void NavigateTask(Task task)
        {
            IUIService uiService = ServiceHelper.GetUIService(serviceProvider);

            ConfigurationNodeTask nodeTask = task as ConfigurationNodeTask;

            uiService.ActivateNode(nodeTask.ConfigurationNode);
        }

        void OnComponentChanged(object sender,
                                ComponentChangedEventArgs e)
        {
            ConfigurationNode configurationNode = e.Component as ConfigurationNode;
            if (configurationNode != null)
            {
                IUIService uiService = ServiceHelper.GetUIService(serviceProvider);
                uiService.SetUIDirty(configurationNode.Hierarchy);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurationFile"></param>
        public bool Save(string configurationFile)
        {
            ConfigurationNode currentRootNode = ServiceHelper.GetCurrentRootNode(serviceProvider);

            SaveAsConfigurationApplicationNodeCommand saveConfigurationCommand = new SaveAsConfigurationApplicationNodeCommand(serviceProvider, configurationFile);
            saveConfigurationCommand.Execute(currentRootNode);

            if (saveConfigurationCommand.SaveSucceeded)
            {
                configurationEditor.ClearDirtyState();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<StatusTextChangedEventArgs> StatusTextChanged;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<TasksChangedEventArgs> TasksChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            ValidateNodeCommand validateCommand = new ValidateNodeCommand(serviceProvider);
            validateCommand.Execute(configurationEditor.MainHierarchy.RootNode);

            return validateCommand.ValidationSucceeded;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class ConfigurationNodeTask : Task
    {
        ConfigurationNode configurationNode;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurationNode"></param>
        /// <param name="message"></param>
        public ConfigurationNodeTask(ConfigurationNode configurationNode,
                                     string message)
            : base(message)
        {
            this.configurationNode = configurationNode;
        }

        /// <summary>
        /// 
        /// </summary>
        public ConfigurationNode ConfigurationNode
        {
            get { return configurationNode; }
        }
    }
}