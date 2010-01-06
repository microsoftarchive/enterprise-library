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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.HostAdapter;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using System.Windows.Forms.Integration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Console.Hosting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Console;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.HostAdapterV5;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting
{
    public class SingleHierarchyConfigurationUIHostAdapter : ISingleHierarchyConfigurationUIHostAdapter, IWindowsFormsEditorService
    {
        UnityContainer container;
        ConfigurationSourceModel sourceModel;
        ElementHost elementHost;
        ConfigurationEditorUI editorUI;
        IApplicationModel applicationModel;
        UserControl editorControl;
        ValidationModel validationModel;
        INotifyCollectionChanged validationErrorsChanged;

        public SingleHierarchyConfigurationUIHostAdapter(HostAdapterConfiguration hostConfiguration)
        {
            container = new UnityContainer();
            container.RegisterType<AssemblyLocator, BinPathProbingAssemblyLocator>(new ContainerControlledLifetimeManager());
            container.RegisterType<ConfigurationSectionLocator, AssemblyAttributeSectionLocator>(new ContainerControlledLifetimeManager());
            container.RegisterType<AnnotationService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ElementLookup>(new ContainerControlledLifetimeManager());
            container.RegisterType<ConfigurationSourceModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<MenuCommandService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ConfigurationSourceDependency>(new ContainerControlledLifetimeManager());
            container.RegisterType<ValidationModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<IApplicationModel, ApplicationViewModel>(new ContainerControlledLifetimeManager());

            container.RegisterInstance<AssemblyLocator>(new AssemblyLocator(hostConfiguration.PluginDirectory));
            container.RegisterInstance<IWindowsFormsEditorService>(this);

            container.RegisterInstance<IServiceProvider>(this);

            editorUI = new ConfigurationEditorUI();
            elementHost = new ElementHost { Child = editorUI, Dock = DockStyle.Fill};
            editorControl = new UserControl() { Dock = DockStyle.Fill};
            editorControl.Controls.Add(elementHost);

            container.RegisterInstance<IUIServiceWpf>(editorUI);
            container.RegisterInstance<IUIService>(editorUI);
            sourceModel = container.Resolve<ConfigurationSourceModel>();
            editorUI.DataContext = sourceModel;

            container.BuildUp(editorUI);
            elementHost.Resize += new EventHandler(elementHost_Resize);

            validationModel = container.Resolve<ValidationModel>();

            AnnotationService annotationService = container.Resolve<AnnotationService>();
            AppSettingsDecorator.DecorateAppSettingsSection(annotationService);
            ConnectionStringsDecorator.DecorateConnectionStringsSection(annotationService);

            applicationModel = container.Resolve<IApplicationModel>();
            applicationModel.SelectedElementChanged += new EventHandler<SelectedElementChangedEventHandlerArgs>(applicationModel_SelectedElementChanged);

            validationErrorsChanged = validationModel.ValidationErrors as INotifyCollectionChanged;
            if (validationErrorsChanged != null)
            {
                validationErrorsChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(validationErrorsChanged_CollectionChanged);
            }
        }

        void elementHost_Resize(object sender, EventArgs e)
        {
            editorUI.UpdateLayout();       
        }

        void applicationModel_SelectedElementChanged(object sender, SelectedElementChangedEventHandlerArgs e)
        {
            var handler = SelectionChanged;
            if (handler != null)
            {
                handler(this, new SelectionChangedEventArgs( new ComponentModelElement( e.SelectedElement, this )));
            }
        }

        void validationErrorsChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var tasksChangedHandler = TasksChanged;
            if (tasksChangedHandler != null)
            {
                tasksChangedHandler(this, 
                    new TasksChangedEventArgs(validationModel.ValidationErrors.Select(x=>new TaskImpl(x)).Cast<Task>()));
            }
        }

        public event EventHandler<EventArgs> DocumentClosed;

        public System.Windows.Forms.UserControl EditorControl
        {
            get { return editorControl; }
        }

        public bool IsDirty
        {
            get { return applicationModel.IsDirty; }
        }

        public void Load(string configurationFile)
        {
            applicationModel.Load(configurationFile);
        }

        public void NavigateTask(Task task)
        {

        }

        public bool Save(string configurationFile)
        {
            return applicationModel.Save(configurationFile);
        }

        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        public event EventHandler<StatusTextChangedEventArgs> StatusTextChanged;

        public event EventHandler<TasksChangedEventArgs> TasksChanged;

        public bool Validate()
        {
            return validationModel.ValidationErrors.Where(x=>x.IsError).Count() == 0;
        }

        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            return container.Resolve(serviceType);
        }

        #endregion


        private class TaskImpl : Task, ITaskV5
        {
            ValidationError validationError;
            public TaskImpl(ValidationError validationError)
                :base(validationError.Message)
            {
                this.validationError = validationError;
            }

            public ValidationError ValidationError
            {
                get { return validationError; }
            }

            #region ITaskV5 Members

            public bool IsError
            {
                get { return validationError.IsError; }
            }

            #endregion
        }

        #region IWindowsFormsEditorService Members

        public void CloseDropDown()
        {
            return;
        }

        public void DropDownControl(Control control)
        {
            return;
        }

        public DialogResult ShowDialog(Form dialog)
        {
            return dialog.ShowDialog();
        }

        #endregion
    }
}
