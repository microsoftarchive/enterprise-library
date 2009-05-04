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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Represents a command to execute against a <see cref="ConfigurationNode"/>.
    /// </summary>
    public abstract class ConfigurationNodeCommand : IDisposable
    {
        private static readonly object ExecutingEvent = new object();
        private static readonly object ExecutedEvent = new object();

        private EventHandlerList events;
        private IServiceProvider serviceProvider;
        private bool clearErrorLog;

        /// <summary>
        /// Initialize a new instance of the <see cref="ConfigurationNodeCommand"/> class with an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        protected ConfigurationNodeCommand(IServiceProvider serviceProvider) : this(serviceProvider, true)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ConfigurationNodeCommand"/> class with an <see cref="IServiceProvider"/> and if the error service should be cleared after the command executes.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        /// <param name="clearErrorLog">
        /// Determines if all the messages in the <see cref="IErrorLogService"/> should be cleared when the command has executed.
        /// </param>
        protected ConfigurationNodeCommand(IServiceProvider serviceProvider, bool clearErrorLog)
        {
            if (serviceProvider == null) { throw new ArgumentNullException("serviceProvider"); }

            this.serviceProvider = serviceProvider;
            this.clearErrorLog = clearErrorLog;
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="ConfigurationNodeCommand "/> and optionally releases the managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="ConfigurationNodeCommand "/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != events)
                {
                    events.Dispose();
                }
            }
        }

        /// <summary>
        /// Occurs as the command is about to be executed.
        /// </summary>
        public event EventHandler<CommandExecutingEventArgs> Executing
        {
            add { Events.AddHandler(ExecutingEvent, value); }
            remove { Events.RemoveHandler(ExecutingEvent, value); }
        }

        /// <summary>
        /// Occurs after the commmand has completed execution.
        /// </summary>
        public event EventHandler Executed
        {
            add { Events.AddHandler(ExecutedEvent, value); }
            remove { Events.RemoveHandler(ExecutedEvent, value); }
        }        

        /// <summary>
        /// Gets the list of event handlers that are attached to this command.
        /// </summary>
        /// <value>
        /// An <see cref="EventHandlerList"/> that provides the delegates for this component
        /// </value>
        protected EventHandlerList Events
        {
            get
            {
                if (events == null)
                {
                    events = new EventHandlerList();
                }
                return events;
            }
        }

        /// <summary>
        /// Gets the service provider for the command.
        /// </summary>
        /// <value>
        /// The service provider for the command.
        /// </value>
        protected IServiceProvider ServiceProvider
        {
            get { return serviceProvider; }
        }

        /// <summary>
        /// Gets the <see cref="IUIService"/>.
        /// </summary>
        /// <value>
        /// The <see cref="IUIService"/>.
        /// </value>
        protected IUIService UIService
        {
            get { return ServiceHelper.GetUIService(serviceProvider); }
        }

        /// <summary>
		/// Gets the <see cref="IConfigurationUIHierarchyService"/>.
        /// </summary>
        /// <value>
		/// The <see cref="IConfigurationUIHierarchyService"/>.
        /// </value>
        protected IConfigurationUIHierarchyService ConfigurationUIHierarchyService
        {
            get { return ServiceHelper.GetUIHierarchyService(serviceProvider); }
        }

        /// <summary>
		/// Gets the currently selected <see cref="IConfigurationUIHierarchy"/>.
        /// </summary>
        /// <value>
		/// The currently selected <see cref="IConfigurationUIHierarchy"/>.
        /// </value>
        protected IConfigurationUIHierarchy CurrentHierarchy
        {
            get { return ServiceHelper.GetCurrentHierarchy(serviceProvider); } 
        }       

        /// <summary>
        /// Gets the <see cref="IErrorLogService"/>.
        /// </summary>
        /// <value>
        /// The <see cref="IErrorLogService"/>.
        /// </value>
        protected IErrorLogService ErrorLogService
        {
            get { return ServiceHelper.GetErrorService(serviceProvider); }
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="node">
        /// The node to execute on the command on.
        /// </param>
        public void Execute(ConfigurationNode node)
        {
            CommandExecutingEventArgs e = new CommandExecutingEventArgs();
            OnExecuting(e);
            if (e.Cancel)
            {
                return;
            }
            UIService.ClearErrorDisplay();
            ExecuteCore(node);			
            OnExecuted(new EventArgs());
            ClearErrorsBeforeExitingCommand();
        }

        /// <summary>
        /// When overridden by a class, executes the core logic of the command.
        /// </summary>
        /// <param name="node">
        /// The node to execute the command upon.
        /// </param>
        protected abstract void ExecuteCore(ConfigurationNode node);

        /// <summary>
        /// Raises the <see cref="Executing"/> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="CommandExecutingEventArgs"/> containing the event data.</param>
        protected virtual void OnExecuting(CommandExecutingEventArgs e)
        {
            if (events != null)
            {
				EventHandler<CommandExecutingEventArgs> handler = (EventHandler<CommandExecutingEventArgs>)events[ExecutingEvent];
                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="Executed"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> containing the event data.</param>
        protected virtual void OnExecuted(EventArgs e)
        {
            if (events != null)
            {
				EventHandler handler = (EventHandler)events[ExecutedEvent];
                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }

        private void ClearErrorsBeforeExitingCommand()
        {
            if (clearErrorLog == false)
            {
                return;
            }
            ErrorLogService.ClearErrorLog();
        }
    }
}
