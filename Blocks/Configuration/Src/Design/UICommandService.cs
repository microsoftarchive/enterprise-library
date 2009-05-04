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
using System.Collections.Generic;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Represents a service for <see cref="ConfigurationUICommand"/> objects.
	/// </summary>
    public class UICommandService : IUICommandService
    {
		private Dictionary<HierarchyAndType, List<ConfigurationUICommand> > list;
        private IConfigurationUIHierarchyService hierarchyService;
        
		/// <summary>
		/// Initialize a new instance of the <see cref="UICommandService"/> class with an <see cref="IConfigurationUIHierarchyService"/>.
		/// </summary>
		/// <param name="hierarchyService">A <see cref="IConfigurationUIHierarchyService"/> instance.</param>
        public UICommandService(IConfigurationUIHierarchyService hierarchyService)
        {
            this.hierarchyService = hierarchyService;
            list = new Dictionary<HierarchyAndType, List<ConfigurationUICommand>>();
        }

		/// <summary>
		/// Add a <see cref="ConfigurationUICommand"/> for a particular <see cref="Type"/>.
		/// </summary>
		/// <param name="type">The <see cref="Type"/> the command applies.</param>
		/// <param name="command">The <see cref="ConfigurationUICommand"/> to add.</param>
		public void AddCommand(Type type, ConfigurationUICommand command)
		{
            HierarchyAndType hierarchyAndType = new HierarchyAndType(hierarchyService.SelectedHierarchy, type);
            if (!list.ContainsKey(hierarchyAndType)) list.Add(hierarchyAndType, new List<ConfigurationUICommand>());
            List<ConfigurationUICommand> commands = list[hierarchyAndType];
			commands.Add(command);
		}

		/// <summary>
		/// Remove a <see cref="ConfigurationUICommand"/> for a particular <see cref="Type"/>.
		/// </summary>
		/// <param name="type">The <see cref="Type"/> the command applies.</param>
		/// <param name="command">The <see cref="ConfigurationUICommand"/> to Remove.</param>
		public void RemoveCommand(Type type, ConfigurationUICommand command)
		{
            HierarchyAndType hierarchyAndType = new HierarchyAndType(hierarchyService.SelectedHierarchy, type);
            if (!list.ContainsKey(hierarchyAndType)) return;
            List<ConfigurationUICommand> commands = list[hierarchyAndType];
			commands.Remove(command);
		}

		/// <summary>
		/// Performs the specified action on each item.
		/// </summary>		
		/// <param name="type">The <see cref="Type"/> the command applies.</param>
		/// <param name="action">The Action to perform on each element.</param>
		public void ForEach(Type type, Action<ConfigurationUICommand> action)
		{

            HierarchyAndType hierarchyAndType = new HierarchyAndType(hierarchyService.SelectedHierarchy, type);
            if (!list.ContainsKey(hierarchyAndType)) return;
            List<ConfigurationUICommand> commands = list[hierarchyAndType];
			for (int index = 0; index < commands.Count; index++)
			{
				action(commands[index]);
			}
		}

        private struct HierarchyAndType
        {
            IConfigurationUIHierarchy hierarchy;
            Type type;

            public HierarchyAndType(IConfigurationUIHierarchy hierarchy, Type type)
            {
                this.hierarchy = hierarchy;
                this.type = type;
            }
        }
	}
}
