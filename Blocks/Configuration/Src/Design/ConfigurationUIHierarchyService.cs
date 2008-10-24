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
using System.Collections.Generic;
using System.Collections;
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Provides a container and management for <see cref="IConfigurationUIHierarchy"/> objects.
	/// </summary>
    public class ConfigurationUIHierarchyService : IConfigurationUIHierarchyService
    {
        internal Dictionary<Guid, IConfigurationUIHierarchy> hierarchies;
        private IConfigurationUIHierarchy selectedHierarchy;
        private static object hierarchyAddedEvent = new object();
        private static object hierarchyRemovedEvent = new object();
        private EventHandlerList handlerList;
        
		/// <summary>
		/// Initialize a new instance of the <see cref="ConfigurationUIHierarchyService"/> class.
		/// </summary>
        public ConfigurationUIHierarchyService()
        {
            hierarchies = new Dictionary<Guid,IConfigurationUIHierarchy>();
            handlerList = new EventHandlerList();
        }

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="ConfigurationUIHierarchyService"/> and optionally releases the managed resources.
		/// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="ConfigurationUIHierarchyService"/> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
		/// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                handlerList.Dispose();
                foreach (IConfigurationUIHierarchy hierarchy in hierarchies.Values)
                {
                    IDisposable disposable = hierarchy as IDisposable;
                    if (disposable != null) disposable.Dispose();
                }
            }
        }

		/// <summary>
		/// Occurs after an <see cref="IConfigurationUIHierarchy"/> is added.
		/// </summary>
        public event EventHandler<HierarchyAddedEventArgs> HierarchyAdded
        {
            add { handlerList.AddHandler(hierarchyAddedEvent, value); }
            remove { handlerList.RemoveHandler(hierarchyAddedEvent, value); }
        }

        /// <summary>
        /// Occurs when a hierarchy is removed.
        /// </summary>
        public event EventHandler<HierarchyRemovedEventArgs> HierarchyRemoved
        {
            add { handlerList.AddHandler(hierarchyRemovedEvent, value); }
            remove { handlerList.RemoveHandler(hierarchyRemovedEvent, value); }
        }

		/// <summary>
		/// Gets or sets the current selected hierarchy.
		/// </summary>
		/// <value>
		/// The current selected hierarchy.
		/// </value>
        public IConfigurationUIHierarchy SelectedHierarchy
        {
            get { return selectedHierarchy; }
            set { selectedHierarchy = value; }
        }

		/// <summary>
		/// Adds a hierarchy to the container.
		/// </summary>
		/// <param name="hierarchy">
		/// The hierarchy to add.
		/// </param>
        public void AddHierarchy(IConfigurationUIHierarchy hierarchy)
        {
            hierarchy.RootNode.Name = CreateUniqueHierarchyRootNode(hierarchy.RootNode.Name);
            hierarchies.Add(hierarchy.Id, hierarchy);

            if (selectedHierarchy == null)
            {
                selectedHierarchy = hierarchy;
            }
            OnHierarchyAdded(new HierarchyAddedEventArgs(hierarchy));
        }

        private string CreateUniqueHierarchyRootNode(string name)
        {
            int namePostFix = 1;
            string newName = name;

            IConfigurationUIHierarchy[] currentHierarchies = GetAllHierarchies();

            while (null != Array.Find<IConfigurationUIHierarchy>(currentHierarchies, delegate(IConfigurationUIHierarchy hierarchy) { return hierarchy.RootNode != null && string.Compare(hierarchy.RootNode.Name, newName) == 0; }))
            {
                
                newName = string.Concat(name, namePostFix.ToString(Thread.CurrentThread.CurrentUICulture));
                namePostFix++;
            }
            return newName;
        }

		/// <summary>
		/// Gets a hierarchy from the container.
		/// </summary>
		/// <param name="id">The identifier for the hierarchy.</param>
		/// <returns>
		/// The <see cref="IConfigurationUIHierarchy"/> if found, or <see langword="null"/> if not found.
		/// </returns>
        public IConfigurationUIHierarchy GetHierarchy(Guid id)
        {
            if (!hierarchies.ContainsKey(id)) return null;
            
            return hierarchies[id];
        }

		/// <summary>
		/// Gets all the hierarchies in the service.
		/// </summary>
		/// <returns>
		/// All the hierarchies in the service.
		/// </returns>
        public IConfigurationUIHierarchy[] GetAllHierarchies()
        {
            ArrayList list = new ArrayList(hierarchies.Values);
            return (IConfigurationUIHierarchy[])list.ToArray(typeof(IConfigurationUIHierarchy));
        }

		/// <summary>
		/// Removes a hierarchy from the container.
		/// </summary>
		/// <param name="id">
		/// The identifier of the hierarchy to remove.
		/// </param>
        public void RemoveHierarchy(Guid id)
        {
            IConfigurationUIHierarchy hierarchy = hierarchies[id] as IConfigurationUIHierarchy;
            if (hierarchy == null)
            {
                return;
            }
            hierarchies.Remove(id);
            OnHierarchyRemoved(new HierarchyRemovedEventArgs(hierarchy));
        }

		/// <summary>
		/// Removes a hierarchy from the container.
		/// </summary>
		/// <param name="hierarchy">
		/// The hierarchy to remove.
		/// </param>
        public void RemoveHierarchy(IConfigurationUIHierarchy hierarchy)
        {
			if (null == hierarchy) throw new ArgumentNullException("hierarchy");

            RemoveHierarchy(hierarchy.Id);
        }

		/// <summary>
		/// Saves all the hierarchies by calling save on all <see cref="IConfigurationUIHierarchy"/>.
		/// </summary>
        public void SaveAll()
        {
            foreach (IConfigurationUIHierarchy hierarchy in hierarchies.Values)
            {
                hierarchy.Save();
            }
        }

        private void OnHierarchyAdded(HierarchyAddedEventArgs e)
        {
			EventHandler<HierarchyAddedEventArgs> handler = (EventHandler<HierarchyAddedEventArgs>)handlerList[hierarchyAddedEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnHierarchyRemoved(HierarchyRemovedEventArgs e)
        {
			EventHandler<HierarchyRemovedEventArgs> handler = (EventHandler<HierarchyRemovedEventArgs>)handlerList[hierarchyRemovedEvent];
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
