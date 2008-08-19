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

using System.Collections;
using System.ComponentModel.Design;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Represents a service for <see cref="ConfigurationUICommand"/> objects.
	/// </summary>
    public interface IUICommandService
    {
        /// <summary>
		/// When implemented by a class, add a <see cref="ConfigurationUICommand"/> for a particular <see cref="Type"/>.
        /// </summary>
		/// <param name="type">The <see cref="Type"/> the command applies.</param>
        /// <param name="command">The <see cref="ConfigurationUICommand"/> to add.</param>
        void AddCommand(Type type, ConfigurationUICommand command);

		/// <summary>
		/// When implemented by a class, remove a <see cref="ConfigurationUICommand"/> for a particular <see cref="Type"/>.
		/// </summary>
		/// <param name="type">The <see cref="Type"/> the command applies.</param>
		/// <param name="command">The <see cref="ConfigurationUICommand"/> to Remove.</param>
		void RemoveCommand(Type type, ConfigurationUICommand command);

		/// <summary>
		/// When implemented by a class, performs the specified action on each item.
		/// </summary>		
		/// <param name="type">The <see cref="Type"/> the command applies.</param>
		/// <param name="action">The Action to perform on each element.</param>
		void ForEach(Type type, Action<ConfigurationUICommand> action);
    }
}