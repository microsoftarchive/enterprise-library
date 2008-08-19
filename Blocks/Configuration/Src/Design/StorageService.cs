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

using System.Globalization;
using System.IO;
using System;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Provides a list of storage commands in the current <see cref="IConfigurationUIHierarchy"/>.
	/// </summary>
    public class StorageService : IStorageService
    {
        internal Dictionary<string, StorageCreationCommand> storageCommands;
        private string configurationFile;
        
		/// <summary>
		/// Initialize a new instance of the <see cref="StorageService"/> class.
		/// </summary>
        public StorageService()
        {
            configurationFile = string.Empty;
			storageCommands = new Dictionary<string, StorageCreationCommand>();
        }

		/// <summary>
		/// Gets or sets the meta configuration file.
		/// </summary>
		/// <value>
		/// The meta configuration file.
		/// </value>
		/// <remarks>
		/// This file is used to store the meta data for your configuration describing the information around the configuration.
		/// </remarks>
        public string ConfigurationFile
        {
            get { return configurationFile; }
            set
            {
                if (string.IsNullOrEmpty(value)) return;

                if (!Path.IsPathRooted(value))
                {
                    value = Path.GetFullPath(value);
                }
                configurationFile = value.ToLower(CultureInfo.InvariantCulture);
            }
        }

		/// <summary>
		/// Performs the specified action on each item.
		/// </summary>
		/// <param name="action">The Action to perform on each element.</param>
        public void ForEach(Action<StorageCreationCommand> action)
        {
			foreach (StorageCreationCommand command in storageCommands.Values)
			{
				action(command);
			}
        }

		/// <summary>
		/// Adds a <see cref="StorageCreationCommand"/> to the table.
		/// </summary>
		/// <param name="storageCreationCommand">
		/// The <see cref="StorageCreationCommand"/> to add.
		/// </param>
        public void Add(StorageCreationCommand storageCreationCommand)
        {
           storageCommands.Add(storageCreationCommand.Name.ToLower(CultureInfo.InvariantCulture),  storageCreationCommand);
        }

		/// <summary>
		/// Determines if a <see cref="StorageCreationCommand"/> exits table.
		/// </summary>
		/// <param name="name">
		/// The name of the <see cref="StorageCreationCommand"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the <see cref="StorageCreationCommand"/> exists; otherwise, <see langword="false"/>.
		/// </returns>
        public bool Contains(string name)
        {
            return storageCommands.ContainsKey(name.ToLower(CultureInfo.InvariantCulture));
        }

		/// <summary>
		/// Removes a <see cref="StorageCreationCommand"/> from the document
		/// </summary>
		/// <param name="name">
		/// The name of the <see cref="StorageCreationCommand"/>.
		/// </param> 
        public void Remove(string name)
        {
            storageCommands.Remove(name.ToLower(CultureInfo.InvariantCulture));
        }

		/// <summary>
		/// Clears all the commands in the list.
		/// </summary>
		public void Clear()
		{
			storageCommands.Clear();
		}
    }
}