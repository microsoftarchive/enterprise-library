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
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Integration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Console.Hosting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.HostAdapter;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting
{
    /// <summary>
    /// An <see cref="ISingleHierarchyConfigurationUIHostAdapter"/> implementation which can be used to load the configuration UI inside an external tool, e.g: Visual Studio.
    /// </summary>
    /// <remarks>
    /// The <see cref="ISingleHierarchyConfigurationUIHostAdapter"/> implementation has System.ComponentModel support, which allows the host to use common components such as the <see cref="PropertyGrid"/> to interact with its elements.
    /// </remarks>
    public class SingleHierarchyConfigurationUIHostAdapter : ISingleHierarchyConfigurationUIHostAdapter, IWindowsFormsEditorService, IUIServiceWpf, IAssemblyDiscoveryService, IDisposable
    {
        readonly ConfigurationContainer container;
        readonly ElementHost elementHost;
        readonly ConfigurationEditorUI editorUI;
        readonly IApplicationModel applicationModel;
        readonly UserControl editorControl;
        readonly ValidationModel validationModel;
        readonly INotifyCollectionChanged validationErrorsChanged;
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="SingleHierarchyConfigurationUIHostAdapter"/>.
        /// </summary>
        /// <param name="hostConfiguration">The <see cref="HostAdapterConfiguration"/> that contains information on which assemblies to load into the designer.</param>
        /// <param name="serviceProvider">An <see cref="IServiceProvider"/> used to obtain services.</param>
        public SingleHierarchyConfigurationUIHostAdapter(HostAdapterConfiguration hostConfiguration, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            editorUI = new ConfigurationEditorUI();
            elementHost = new ElementHost { Child = editorUI, Dock = DockStyle.Fill };

            editorControl = new UserControl() { Dock = DockStyle.Fill };
            editorControl.Controls.Add(elementHost);

            container = new ConfigurationContainer(serviceProvider);
            container.RegisterInstance(new AssemblyLocator(hostConfiguration.PluginDirectory));
            container.RegisterInstance<IWindowsFormsEditorService>(this);
            container.RegisterInstance<IUIServiceWpf>(this);
            container.RegisterInstance<IUIService>(this);
            container.RegisterInstance<IAssemblyDiscoveryService>(this);

            container.DiscoverSubstituteTypesFromAssemblies();

            elementHost.Resize += ElementHostResize;

            applicationModel = container.Resolve<IApplicationModel>();
            editorUI.DataContext = applicationModel;

            validationModel = applicationModel.ValidationModel;

            applicationModel.SelectedElementChanged += SelectedElementChanged;

            validationErrorsChanged = validationModel.ValidationResults as INotifyCollectionChanged;
            if (validationErrorsChanged != null)
            {
                validationErrorsChanged.CollectionChanged += ValidationErrorsCollectionChanged;
            }
        }

        void ElementHostResize(object sender, EventArgs e)
        {
            editorUI.UpdateLayout();
        }

        void SelectedElementChanged(object sender, SelectedElementChangedEventHandlerEventArgs e)
        {
            var handler = SelectionChanged;
            if (handler != null)
            {
                handler(this, new SelectionChangedEventArgs(new ComponentModelElement(e.SelectedElement, this)));
            }
        }

        void ValidationErrorsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var tasksChangedHandler = TasksChanged;
            if (tasksChangedHandler != null)
            {
                tasksChangedHandler(this,
                    new TasksChangedEventArgs(validationModel.ValidationResults.Select(x => new TaskImpl(x)).Cast<Task>()));
            }
        }

#pragma warning disable 67
        /// <summary>
        /// This event is not used.
        /// </summary>
        public event EventHandler<EventArgs> DocumentClosed;
#pragma warning restore 67

        /// <summary>
        /// Gets an <see cref="UserControl"/> that can be used to display the designer.
        /// </summary>
        /// <value>
        /// An <see cref="UserControl"/> that can be used to display the designer.
        /// </value>
        public System.Windows.Forms.UserControl EditorControl
        {
            get { return editorControl; }
        }

        /// <summary>
        /// Gets a <see cref="bool"/> value indicating whether the designer has unsaved changes.
        /// </summary>
        /// <value>
        /// <see langref="true"/> if the designer has unsaved changes; Otherwise <see langref="false"/>.
        /// </value>
        public bool IsDirty
        {
            get { return applicationModel.IsDirty; }
        }

        /// <summary>
        /// Loads a configuration file into the designer.
        /// </summary>
        /// <param name="configurationFile">The path to the configuration file that should be loaded into the designer.</param>
        public void Load(string configurationFile)
        {
            applicationModel.Load(configurationFile);
        }

        /// <summary>
        /// Orients the editor towards a <see cref="Task"/>, typically error or warning, obtained by subscribing to the <see cref="TasksChanged"/> event.
        /// </summary>
        /// <param name="task">The <see cref="Task"/> that should be oriented to.</param>
        public void NavigateTask(Task task)
        {
            TaskImpl taskImpl = task as TaskImpl;
            if (taskImpl != null)
            {
                validationModel.Navigate(taskImpl.ValidationResult);
            }
        }

        /// <summary>
        /// Saves the configuration in the designer to a file.
        /// </summary>
        /// <param name="configurationFile">The path of the file the configuration should be saved to.</param>
        /// <see langref="true"/> if the save operation completed successfully; Otherwise <see langref="false"/>.
        public bool Save(string configurationFile)
        {
            return applicationModel.Save(configurationFile);
        }

        /// <summary>
        /// Occurs if the selected <see cref="ElementViewModel"/> changed in the designer.
        /// </summary>
        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

#pragma warning disable 67
        /// <summary>
        /// This event is not used.
        /// </summary>
        public event EventHandler<StatusTextChangedEventArgs> StatusTextChanged;
#pragma warning restore 67

        /// <summary>
        /// Occurs if the list of <see cref="Task"/>'s changed in the designer.
        /// </summary>
        /// <remarks>
        /// Tasks are typically validation errors or warnings.
        /// </remarks>
        public event EventHandler<TasksChangedEventArgs> TasksChanged;

        /// <summary>
        /// Performs validation on the elements inside the designer.
        /// </summary>
        /// <see langref="true"/> if no validation errors where found; Otherwise <see langref="false"/>.
        public bool Validate()
        {
            return validationModel.ValidationResults.Where(x => x.IsError).Count() == 0;
        }

        #region IServiceProvider Members

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="serviceType"/>.</returns>
        public object GetService(Type serviceType)
        {
            return container.Resolve(serviceType);
        }

        #endregion


        #region IDisposable Members

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="SingleHierarchyConfigurationUIHostAdapter"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources. </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (validationErrorsChanged != null)
                {
                    validationErrorsChanged.CollectionChanged -= ValidationErrorsCollectionChanged;
                }

                applicationModel.SelectedElementChanged -= SelectedElementChanged;
                elementHost.Resize -= ElementHostResize;

                container.Dispose();
                editorControl.Dispose();
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="ComponentModelElement"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion


        private class TaskImpl : Task, ITaskV5
        {
            ValidationResult validationResult;
            public TaskImpl(ValidationResult validationResult)
                : base(GetMessage(validationResult))
            {
                this.validationResult = validationResult;
            }

            private static string GetMessage(ValidationResult validationResult)
            {
                var messageBuilder = new StringBuilder();
                var elementName = validationResult.ElementName;
                var propertyName = validationResult.PropertyName;

                messageBuilder.Append(elementName);

                if (!string.IsNullOrEmpty(propertyName))
                {
                    messageBuilder.Append(" (");
                    messageBuilder.Append(propertyName);
                    messageBuilder.Append(")");
                }

                messageBuilder.Append(": ");

                messageBuilder.Append(validationResult.Message);

                return messageBuilder.ToString();
            }

            public ValidationResult ValidationResult
            {
                get { return validationResult; }
            }

            #region ITaskV5 Members

            public bool IsError
            {
                get { return validationResult.IsError; }
            }

            #endregion
        }

        #region IWindowsFormsEditorService Members

        /// <summary>
        /// Displays the specified control in a drop down area below a value field of the property grid that provides this service.
        /// </summary>
        /// <remarks>
        /// This method is not implemented for the <see cref="SingleHierarchyConfigurationUIHostAdapter"/> implementation.
        /// </remarks>
        public void CloseDropDown()
        {
            return;
        }

        /// <summary>
        /// Closes any previously opened drop down control area.
        /// </summary>
        /// <remarks>
        /// This method is not implemented for the <see cref="SingleHierarchyConfigurationUIHostAdapter"/> implementation.
        /// </remarks>
        public void DropDownControl(Control control)
        {
            return;
        }

        /// <summary>
        /// Shows the specified <see cref="Form"/>.
        /// </summary>
        /// <param name="dialog">The <see cref="Form"/> to display.</param>
        /// <returns>A <see cref="DialogResult" /> indicating the result code returned by the <see cref="Form"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public DialogResult ShowDialog(Form dialog)
        {
            Guard.ArgumentNotNull(dialog, "dialog");

            return dialog.ShowDialog();
        }

        #endregion

        bool? IUIServiceWpf.ShowDialog(System.Windows.Window dialog)
        {
            return GetService<IUIServiceWpf>().ShowDialog(dialog);
        }

        FileDialogResult IUIServiceWpf.ShowFileDialog(Microsoft.Win32.FileDialog dialog)
        {
            return GetService<IUIServiceWpf>().ShowFileDialog(dialog);
        }

        System.Windows.MessageBoxResult IUIServiceWpf.ShowMessageWpf(string message, string caption, System.Windows.MessageBoxButton buttons)
        {
            return GetService<IUIServiceWpf>().ShowMessageWpf(message, caption, buttons);
        }

        /// <summary>
        /// Shows a <see cref="Window"/> to the user.
        /// </summary>
        /// <param name="window">An instance of the <see cref="Window"/> to show.</param>
        public void ShowWindow(Window window)
        {
            GetService<IUIServiceWpf>().ShowWindow(window);
        }

        bool IUIService.CanShowComponentEditor(object component)
        {
            return GetService<IUIService>().CanShowComponentEditor(component);
        }

        IWin32Window IUIService.GetDialogOwnerWindow()
        {
            return GetService<IUIService>().GetDialogOwnerWindow();
        }

        void IUIService.SetUIDirty()
        {
            GetService<IUIService>().SetUIDirty();
        }

        bool IUIService.ShowComponentEditor(object component, IWin32Window parent)
        {
            return GetService<IUIService>().ShowComponentEditor(component, parent);
        }

        DialogResult IUIService.ShowDialog(Form form)
        {
            return GetService<IUIService>().ShowDialog(form);
        }

        void IUIService.ShowError(Exception ex, string message)
        {
            GetService<IUIService>().ShowError(ex, message);
        }

        void IUIService.ShowError(Exception ex)
        {
            GetService<IUIService>().ShowError(ex);
        }

        void IUIService.ShowError(string message)
        {
            GetService<IUIService>().ShowError(message);
        }

        DialogResult IUIService.ShowMessage(string message, string caption, MessageBoxButtons buttons)
        {
            return GetService<IUIService>().ShowMessage(message, caption, buttons);
        }

        void IUIService.ShowMessage(string message, string caption)
        {
            GetService<IUIService>().ShowMessage(message, caption);
        }

        void IUIService.ShowMessage(string message)
        {
            GetService<IUIService>().ShowMessage(message);
        }

        bool IUIService.ShowToolWindow(Guid toolWindow)
        {
            return GetService<IUIService>().ShowToolWindow(toolWindow);
        }

        System.Collections.IDictionary IUIService.Styles
        {
            get { return GetService<IUIService>().Styles; }
        }

        System.Collections.Generic.IDictionary<string, System.Collections.Generic.IEnumerable<System.Reflection.Assembly>> IAssemblyDiscoveryService.GetAvailableAssemblies()
        {
            return GetService<IAssemblyDiscoveryService>().GetAvailableAssemblies();
        }

        bool IAssemblyDiscoveryService.SupportsAssemblyLoading
        {
            get { return GetService<IAssemblyDiscoveryService>().SupportsAssemblyLoading; }
        }

        private T GetService<T>()
            where T : class
        {
            var service = this.serviceProvider.GetService(typeof(T)) as T;
            if (service == null)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture, Resources.ServiceNotFound, typeof(T).FullName));
            }
            return service;
        }
    }
}
