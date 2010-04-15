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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    /// <summary>
    /// Represents a coordinator for configuration change notifications.
    /// </summary>
    public class ConfigurationChangeNotificationCoordinator
    {
        readonly EventHandlerList eventHandlers = new EventHandlerList();
        readonly object eventHandlersLock = new object();

        /// <summary>
        /// Add a section handler for a section.
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section.
        /// </param>
        /// <param name="handler">
        /// The handler to add.
        /// </param>
        public void AddSectionChangeHandler(string sectionName,
                                            ConfigurationChangedEventHandler handler)
        {
            lock (eventHandlersLock)
            {
                eventHandlers.AddHandler(CanonicalizeSectionName(sectionName), handler);
            }
        }

        static string CanonicalizeSectionName(string sectionName)
        {
            return String.Intern(sectionName);
        }

        /// <summary>
        /// Notify updated configuration sections.
        /// </summary>
        /// <param name="sectionsToNotify">
        /// The name of the sections to notify.
        /// </param>
        public void NotifyUpdatedSections(IEnumerable<string> sectionsToNotify)
        {
            foreach (string rawSectionName in sectionsToNotify)
            {
                String sectionName = CanonicalizeSectionName(rawSectionName);

                Delegate[] invocationList = null;

                lock (eventHandlersLock)
                {
                    ConfigurationChangedEventHandler callbacks = (ConfigurationChangedEventHandler)eventHandlers[sectionName];
                    if (callbacks == null)
                    {
                        continue;
                    }
                    invocationList = callbacks.GetInvocationList();
                }

                ConfigurationChangedEventArgs eventData = new ConfigurationChangedEventArgs(sectionName);
                foreach (ConfigurationChangedEventHandler callback in invocationList)
                {
                    try
                    {
                        if (callback != null)
                        {
                            callback(this, eventData);
                        }
                    }
                    catch (Exception e)
                    {
                        ManageabilityExtensionsLogger.LogException(e,
                                                                   String.Format(CultureInfo.CurrentCulture,
                                                                                 Resources.ExceptionErrorOnCallbackForSectionUpdate,
                                                                                 sectionName,
                                                                                 callback.ToString()));
                    }
                }
            }
        }

        /// <summary>
        /// Remove a section change handler.
        /// </summary>
        /// <param name="sectionName">
        /// The section to remove the handler.
        /// </param>
        /// <param name="handler">
        /// The handler to remove.
        /// </param>
        public void RemoveSectionChangeHandler(string sectionName,
                                               ConfigurationChangedEventHandler handler)
        {
            lock (eventHandlersLock)
            {
                eventHandlers.RemoveHandler(CanonicalizeSectionName(sectionName), handler);
            }
        }
    }
}
