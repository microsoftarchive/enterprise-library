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
	/// Provides a container and management for <see cref="IConfigurationUIHierarchy"/> objects.
	/// </summary>
    public interface IConfigurationUIHierarchyService : IDisposable
    {
		/// <summary>
		/// When implemented by a class, occurs after an <see cref="IConfigurationUIHierarchy"/> is added.
		/// </summary>
		event EventHandler<HierarchyAddedEventArgs> HierarchyAdded;

		/// <summary>
		/// When implemented by a class, occurs after an <see cref="IConfigurationUIHierarchy"/> is removed.
		/// </summary>
		event EventHandler<HierarchyRemovedEventArgs> HierarchyRemoved;

		/// <summary>
		/// When implemented by a class, gets or sets the current selected hierarchy.
		/// </summary>
		/// <value>
		/// The current selected hierarchy.
		/// </value>
        IConfigurationUIHierarchy SelectedHierarchy { get; set; }

		/// <summary>
		/// When implemented by a class, adds a hierarchy to the container.
		/// </summary>
		/// <param name="hierarchy">
		/// The hierarchy to add.
		/// </param>
        void AddHierarchy(IConfigurationUIHierarchy hierarchy);

		/// <summary>
		/// When implemented by a class, gets a hierarchy from the container.
		/// </summary>
		/// <param name="id">The identifier for the hierarchy.</param>
		/// <returns>
		/// The <see cref="IConfigurationUIHierarchy"/> if found, or <see langword="null"/> if not found.
		/// </returns>
        IConfigurationUIHierarchy GetHierarchy(Guid id);

		/// <summary>
		/// When implemented by a class, gets all the hierarchies in the service.
		/// </summary>
		/// <returns>
		/// All the hierarchies in the service.
		/// </returns>
        IConfigurationUIHierarchy[] GetAllHierarchies();

		/// <summary>
		/// When implemented by a class, removes a hierarchy from the container.
		/// </summary>
		/// <param name="id">
		/// The identifier of the hierarchy to remove.
		/// </param>
        void RemoveHierarchy(Guid id);

		/// <summary>
		/// When implemented by a class, removes a hierarchy from the container.
		/// </summary>
		/// <param name="hierarchy">
		/// The hierarchy to remove.
		/// </param>
        void RemoveHierarchy(IConfigurationUIHierarchy hierarchy);

		/// <summary>
		/// When implemented by a class, saves all the hierarchies by calling save on all <see cref="IConfigurationUIHierarchy"/>.
		/// </summary>
        void SaveAll();
    }
}
