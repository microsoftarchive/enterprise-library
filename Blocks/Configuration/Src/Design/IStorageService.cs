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

using System.Collections.Generic;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Provides a list of storage commands in the current <see cref="IConfigurationUIHierarchy"/>.
    /// </summary>    
    public interface IStorageService
    {
        /// <summary>
        /// When implemented by a class, gets or sets the meta configuration file.
        /// </summary>
        /// <value>
        /// The meta configuration file.
        /// </value>
        /// <remarks>
        /// This file is used to store the meta data for your configuration describing the information around the configuration.
        /// </remarks>
        string ConfigurationFile { get; set; }

		/// <summary>
		/// When implemented by a class, performs the specified action on each item.
		/// </summary>
		/// <param name="action">The Action to perform on each element.</param>
		void ForEach(Action<StorageCreationCommand> action);

        /// <summary>
        /// When implemented by a class, adds a <see cref="StorageCreationCommand"/> to the table.
        /// </summary>
        /// <param name="storageCreationCommand">
        /// The <see cref="StorageCreationCommand"/> to add.
        /// </param>
        void Add(StorageCreationCommand storageCreationCommand);

        /// <summary>
        /// When implemented by a class, determines if a <see cref="StorageCreationCommand"/> exits table.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="StorageCreationCommand"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="StorageCreationCommand"/> exists; otherwise, <see langword="false"/>.
        /// </returns>
        bool Contains(string name);

        /// <summary>
        /// When implemented by a class, removes a <see cref="StorageCreationCommand"/> from the document
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="StorageCreationCommand"/>.
        /// </param>        
        void Remove(string name);

		/// <summary>
		/// When implemented by a class, clears all the commands in the list.
		/// </summary>
		void Clear();
    }
}
